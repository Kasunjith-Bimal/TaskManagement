using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.Command.TaskReleted.DeleteTask;
using TaskManager.Application.Wrappers;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Domain.Services;


namespace TaskManager.Application.Command.TaskReleted.UpdateTask
{
    public class UpdateTask : IConsumer<UpdateTaskCommand>
    {
        private readonly ILogger<UpdateTask> logger;
        private readonly ITaskService taskService;
        private readonly IConfiguration configuration;
        private readonly IJwtTokenManager jwtTokenManager;

        public UpdateTask(ILogger<UpdateTask> logger, ITaskService taskService, IConfiguration configuration, IJwtTokenManager jwtTokenManager)
        {
            this.logger = logger;
            this.taskService = taskService;
            this.configuration = configuration;
            this.jwtTokenManager = jwtTokenManager;
        }

        public async Task Consume(ConsumeContext<UpdateTaskCommand> context)
        {
            try
            {
                this.logger.LogInformation($"[UpdateTask] Received event");

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
                                if (context.Message.TaskDetail.Id != 0)
                                {
                                    if (context.Message.TaskDetail.Id == context.Message.Id)
                                    {
                                        if (!String.IsNullOrEmpty(context.Message.TaskDetail.Title))
                                        {
                                            if (!String.IsNullOrEmpty(context.Message.TaskDetail.Description))
                                            {
                                                var getTask = await this.taskService.GetTaskByIdAsync(context.Message.Id);

                                                if (getTask != null)
                                                {
                                                    if (getTask.CreateUserId == LogUserId)
                                                    {
                                                        var UpdatetasK = new TaskDetail()
                                                        {
                                                            CreateUserId = getTask.CreateUserId,
                                                            Description = context.Message.TaskDetail.Description,
                                                            DueDate = context.Message.TaskDetail.DueDate,
                                                            IsComplete = context.Message.TaskDetail.IsComplete,
                                                            Title = context.Message.TaskDetail.Title,
                                                            Id = getTask.Id
                                                        };

                                                        this.logger.LogInformation($"[UpdateTask] TaskService UpdateTask method call taskid : {UpdatetasK.Id}");
                                                        var updatedTaskDetail = this.taskService.UpdateTask(UpdatetasK);

                                                        if (updatedTaskDetail != null)
                                                        {
                                                            this.logger.LogInformation($"[UpdateTask] Task update successfully task id : {updatedTaskDetail.Id} title : {updatedTaskDetail.Title}");

                                                            var response = new UpdateTaskResponse
                                                            {
                                                                task = updatedTaskDetail
                                                            };

                                                            await context.RespondAsync(ResponseWrapper<UpdateTaskResponse>.Success("Task update successfully.", response));

                                                        }
                                                        else
                                                        {
                                                            this.logger.LogInformation($"[UpdateTask] Failed to update task id ; {context.Message.TaskDetail.Id}");
                                                            await context.RespondAsync(ResponseWrapper<UpdateTaskResponse>.Fail("Failed to update task."));

                                                        }
                                                    }
                                                    else
                                                    {
                                                        this.logger.LogInformation($"[DeleteTask] Failed to update task id {context.Message.Id}");
                                                        await context.RespondAsync(ResponseWrapper<UpdateTaskResponse>.Fail("You are not authorized to update this task"));
                                                    }
                                                  
                                                }
                                                else
                                                {
                                                    this.logger.LogInformation($"[UpdateTask] Invalid Task Id ; {context.Message.TaskDetail.Id}");
                                                    await context.RespondAsync(ResponseWrapper<UpdateTaskResponse>.Fail("Invalid Task Id."));
                                                }
                                            }
                                            else
                                            {
                                                this.logger.LogInformation($"[UpdateTask] Description cannot be empty");
                                                await context.RespondAsync(ResponseWrapper<UpdateTaskResponse>.Fail("Description cannot be empty"));
                                            }
                                        }
                                        else
                                        {
                                            this.logger.LogInformation($"[UpdateTask] Title cannot be empty");
                                            await context.RespondAsync(ResponseWrapper<UpdateTaskResponse>.Fail("Title cannot be empty"));
                                        }
                                    }
                                    else
                                    {
                                        this.logger.LogInformation($"[UpdateTask] Id missmatch");
                                        await context.RespondAsync(ResponseWrapper<UpdateTaskResponse>.Fail("Id missmatch"));
                                    }
                                }
                                else
                                {
                                    this.logger.LogInformation($"[UpdateTask] Id cannot be empty");
                                    await context.RespondAsync(ResponseWrapper<UpdateTaskResponse>.Fail("Id cannot be empty"));
                                }
                            }
                            else
                            {
                                this.logger.LogInformation($"[UpdateTaskResponse] Invalid Token.");
                                await context.RespondAsync(ResponseWrapper<UpdateTaskResponse>.Fail("Invalid Token."));
                            }
                        }
                        else
                        {
                            this.logger.LogInformation($"[UpdateTaskResponse] Invalid Token.");
                            await context.RespondAsync(ResponseWrapper<UpdateTaskResponse>.Fail("Invalid Token."));
                        }
                    }
                    else
                    {
                        this.logger.LogInformation($"[UpdateTaskResponse] Invalid Token.");
                        await context.RespondAsync(ResponseWrapper<UpdateTaskResponse>.Fail("Invalid Token."));
                    }
                }
                else
                {
                    this.logger.LogInformation($"[UpdateTaskResponse] Token is required.");
                    await context.RespondAsync(ResponseWrapper<UpdateTaskResponse>.Fail("Token is required."));
                }

             

            }
            catch (Exception ex)
            {
                this.logger.LogDebug(ex, $"[UpdateTask] id : {context.Message.TaskDetail.Id} Title: {context.Message.TaskDetail.Title} - exception occored. stacktrace: {ex.StackTrace}");
                await context.RespondAsync(ResponseWrapper<UpdateTaskResponse>.Fail(ex.Message));
            }
        }
    }
}
