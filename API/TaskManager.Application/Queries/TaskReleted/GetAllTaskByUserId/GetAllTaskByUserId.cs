using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.Queries.TaskReleted.GetTaskById;
using TaskManager.Application.Wrappers;
using TaskManager.Domain.Interfaces;
using TaskManager.Domain.Services;

namespace TaskManager.Application.Queries.TaskReleted.GetAllTaskByUserId
{
    public class GetAllTaskByUserId : IConsumer<GetAllTaskByUserIdQuery>
    {
        private readonly ILogger<GetAllTaskByUserId> logger;
        private readonly ITaskService taskService;
        private readonly IConfiguration configuration;
        private readonly IJwtTokenManager jwtTokenManager;
        public GetAllTaskByUserId(ILogger<GetAllTaskByUserId> logger, ITaskService taskService, IConfiguration configuration, IJwtTokenManager jwtTokenManager)
        {
            this.logger = logger;
            this.taskService = taskService;
            this.configuration = configuration;
            this.jwtTokenManager = jwtTokenManager;
        }

        public async Task Consume(ConsumeContext<GetAllTaskByUserIdQuery> context)
        {
            try
            {
                this.logger.LogInformation($"[GetAllTaskByUserId] Received event");

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

                                if (!string.IsNullOrEmpty(context.Message.UserId))
                                {
                                    if(context.Message.UserId == LogUserId)
                                    {
                                        this.logger.LogInformation($"[GetAllTaskByUserId] TaskService GetAllTaskByUserId method call");
                                        var findTasks = await this.taskService.GetAllTasksByUserIdAsync(context.Message.UserId);

                                        if (findTasks != null)
                                        {
                                            this.logger.LogInformation($"[GetAllTaskByUserId] Successfuly get task id {context.Message.UserId}");

                                            var response = new GetAllTaskByUserIdResponse
                                            {
                                                tasks = findTasks
                                            };

                                            await context.RespondAsync(ResponseWrapper<GetAllTaskByUserIdResponse>.Success("Successfuly get task", response));

                                        }
                                        else
                                        {
                                            this.logger.LogInformation($"[GetAllTaskByUserId] Failed to get task id {context.Message.UserId}");
                                            await context.RespondAsync(ResponseWrapper<GetAllTaskByUserIdResponse>.Fail("Failed to get task Invalid Task Id"));

                                        }
                                    }
                                    else
                                    {
                                        this.logger.LogInformation($"[GetTaskById] You are not authorized to get this tasks");
                                        await context.RespondAsync(ResponseWrapper<GetAllTaskByUserIdResponse>.Fail("You are not authorized to get tasks"));
                                    }
                                    
                                }
                                else
                                {
                                    this.logger.LogInformation($"[GetAllTaskByUserId] Invalid Task Id {context.Message.UserId}");
                                    await context.RespondAsync(ResponseWrapper<GetAllTaskByUserIdResponse>.Fail("Invalid Task Id"));
                                }
                            }
                            else
                            {
                                this.logger.LogInformation($"[GetTaskById] Invalid Token.");
                                await context.RespondAsync(ResponseWrapper<GetAllTaskByUserIdResponse>.Fail("Invalid Token."));
                            }
                        }
                        else
                        {
                            this.logger.LogInformation($"[GetTaskById] Invalid Token.");
                            await context.RespondAsync(ResponseWrapper<GetAllTaskByUserIdResponse>.Fail("Invalid Token."));
                        }
                    }
                    else
                    {
                        this.logger.LogInformation($"[GetTaskById] Invalid Token.");
                        await context.RespondAsync(ResponseWrapper<GetAllTaskByUserIdResponse>.Fail("Invalid Token."));
                    }
                }
                else
                {
                    this.logger.LogInformation($"[GetTaskById] Token is required.");
                    await context.RespondAsync(ResponseWrapper<GetAllTaskByUserIdResponse>.Fail("Token is required."));
                }

            }
            catch (Exception ex)
            {
                this.logger.LogDebug(ex, $"[GetAllTaskByUserId] id {context.Message.UserId} - exception occored. stacktrace: {ex.StackTrace}");
                await context.RespondAsync(ResponseWrapper<GetAllTaskByUserIdResponse>.Fail(ex.Message));
            }
        }
    }
}
