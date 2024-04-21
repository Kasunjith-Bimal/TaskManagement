using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Command.TaskReleted.UpdateTask
{
    public class UpdateTaskCommand
    {
        public UpdateTaskDetail TaskDetail { get; set; }

        public long Id { get; set; }

        public string Token { get; set; }

    }

    public class UpdateTaskDetail
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsComplete { get; set; }
    }
}
