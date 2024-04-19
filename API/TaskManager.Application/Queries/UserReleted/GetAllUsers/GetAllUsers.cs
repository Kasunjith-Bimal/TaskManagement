
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


namespace TaskManager.Application.Queries.UserReleted.GetAllUsers
{
    public class GetAllUsers : IConsumer<GetAllUsersQuery>
    {
        private readonly ILogger<GetAllUsers> logger;
        private readonly IUserService userService;
        private readonly IConfiguration configuration;
        public GetAllUsers(ILogger<GetAllUsers> logger, IUserService userService, IConfiguration configuration)
        {
            this.logger = logger;
            this.userService = userService;
            this.configuration = configuration;
        }

        public async Task Consume(ConsumeContext<GetAllUsersQuery> context)
        {
            try
            {
                this.logger.LogInformation($"[GetAllUser] Received event");
               
                this.logger.LogInformation($"[GetAllUser] TaskService GetAllUsersAsync method call");
                var allusers =  await this.userService.GetAllUsersAsync();

                if (allusers != null)
                {
                    this.logger.LogInformation($"[GetAllUser] Successfuly get all tasks");

                    List<GetAllUserResponseDetail> allusersList = new List<GetAllUserResponseDetail>();

                    foreach (var user in allusers)
                    {

                        GetAllUserResponseDetail userData = new GetAllUserResponseDetail()
                        {
                            Email = user.Email,
                            FullName = user.FullName,
                            Id = user.Id,
                            RoleType = RoleType.User,
                            IsActive  = user.IsActive

                        };

                        allusersList.Add(userData);
                    }


                    var response = new GetAllUsersResponse
                    {
                        users = allusersList
                    };

                    await context.RespondAsync(ResponseWrapper<GetAllUsersResponse>.Success("Successfuly get all employees", response));

                }
                else
                {
                    var response = new GetAllUsersResponse
                    {
                        users = new List<GetAllUserResponseDetail>()
                    };

                    await context.RespondAsync(ResponseWrapper<GetAllUsersResponse>.Success("Successfuly get all employees", response));

                }
            }
            catch (Exception ex)
            {
                this.logger.LogDebug(ex, $"[GetAllUser] - exception occored. stacktrace: {ex.StackTrace}");
                await context.RespondAsync(ResponseWrapper<GetAllUsersResponse>.Fail(ex.Message));
            }
        }
    }
}
