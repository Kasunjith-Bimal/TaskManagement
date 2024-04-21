using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Queries.TaskReleted.GetTaskById
{
    public class GetTaskByIdQuery
    {
        public long Id { get; set; }

        public string Token { get; set; }
    }
}
