using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Command.TaskReleted.CreateTask
{
    public class CreateTaskCommand
    {
        public CreateTaskDetail TaskDetail { get; set; }

        public string Token { get; set; }

    }

    public class CreateTaskDetail
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsComplete { get; set; }
    }
}
