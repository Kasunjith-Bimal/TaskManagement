using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Command.TaskReleted.DeleteTask
{
    public class DeleteTaskCommand
    {
        public long Id { get; set; }

        public string Token { get; set; }

    }
}
