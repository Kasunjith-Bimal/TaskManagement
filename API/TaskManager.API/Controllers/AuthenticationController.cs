using MassTransit.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Dtos;
using TaskManager.API.Filters.Authorization;
using TaskManager.Application.Command.AuthenticationReleted.ChangePassword;
using TaskManager.Application.Command.AuthenticationReleted.Login;
using TaskManager.Application.Command.AuthenticationReleted.Register;
using TaskManager.Application.Wrappers;
using TaskManager.Domain.Enums;

namespace TaskManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ILogger<AuthenticationController> logger;

        public AuthenticationController(IMediator mediator, ILogger<AuthenticationController> logger)
        {
            this.mediator = mediator;
            this.logger = logger;
        }

        [HttpPost("Register")]
        //[CustomAuthorize("Admin")]
        public async Task<IActionResult> RegisterUser(RegisterDto register)
        {
            try
            {
                var client = this.mediator.CreateRequestClient<RegisterUserCommand>();
                var response = await client.GetResponse<ResponseWrapper<RegisterUserResponse>>(new RegisterUserCommand
                {
                   
                    Email = String.IsNullOrEmpty(register.Email) ? "" : register.Email,
                    RoleType = RoleType.User,
                    FullName = String.IsNullOrEmpty(register.FullName) ? "" : register.FullName,
                });

                if (response.Message.Succeeded)
                {
                    this.logger.LogInformation($"Event succeeded in AuthenticationController:RegisterUser");
                    return Ok(response.Message);
                   // return CreatedAtAction(nameof(GetTaskById), new { id = response.Message.Payload.task.Id }, response.Message);
                }
                else
                {
                    this.logger.LogInformation($"Event not succeeded in AuthenticationController:RegisterUser. Message: {response.Message.Message}");
                    return BadRequest(response.Message);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogInformation($"Exception occurred in AuthenticationController:RegisterUser. Message: {ex.Message} - Exception: {ex.InnerException?.ToString()} - StackTrace: {ex.StackTrace}");
                return BadRequest(ex);
            }
        }


        [HttpPost("Login")]
        public async Task<IActionResult> LoginUser(LoginDto login)
        {
            try
            {
                var client = this.mediator.CreateRequestClient<LoginUserCommand>();
                var response = await client.GetResponse<ResponseWrapper<LoginUserResponse>>(new LoginUserCommand
                {
                    Email = login.Email,
                    Password = login.Password
                });
                

                if (response.Message.Succeeded)
                {
                    this.logger.LogInformation($"Event succeeded in AuthenticationController:LoginUser");
                    return Ok(response.Message);
                    // return CreatedAtAction(nameof(GetTaskById), new { id = response.Message.Payload.task.Id }, response.Message);
                }
                else
                {
                    this.logger.LogInformation($"Event not succeeded in AuthenticationController:LoginUser. Message: {response.Message.Message}");
                    return BadRequest(response.Message);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogInformation($"Exception occurred in AuthenticationController:LoginUser. Message: {ex.Message} - Exception: {ex.InnerException?.ToString()} - StackTrace: {ex.StackTrace}");
                return BadRequest(ex);
            }
        }

        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            try
            {
                var client = this.mediator.CreateRequestClient<ChangePasswordCommand>();
                var response = await client.GetResponse<ResponseWrapper<ChangePasswordResponse>>(new ChangePasswordCommand
                {
                    Email = changePasswordDto.Email,
                    OldPassword = changePasswordDto.OldPassword,
                    NewPassword = changePasswordDto.NewPassword
                });


                if (response.Message.Succeeded)
                {
                    this.logger.LogInformation($"Event succeeded in AuthenticationController:ChangePassword");
                    return Ok(response.Message);
                    // return CreatedAtAction(nameof(GetTaskById), new { id = response.Message.Payload.task.Id }, response.Message);
                }
                else
                {
                    this.logger.LogInformation($"Event not succeeded in AuthenticationController:ChangePassword. Message: {response.Message.Message}");
                    return BadRequest(response.Message);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogInformation($"Exception occurred in AuthenticationController:ChangePassword. Message: {ex.Message} - Exception: {ex.InnerException?.ToString()} - StackTrace: {ex.StackTrace}");
                return BadRequest(ex);
            }
        }
    }
}
