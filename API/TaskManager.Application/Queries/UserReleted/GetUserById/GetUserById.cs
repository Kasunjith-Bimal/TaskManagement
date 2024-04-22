using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.Wrappers;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Interfaces;


namespace TaskManager.Application.Queries.UserReleted.GetUserById
{
    public class GetUserById : IConsumer<GetUserByIdQuery>
    {
        private readonly ILogger<GetUserById> logger;
        private readonly IUserService userService;
        private readonly IConfiguration configuration;
        public GetUserById(ILogger<GetUserById> logger, IUserService userService, IConfiguration configuration)
        {
            this.logger = logger;
            this.userService = userService;
            this.configuration = configuration;
        }

        public async Task Consume(ConsumeContext<GetUserByIdQuery> context)
        {
            try
            {
                this.logger.LogInformation($"[GetUserById] Received event");
                if (!string.IsNullOrEmpty(context.Message.Id))
                {
                    this.logger.LogInformation($"[GetUserById] EmployeeService GetUserByIdAsync method call");
                    var findEmployee = await this.userService.GetUserByIdAsync(context.Message.Id);

                    if (findEmployee != null)
                    {
                        var roleType = await this.userService.GetUserRolesAsync(findEmployee);
                        this.logger.LogInformation($"[GetUserById] Successfuly get employee id {context.Message.Id}");

                        var response = new GetUserByIdResponse
                        {
                            user = new GetUserByIdDetail
                            {
                                Id = findEmployee.Id,
                                Email = findEmployee.Email,
                                FullName = findEmployee.FullName,
                                RoleType = roleType[0].ToLower() == "admin"? RoleType.ADMIN :RoleType.User,
                                IsActive = findEmployee.IsActive
                            }
                        };

                        await context.RespondAsync(ResponseWrapper<GetUserByIdResponse>.Success("Successfuly get employee", response));

                    }
                    else
                    {
                        this.logger.LogInformation($"[GetUserById] Failed to get employee id {context.Message.Id}");
                        await context.RespondAsync(ResponseWrapper<GetUserByIdResponse>.Fail("Failed to get employee Invalid employee Id"));

                    }
                }
                else
                {
                    this.logger.LogInformation($"[GetUserById] Invalid employee Id {context.Message.Id}");
                    await context.RespondAsync(ResponseWrapper<GetUserByIdResponse>.Fail("Invalid employee Id"));
                }
                
            }
            catch (Exception ex)
            {
                this.logger.LogDebug(ex, $"[GetUserById] id {context.Message.Id} - exception occored. stacktrace: {ex.StackTrace}");
                await context.RespondAsync(ResponseWrapper<GetUserByIdResponse>.Fail(ex.Message));
            }
        }
    }
}
