using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Domain.Interfaces.ITaskRepository;

namespace TaskManager.Domain.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskReadRepository _readRepository;
        private readonly ITaskWriteRepository _writeRepository;
        private readonly ILogger<TaskService> logger;

        public TaskService(ITaskReadRepository readRepository, ITaskWriteRepository writeRepository, ILogger<TaskService> logger)
        {
            this._readRepository = readRepository;
            this._writeRepository = writeRepository;
            this.logger = logger;
        }

        public Task<TaskDetail> GetTaskByIdAsync(long taskId)
        {
            return _readRepository.GetTaskByIdAsync(taskId);
        }

        public Task<List<TaskDetail>> GetAllTasksAsync()
        {
            try
            {
                this.logger.LogInformation($"[TaskService:GetAllTasksAsync] recieved event");
                return _readRepository.GetAllTasksAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogDebug($"[TaskService:GetAllTasksAsync] exception occurred: {ex.Message} - Stacktrace: {ex.StackTrace}");
                return null;

            }
        }

        public TaskDetail AddTask(TaskDetail task)
        {
            try
            {
                this.logger.LogInformation($"[TaskService:AddTask] recieved event task title : {task.Title}");
                return _writeRepository.AddTask(task);
            }
            catch (Exception ex)
            {
                this.logger.LogDebug($"[TaskService:AddTask] exception occurred: {ex.Message} - Stacktrace: {ex.StackTrace}");
                return null;

            }

        }

        public TaskDetail UpdateTask(TaskDetail task)
        {
            try
            {
                this.logger.LogInformation($"[TaskService:UpdateTask] recieved event task id: {task.Id} title : {task.Title}");
                return _writeRepository.UpdateTask(task);
            }
            catch (Exception ex)
            {

                this.logger.LogDebug($"[TaskService:UpdateTask] task id: {task.Id} exception occurred: {ex.Message} - Stacktrace: {ex.StackTrace}");
                return null;
            }

        }

        public Task<bool> DeleteTask(long taskId)
        {
            try
            {
                this.logger.LogInformation($"[TaskService:DeleteTask] recieved event task id: {taskId}");
                return _writeRepository.DeleteTask(taskId);
            }
            catch (Exception ex)
            {
                this.logger.LogDebug($"[TaskService:DeleteTask]  task id: {taskId} exception occurred: {ex.Message} - Stacktrace: {ex.StackTrace}");
                return Task.FromResult(false);
            }

        }

        public async Task<List<TaskDetail>> GetAllTasksByUserIdAsync(string userId)
        {
            try
            {
                this.logger.LogInformation($"[TaskService:GetAllTasksByUserIdAsync] recieved event userId id: {userId}");
                return await _readRepository.GetAllTasksByUserIdAsync(userId);
            }
            catch (Exception ex)
            {
                this.logger.LogDebug($"[TaskService:GetAllTasksByUserIdAsync]  userId id: {userId} exception occurred: {ex.Message} - Stacktrace: {ex.StackTrace}");
                return null;
            }
        }
    }
}
