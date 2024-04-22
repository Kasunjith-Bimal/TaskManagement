using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.Wrappers;
using TaskManager.Domain.Interfaces;


namespace TaskManager.Application.Command.AuthenticationReleted.Login
{
    public class LoginUser : IConsumer<LoginUserCommand>
    {
        private readonly ILogger<LoginUser> logger;
        private readonly IConfiguration configuration;
        private readonly IUserService userService;
        private readonly IJwtTokenManager JwtTokenManager;
        public LoginUser(ILogger<LoginUser> logger, IConfiguration configuration, IUserService userService, IJwtTokenManager JwtTokenManager)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.userService = userService;
            this.JwtTokenManager = JwtTokenManager;
        }

        public async Task Consume(ConsumeContext<LoginUserCommand> context)
        {
            try
            {
                this.logger.LogInformation($"[LoginUser] Received event");
                if ((!String.IsNullOrEmpty(context.Message.Password)))
                {
                    this.logger.LogInformation($"[LoginUser] Check Email is Empty");
                    if (!String.IsNullOrEmpty(context.Message.Email))
                    {
                        this.logger.LogInformation($"[LoginUser] Check Email is exsist");
                        var checkExistingEmployee = await this.userService.GetUserByEmailAsync(context.Message.Email);
                       

                        if (checkExistingEmployee != null)
                        {

                            if (checkExistingEmployee.IsActive)
                            {
                                var chekPasswordCurrect = await this.userService.CheckPasswordAsync(checkExistingEmployee, context.Message.Password);

                                if (chekPasswordCurrect)
                                {
                                    // generatee Token 
                                    var generateAccesstokenDetails = await this.JwtTokenManager.GenerateToken(checkExistingEmployee);

                                    if (generateAccesstokenDetails != null)
                                    {
                                        var response = new LoginUserResponse
                                        {
                                            TokenDetail = new LoginUserTokenResponse
                                            {
                                                AccessToken = generateAccesstokenDetails.Item1,
                                                Expire = generateAccesstokenDetails.Item2,
                                            },
                                            Email = checkExistingEmployee.Email,
                                            FullName = checkExistingEmployee.FullName,
                                            IsFirstLogin = checkExistingEmployee.IsFirstLogin,
                                            Id = checkExistingEmployee.Id
                                        };

                                        this.logger.LogInformation($"[LoginUser] success fully login");
                                        await context.RespondAsync(ResponseWrapper<LoginUserResponse>.Success("success fully login", response));
                                    }
                                    else
                                    {
                                        this.logger.LogInformation($"[LoginUser] Fail to generate Token");
                                        await context.RespondAsync(ResponseWrapper<LoginUserResponse>.Fail("Fail to generate Token"));
                                    }
                                }
                                else
                                {
                                    this.logger.LogInformation($"[LoginUser] password mismatch");
                                    await context.RespondAsync(ResponseWrapper<LoginUserResponse>.Fail("password not found"));
                                }

                            }
                            else
                            {
                                this.logger.LogInformation($"[LoginUser] Inactive user");
                                await context.RespondAsync(ResponseWrapper<LoginUserResponse>.Fail("Inactive user"));

                            }
                            
                        }
                        else
                        {
                            this.logger.LogInformation($"[LoginUser] Email address mismatch");
                            await context.RespondAsync(ResponseWrapper<LoginUserResponse>.Fail("Email address not found"));
                        }
                    }
                    else
                    {
                        this.logger.LogInformation($"[LoginUser] Email cannot be empty");
                        await context.RespondAsync(ResponseWrapper<LoginUserResponse>.Fail("Email cannot be empty"));
                    }
                }
                else
                {
                    this.logger.LogInformation($"[LoginUser] password cannot be empty");
                    await context.RespondAsync(ResponseWrapper<LoginUserResponse>.Fail("password cannot be empty"));
                }

            }
            catch (Exception ex)
            {
                this.logger.LogDebug(ex, $"[LoginUser] - exception occored. stacktrace: {ex.StackTrace}");
                await context.RespondAsync(ResponseWrapper<LoginUserResponse>.Fail(ex.Message));
            }
        }
    }
}
