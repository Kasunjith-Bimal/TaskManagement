using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.Command.TaskReleted.UpdateTask;
using TaskManager.Application.Wrappers;
using TaskManager.Domain.Interfaces;
using TaskManager.Domain.Services;

namespace TaskManager.Application.Queries.TaskReleted.GetTaskById
{
    public class GetTaskById : IConsumer<GetTaskByIdQuery>
    {
        private readonly ILogger<GetTaskById> logger;
        private readonly ITaskService taskService;
        private readonly IConfiguration configuration;
        private readonly IJwtTokenManager jwtTokenManager;
        public GetTaskById(ILogger<GetTaskById> logger, ITaskService taskService, IConfiguration configuration, IJwtTokenManager jwtTokenManager)
        {
            this.logger = logger;
            this.taskService = taskService;
            this.configuration = configuration;
            this.jwtTokenManager = jwtTokenManager;
        }

        public async Task Consume(ConsumeContext<GetTaskByIdQuery> context)
        {
            try
            {
                this.logger.LogInformation($"[GetTaskById] Received event");
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
                                    this.logger.LogInformation($"[GetTaskById] TaskService GetTaskById method call");
                                    var findTask = await this.taskService.GetTaskByIdAsync(context.Message.Id);

                                    if (findTask != null)
                                    {
                                        this.logger.LogInformation($"[GetTaskById] Successfuly get task id {context.Message.Id}");


                                        if(findTask.CreateUserId == LogUserId)
                                        {
                                            var response = new GetTaskByIdResponse
                                            {
                                                task = findTask
                                            };

                                            await context.RespondAsync(ResponseWrapper<GetTaskByIdResponse>.Success("Successfuly get task", response));
                                        }
                                        else
                                        {
                                            this.logger.LogInformation($"[GetTaskById] You are not authorized to get this task {context.Message.Id}");
                                            await context.RespondAsync(ResponseWrapper<GetTaskByIdResponse>.Fail("You are not authorized to get this task"));
                                        }
                                       

                                    }
                                    else
                                    {
                                        this.logger.LogInformation($"[GetTaskById] Failed to get task id {context.Message.Id}");
                                        await context.RespondAsync(ResponseWrapper<GetTaskByIdResponse>.Fail("Failed to get task Invalid Task Id"));

                                    }
                                }
                                else
                                {
                                    this.logger.LogInformation($"[GetTaskById] Invalid Task Id {context.Message.Id}");
                                    await context.RespondAsync(ResponseWrapper<GetTaskByIdResponse>.Fail("Invalid Task Id"));
                                }

                            }
                            else
                            {
                                this.logger.LogInformation($"[GetTaskById] Invalid Token.");
                                await context.RespondAsync(ResponseWrapper<GetTaskByIdResponse>.Fail("Invalid Token."));
                            }
                        }
                        else
                        {
                            this.logger.LogInformation($"[GetTaskById] Invalid Token.");
                            await context.RespondAsync(ResponseWrapper<GetTaskByIdResponse>.Fail("Invalid Token."));
                        }
                    }
                    else
                    {
                        this.logger.LogInformation($"[GetTaskById] Invalid Token.");
                        await context.RespondAsync(ResponseWrapper<GetTaskByIdResponse>.Fail("Invalid Token."));
                    }
                }
                else
                {
                    this.logger.LogInformation($"[GetTaskById] Token is required.");
                    await context.RespondAsync(ResponseWrapper<GetTaskByIdResponse>.Fail("Token is required."));
                }
            }
            catch (Exception ex)
            {
                this.logger.LogDebug(ex, $"[GetTaskById] id {context.Message.Id} - exception occored. stacktrace: {ex.StackTrace}");
                await context.RespondAsync(ResponseWrapper<GetTaskByIdResponse>.Fail(ex.Message));
            }
        }
    }
}
