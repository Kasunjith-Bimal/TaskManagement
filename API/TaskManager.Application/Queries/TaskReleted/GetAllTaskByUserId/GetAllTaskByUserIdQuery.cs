using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Queries.TaskReleted.GetAllTaskByUserId
{
    public class GetAllTaskByUserIdQuery
    {
        public string UserId { get; set; }

        public string Token { get; set; }
    }
}
