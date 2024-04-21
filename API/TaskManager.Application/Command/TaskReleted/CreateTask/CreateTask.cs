using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.Wrappers;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Domain.Services;


namespace TaskManager.Application.Command.TaskReleted.CreateTask
{
    public class CreateTask : IConsumer<CreateTaskCommand>
    {
        private readonly ILogger<CreateTask> logger;
        private readonly ITaskService taskService;
        private readonly IConfiguration configuration;
        private readonly IJwtTokenManager jwtTokenManager;
        public CreateTask(ILogger<CreateTask> logger, ITaskService taskService, IConfiguration configuration, IJwtTokenManager jwtTokenManager)
        {
            this.logger = logger;
            this.taskService = taskService;
            this.configuration = configuration;
            this.jwtTokenManager = jwtTokenManager;
        }

        public async Task Consume(ConsumeContext<CreateTaskCommand> context)
        {
            try
            {
                this.logger.LogInformation($"[CreateTask] Received event");
                // Check token
                if (!String.IsNullOrEmpty(context.Message.Token))
                {
                    ClaimsPrincipal tokenDecryptValues = new ClaimsPrincipal();
                    string tokenString = context.Message.Token.ToString().Replace("Bearer ", "");
                    tokenDecryptValues = await jwtTokenManager.DecodeJwtAccessToken(tokenString);


                    if(tokenDecryptValues != null)
                    {
                        var tokenPayLoadClims = tokenDecryptValues?.Claims;

                        if(tokenPayLoadClims != null)
                        {
                            string LogUserId = tokenPayLoadClims.Where(c => c.Type.Contains("nameidentifier")).FirstOrDefault().Value.ToString();

                            if (!string.IsNullOrEmpty(LogUserId))
                            {
                                if (!String.IsNullOrEmpty(context.Message.TaskDetail.Title))
                                {
                                    if (!String.IsNullOrEmpty(context.Message.TaskDetail.Description))
                                    {
                                        this.logger.LogInformation($"[CreateTask] TaskService addTask method call");

                                        var createTask = new TaskDetail
                                        {
                                            CreateUserId = LogUserId,
                                            Description = context.Message.TaskDetail.Description,
                                            Title = context.Message.TaskDetail.Title,
                                            DueDate = context.Message.TaskDetail.DueDate,
                                            Id = 0,
                                            IsComplete = context.Message.TaskDetail.IsComplete
                                        };

                                        var addedTaskDetail = this.taskService.AddTask(createTask);

                                        if (addedTaskDetail != null)
                                        {
                                            this.logger.LogInformation($"[CreateTask] Task added successfully task id : {addedTaskDetail.Id} title : {addedTaskDetail.Title}");

                                            var response = new CreateTaskResponse
                                            {
                                                task = addedTaskDetail
                                            };

                                            await context.RespondAsync(ResponseWrapper<CreateTaskResponse>.Success("Task added successfully.", response));

                                        }
                                        else
                                        {
                                            this.logger.LogInformation($"[CreateTask] Failed to Add task title {context.Message.TaskDetail.Title}");
                                            await context.RespondAsync(ResponseWrapper<CreateTaskResponse>.Fail("Failed to Add task."));

                                        }
                                    }
                                    else
                                    {
                                        this.logger.LogInformation($"[CreateTask] Description cannot be empty");
                                        await context.RespondAsync(ResponseWrapper<CreateTaskResponse>.Fail("Description cannot be empty"));

                                    }
                                }
                                else
                                {
                                    this.logger.LogInformation($"[CreateTask] Title cannot be empty");
                                    await context.RespondAsync(ResponseWrapper<CreateTaskResponse>.Fail("Title cannot be empty"));
                                }
                            }
                            else
                            {
                                this.logger.LogInformation($"[CreateTask] Invalid Token.");
                                await context.RespondAsync(ResponseWrapper<CreateTaskResponse>.Fail("Invalid Token."));
                            }
                        }
                        else
                        {
                            this.logger.LogInformation($"[CreateTask] Invalid Token.");
                            await context.RespondAsync(ResponseWrapper<CreateTaskResponse>.Fail("Invalid Token."));
                        }



                    }
                    else
                    {
                        this.logger.LogInformation($"[CreateTask] Invalid Token.");
                        await context.RespondAsync(ResponseWrapper<CreateTaskResponse>.Fail("Invalid Token."));
                    }

                    
                }
                else
                {
                    this.logger.LogInformation($"[CreateTask] Token is required.");
                    await context.RespondAsync(ResponseWrapper<CreateTaskResponse>.Fail("Token is required."));
                }

               
               
            }
            catch (Exception ex)
            {
                this.logger.LogDebug(ex, $"[CreateTask] Title: {context.Message.TaskDetail.Title} - exception occored. stacktrace: {ex.StackTrace}");
                await context.RespondAsync(ResponseWrapper<CreateTaskResponse>.Fail(ex.Message));
            }
        }
    }
}
