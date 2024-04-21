using MassTransit.Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Dtos;
using TaskManager.API.Filters.Authorization;
using TaskManager.Application.Command.TaskReleted.CreateTask;
using TaskManager.Application.Command.TaskReleted.DeleteTask;
using TaskManager.Application.Command.TaskReleted.UpdateTask;
using TaskManager.Application.Queries.TaskReleted.GetTaskById;
using TaskManager.Application.Wrappers;
using TaskManager.Domain.Entities;

namespace TaskManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [CustomAuthorize("User")]
    public class TaskController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ILogger<TaskController> logger;


        public TaskController(IMediator mediator, ILogger<TaskController> logger)
        {
            this.mediator = mediator;
            this.logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskDetail>> GetTaskById(long id)
        {
          
            try
            {
                var client = this.mediator.CreateRequestClient<GetTaskByIdQuery>();
                var response = await client.GetResponse<ResponseWrapper<GetTaskByIdResponse>>(new GetTaskByIdQuery
                {
                    Id = id,
                    Token = Request.Headers["Authorization"].ToString(),
                });

                if (response.Message.Succeeded)
                {
                    this.logger.LogInformation($"Event succeeded in TaskController:GetTaskById");
                    return Ok(response.Message);
                }
                else
                {
                    this.logger.LogInformation($"Event not succeeded in TaskController:GetTaskById. Message: {response.Message.Message}");
                    return NotFound(response.Message);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogInformation($"Exception occurred in TaskController:GetTaskById . Message: {ex.Message} - Exception: {ex.InnerException?.ToString()} - StackTrace: {ex.StackTrace}");
                return BadRequest(ex);
            }

        }

        [HttpPost]
        public async Task<IActionResult> AddTask(TaskDetailDto task)
        {
            try
            {
                var client = this.mediator.CreateRequestClient<CreateTaskCommand>();
                var response = await client.GetResponse<ResponseWrapper<CreateTaskResponse>>(new CreateTaskCommand
                {
                    TaskDetail = new CreateTaskDetail
                    {
                        Description = task.Description,
                        DueDate = task.DueDate,
                        IsComplete = task.IsComplete,
                        Title = task.Title
                        
                    },
                    Token = Request.Headers["Authorization"].ToString(),
                });

                if (response.Message.Succeeded)
                {
                    this.logger.LogInformation($"Event succeeded in TaskController:AddTask");
                    //return Ok(response.Message);
                    return CreatedAtAction(nameof(GetTaskById), new { id = response.Message.Payload.task.Id }, response.Message);
                }
                else
                {
                    this.logger.LogInformation($"Event not succeeded in TaskController:AddTask. Message: {response.Message.Message}");
                    return BadRequest(response.Message);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogInformation($"Exception occurred in TaskController:AddTask. Message: {ex.Message} - Exception: {ex.InnerException?.ToString()} - StackTrace: {ex.StackTrace}");
                return BadRequest(ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(long id, TaskDetailDto task)
        {
            try
            {
                //if (id != task.Id)
                //    return BadRequest();

                var client = this.mediator.CreateRequestClient<UpdateTaskCommand>();
                var response = await client.GetResponse<ResponseWrapper<UpdateTaskResponse>>(new UpdateTaskCommand
                {
                    TaskDetail = new UpdateTaskDetail
                    {
                        Id = task.Id,
                        Description = task.Description,
                        DueDate = task.DueDate,
                        Title = task.Title,
                        IsComplete = task.IsComplete,
                    },
                    Id = id,
                    Token = Request.Headers["Authorization"].ToString(),
                });

                if (response.Message.Succeeded)
                {
                    this.logger.LogInformation($"Event succeeded in TaskController:UpdateTask");
                    return Ok(response.Message);
                }
                else
                {
                    this.logger.LogInformation($"Event not succeeded in TaskController:UpdateTask. Message: {response.Message.Message}");
                    return BadRequest(response.Message);
                }
            }
            catch (Exception ex)
            {

                this.logger.LogInformation($"Exception occurred in TaskController:UpdateTask. Message: {ex.Message} - Exception: {ex.InnerException?.ToString()} - StackTrace: {ex.StackTrace}");
                return BadRequest(ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTask(long id)
        {
            try
            {
                var client = this.mediator.CreateRequestClient<DeleteTaskCommand>();
                var response = await client.GetResponse<ResponseWrapper<DeleteTaskResponse>>(new DeleteTaskCommand
                {
                    Id = id,
                    Token = Request.Headers["Authorization"].ToString(),
                });

                if (response.Message.Succeeded)
                {
                    this.logger.LogInformation($"Event succeeded in TaskController:DeleteTask");
                    return Ok(response.Message);
                }
                else
                {
                    this.logger.LogInformation($"Event not succeeded in TaskController:DeleteTask. Message: {response.Message.Message}");
                    return NotFound(response.Message);

                }
            }
            catch (Exception ex)
            {

                this.logger.LogInformation($"Exception occurred in TaskController:DeleteTask. Message: {ex.Message} - Exception: {ex.InnerException?.ToString()} - StackTrace: {ex.StackTrace}");
                return BadRequest(ex);
            }
        }
    }
}
