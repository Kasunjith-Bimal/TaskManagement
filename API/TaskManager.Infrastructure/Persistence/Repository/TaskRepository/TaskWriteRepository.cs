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
    public class TaskWriteRepository: ITaskWriteRepository
    {
        private readonly TaskManagerDbContext _dbContext;
        private readonly ILogger<TaskWriteRepository> logger;

        public TaskWriteRepository(TaskManagerDbContext dbContext, ILogger<TaskWriteRepository> logger)
        {
            this._dbContext = dbContext;
            this.logger = logger;
        }

        public TaskDetail AddTask(TaskDetail task)
        {
            try
            {
                this.logger.LogInformation($"[TaskWriteRepository:AddTask] recieved event task title : {task.Title}");
                this._dbContext.TaskDetails.Add(task);
                this._dbContext.SaveChanges();
                this.logger.LogInformation($"[TaskWriteRepository:AddTask] success event task id:{task.Id} title : {task.Title}");
                return task;
            }
            catch (Exception ex)
            {
                this.logger.LogDebug($"[TaskWriteRepository:AddTask] exception occurred: {ex.Message} - Stacktrace: {ex.StackTrace}");
                return null;
            }

        }

        public TaskDetail UpdateTask(TaskDetail task)
        {
            try
            {
                this.logger.LogInformation($"[TaskWriteRepository:UpdateTask] recieved event task id: {task.Id} title : {task.Title}");
                //this._dbContext.TaskDetails.Update(task);
                this._dbContext.Entry(task).State = EntityState.Modified;
                this._dbContext.SaveChanges();
                this.logger.LogInformation($"[TaskWriteRepository:UpdateTask] success event task id:{task.Id} title : {task.Title}");
                return task;
            }
            catch (Exception ex)
            {
                this.logger.LogDebug($"[TaskWriteRepository:UpdateTask] exception occurred: {ex.Message} - Stacktrace: {ex.StackTrace}");
                return null;
            }

        }

        public async Task<bool> DeleteTask(long taskId)
        {
            try
            {
                this.logger.LogInformation($"[TaskWriteRepository:DeleteTask] recieved event task id: {taskId}");
                var task = await _dbContext.TaskDetails.FindAsync(taskId);
                if (task == null)
                    return false;

                _dbContext.TaskDetails.Remove(task);
                _dbContext.SaveChanges();
                this.logger.LogInformation($"[TaskWriteRepository:DeleteTask] success event task id:{taskId}");
                return true;
            }
            catch (Exception ex)
            {

                this.logger.LogDebug($"[TaskWriteRepository:DeleteTask] exception occurred: {ex.Message} - Stacktrace: {ex.StackTrace}");
                return await Task.FromResult(false);
            }

        }
    }
}
