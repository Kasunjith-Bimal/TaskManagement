using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.Wrappers;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;


namespace TaskManager.Application.Command.AuthenticationReleted.Register
{
    public class RegisterUser : IConsumer<RegisterUserCommand>
    {
        private readonly ILogger<RegisterUser> logger;
        private readonly IConfiguration configuration;
        private readonly IUserService userService;
        public RegisterUser(ILogger<RegisterUser> logger, IConfiguration configuration, IUserService userService)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.userService = userService;
        }

        public async Task Consume(ConsumeContext<RegisterUserCommand> context)
        {
            try
            {
                this.logger.LogInformation($"[RegisterUser] Received event");
                this.logger.LogInformation($"[RegisterUser] Check Email is Empty");
                if (!String.IsNullOrEmpty(context.Message.Email))
                {
                    this.logger.LogInformation($"[RegisterUser] Check Email is exsist");
                    var checkExistingEmail = await this.userService.GetUserByEmailAsync(context.Message.Email);

                    if(checkExistingEmail == null)
                    {
                        this.logger.LogInformation($"[RegisterUser] Check Email is not exsist");
                        var userDetail = new UserDetail
                        {
                           
                            Email = context.Message.Email,
                            UserName = context.Message.Email,
                            FullName = context.Message.FullName,
                            IsFirstLogin = true,
                            IsActive = true,
                        };

                        var user = await this.userService.AddUser(userDetail, context.Message.RoleType);

                        if (user != null)
                        {
                            var response = new RegisterUserResponse
                            {
                                user = user
                            };

                            this.logger.LogInformation($"[RegisterUser] Success fully add employee");
                            await context.RespondAsync(ResponseWrapper<RegisterUserResponse>.Success("Success fully add employee", response));
                        }
                        else
                        {
                            this.logger.LogInformation($"[RegisterUser] Fail to add employee");
                            await context.RespondAsync(ResponseWrapper<RegisterUserResponse>.Fail("Fail to add employee"));
                        }
                    }
                    else
                    {
                        this.logger.LogInformation($"[RegisterUser] Fail to add employee email alredy existe");
                        await context.RespondAsync(ResponseWrapper<RegisterUserResponse>.Fail("Fail to add employee email alredy existe"));
                    }
                }
                else
                {
                    this.logger.LogInformation($"[RegisterUser] Email cannot be empty");
                    await context.RespondAsync(ResponseWrapper<RegisterUserResponse>.Fail("Email cannot be empty"));
                }
            }
            catch (Exception ex)
            {
                this.logger.LogDebug(ex, $"[RegisterUser] - exception occored. stacktrace: {ex.StackTrace}");
                await context.RespondAsync(ResponseWrapper<RegisterUserResponse>.Fail(ex.Message));
            }
        }
    }
}
