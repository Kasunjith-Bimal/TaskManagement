using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Command.TaskReleted.CreateTask
{
    public class CreateTaskResponse
    {
        public TaskDetail task { get; set; }

    }
}
