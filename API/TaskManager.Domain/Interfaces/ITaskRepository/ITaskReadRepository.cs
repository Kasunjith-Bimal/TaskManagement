using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces.ITaskRepository
{
    public interface ITaskReadRepository
    {
        Task<TaskDetail> GetTaskByIdAsync(long taskId);
        Task<List<TaskDetail>> GetAllTasksAsync();
        Task<List<TaskDetail>> GetAllTasksByUserIdAsync(string userId);
    }
}
