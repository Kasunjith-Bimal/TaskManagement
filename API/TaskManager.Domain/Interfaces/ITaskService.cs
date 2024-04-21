using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces
{
    public interface ITaskService
    {
        Task<TaskDetail> GetTaskByIdAsync(long taskId);
        Task<List<TaskDetail>> GetAllTasksAsync();
        TaskDetail AddTask(TaskDetail task);
        TaskDetail UpdateTask(TaskDetail task);
        Task<bool> DeleteTask(long taskId);
        Task<List<TaskDetail>> GetAllTasksByUserIdAsync(string userId);
    }
}
