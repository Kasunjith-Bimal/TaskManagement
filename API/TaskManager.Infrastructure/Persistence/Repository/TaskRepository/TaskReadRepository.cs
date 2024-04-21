using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces.ITaskRepository;
using TaskManager.Infrastructure.Persistence.EFCore;

namespace TaskManager.Infrastructure.Persistence.Repository.TaskRepository
{
    public class TaskReadRepository :ITaskReadRepository
    {
        private readonly TaskManagerDbContext _dbContext;
        private readonly ILogger<TaskReadRepository> logger;

        public TaskReadRepository(TaskManagerDbContext dbContext, ILogger<TaskReadRepository> logger)
        {
            this._dbContext = dbContext;
            this.logger = logger;
        }

        public async Task<TaskDetail> GetTaskByIdAsync(long taskId)
        {
            try
            {
                this.logger.LogInformation($"[TaskReadRepository:GetTaskByIdAsync] recieved event taskId : {taskId}");
                return await this._dbContext.TaskDetails.FindAsync(taskId);
            }
            catch (Exception ex)
            {
                this.logger.LogDebug($"[TaskReadRepository:GetTaskByIdAsync] exception occurred: {ex.Message} - Stacktrace: {ex.StackTrace}");
                return null;
            }

        }

        public async Task<List<TaskDetail>> GetAllTasksAsync()
        {
            try
            {
                this.logger.LogInformation($"[TaskReadRepository:GetAllTasksAsync] recieved event");
                return await this._dbContext.TaskDetails.ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogDebug($"[TaskReadRepository:GetAllTasksAsync] exception occurred: {ex.Message} - Stacktrace: {ex.StackTrace}");
                return null;
            }

        }

        public async Task<List<TaskDetail>> GetAllTasksByUserIdAsync(string userId)
        {
            try
            {
                this.logger.LogInformation($"[TaskReadRepository:GetAllTasksByUserIdAsync] recieved event");
                return await this._dbContext.TaskDetails.Where(x => x.CreateUserId == userId).ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogDebug($"[TaskReadRepository:GetAllTasksByUserIdAsync] exception occurred: {ex.Message} - Stacktrace: {ex.StackTrace}");
                return null;
            }
        }
    }
}
