using MassTransit.Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Filters.Authorization;
using TaskManager.Application.Queries.TaskReleted.GetAllTaskByUserId;
using TaskManager.Application.Queries.UserReleted.GetUserById;
using TaskManager.Application.Wrappers;

namespace TaskManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ILogger<AuthenticationController> logger;

        public UsersController(IMediator mediator, ILogger<AuthenticationController> logger)
        {
            this.mediator = mediator;
            this.logger = logger;
        }

        [HttpGet("{id}")]
        [CustomAuthorize("User")]
        public async Task<IActionResult> GetUserByid(string id)
        {
            try
            {
                var client = this.mediator.CreateRequestClient<GetUserByIdQuery>();
                var response = await client.GetResponse<ResponseWrapper<GetUserByIdResponse>>(new GetUserByIdQuery
                {
                    Id = string.IsNullOrEmpty(id) ? "" : id
                });

                if (response.Message.Succeeded)
                {
                    this.logger.LogInformation($"Event succeeded in UserController:GetUserByid");
                    return Ok(response.Message);
                    // return CreatedAtAction(nameof(GetTaskById), new { id = response.Message.Payload.task.Id }, response.Message);
                }
                else
                {
                    this.logger.LogInformation($"Event not succeeded in UserController:GetUserByid. Message: {response.Message.Message}");
                    return BadRequest(response.Message);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogInformation($"Exception occurred in UserController:GetUserByid. Message: {ex.Message} - Exception: {ex.InnerException?.ToString()} - StackTrace: {ex.StackTrace}");
                return BadRequest(ex);
            }
        }

        [HttpGet("{id}/tasks")]
        [CustomAuthorize("User")]
        public async Task<IActionResult> GetTasksByUserId(string id)
        {
            try
            {
                var client = this.mediator.CreateRequestClient<GetAllTaskByUserIdQuery>();
                var response = await client.GetResponse<ResponseWrapper<GetAllTaskByUserIdResponse>>(new GetAllTaskByUserIdQuery
                {
                    UserId = string.IsNullOrEmpty(id) ? "" : id,
                    Token = Request.Headers["Authorization"].ToString(),
                });

                if (response.Message.Succeeded)
                {
                    this.logger.LogInformation($"Event succeeded in UserController:GetTasksByUserId");
                    return Ok(response.Message);
                    // return CreatedAtAction(nameof(GetTaskById), new { id = response.Message.Payload.task.Id }, response.Message);
                }
                else
                {
                    this.logger.LogInformation($"Event not succeeded in UserController:GetTasksByUserId. Message: {response.Message.Message}");
                    return BadRequest(response.Message);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogInformation($"Exception occurred in UserController:GetTasksByUserId. Message: {ex.Message} - Exception: {ex.InnerException?.ToString()} - StackTrace: {ex.StackTrace}");
                return BadRequest(ex);
            }
        }
    }
}
