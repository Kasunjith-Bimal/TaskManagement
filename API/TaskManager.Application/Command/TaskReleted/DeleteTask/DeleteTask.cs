using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.Command.TaskReleted.CreateTask;
using TaskManager.Application.Wrappers;
using TaskManager.Domain.Interfaces;
using TaskManager.Domain.Services;


namespace TaskManager.Application.Command.TaskReleted.DeleteTask
{
    public class DeleteTask : IConsumer<DeleteTaskCommand>
    {
        private readonly ILogger<DeleteTask> logger;
        private readonly ITaskService taskService;
        private readonly IConfiguration configuration;
        private readonly IJwtTokenManager jwtTokenManager;

        public DeleteTask(ILogger<DeleteTask> logger, ITaskService taskService, IConfiguration configuration, IJwtTokenManager jwtTokenManager)
        {
            this.logger = logger;
            this.taskService = taskService;
            this.configuration = configuration;
            this.jwtTokenManager = jwtTokenManager;
        }

        public async Task Consume(ConsumeContext<DeleteTaskCommand> context)
        {
            try
            {
                this.logger.LogInformation($"[DeleteTask] Received event");
                if (!String.IsNullOrEmpty(context.Message.Token))
                {
                    ClaimsPrincipal tokenDecryptValues = new ClaimsPrincipal();
                    string tokenString = context.Message.Token.ToString().Replace("Bearer ", "");
                    tokenDecryptValues = await jwtTokenManager.DecodeJwtAccessToken(tokenString);


                    if (tokenDecryptValues != null)
                    {
                        var tokenPayLoadClims = tokenDecryptValues?.Claims;

                        if (tokenPayLoadClims != null)
                        {
                            string LogUserId = tokenPayLoadClims.Where(c => c.Type.Contains("nameidentifier")).FirstOrDefault().Value.ToString();

                            if (!string.IsNullOrEmpty(LogUserId))
                            {
                                if (context.Message.Id != 0)
                                {
                                    var getTask = await this.taskService.GetTaskByIdAsync(context.Message.Id);

                                    if (getTask != null)
                                    {
                                        //check task created user delete or not 
                                        if(getTask.CreateUserId == LogUserId)
                                        {
                                            this.logger.LogInformation($"[DeleteTask] TaskService addTask method call");
                                            var deleteTask = await this.taskService.DeleteTask(context.Message.Id);

                                            if (deleteTask)
                                            {
                                                this.logger.LogInformation($"[DeleteTask] Task delete successfully task id : {context.Message.Id}");

                                                var response = new DeleteTaskResponse
                                                {
                                                    IsDelete = deleteTask
                                                };

                                                await context.RespondAsync(ResponseWrapper<DeleteTaskResponse>.Success("Task delete successfully", response));

                                            }
                                            else
                                            {
                                                this.logger.LogInformation($"[DeleteTask] Failed to delete task id {context.Message.Id}");
                                                await context.RespondAsync(ResponseWrapper<DeleteTaskResponse>.Fail("Failed to delete task."));

                                            }
                                        }
                                        else
                                        {
                                            this.logger.LogInformation($"[DeleteTask] Failed to delete task id {context.Message.Id}");
                                            await context.RespondAsync(ResponseWrapper<DeleteTaskResponse>.Fail("You are not authorized to delete this task"));
                                        }

                                        
                                    }
                                    else
                                    {
                                        this.logger.LogInformation($"[DeleteTask] Invalid Task Id {context.Message.Id}");
                                        await context.RespondAsync(ResponseWrapper<DeleteTaskResponse>.Fail("Invalid Task Id."));
                                    }
                                }
                                else
                                {
                                    this.logger.LogInformation($"[DeleteTask] Id cannot be empty");
                                    await context.RespondAsync(ResponseWrapper<DeleteTaskResponse>.Fail("Id cannot be empty"));

                                }
                            }
                            else
                            {
                                this.logger.LogInformation($"[DeleteTask] Invalid Token.");
                                await context.RespondAsync(ResponseWrapper<DeleteTaskResponse>.Fail("Invalid Token."));
                            }
                        }
                        else
                        {
                            this.logger.LogInformation($"[DeleteTask] Invalid Token.");
                            await context.RespondAsync(ResponseWrapper<DeleteTaskResponse>.Fail("Invalid Token."));
                        }
                    }
                    else
                    {
                        this.logger.LogInformation($"[DeleteTask] Invalid Token.");
                        await context.RespondAsync(ResponseWrapper<DeleteTaskResponse>.Fail("Invalid Token."));

                    }
                }
                else
                {
                    this.logger.LogInformation($"[DeleteTask] Token is required.");
                    await context.RespondAsync(ResponseWrapper<DeleteTaskResponse>.Fail("Token is required."));
                }

               
            }
            catch (Exception ex)
            {
                this.logger.LogDebug(ex, $"[DeleteTask] id: {context.Message.Id} - exception occored. stacktrace: {ex.StackTrace}");
                await context.RespondAsync(ResponseWrapper<DeleteTaskResponse>.Fail(ex.Message));
            }
        }
    }
}
