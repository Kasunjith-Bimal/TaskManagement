using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces.ITaskRepository
{
    public interface ITaskWriteRepository
    {
        TaskDetail AddTask(TaskDetail task);
        TaskDetail UpdateTask(TaskDetail task);
        Task<bool> DeleteTask(long taskId);
    }
}
