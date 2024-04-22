
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Enums;


namespace TaskManager.Application.Queries.UserReleted.GetUserById
{
    public class GetUserByIdResponse
    {
        public GetUserByIdDetail user { get; set; }

    }


    public class GetUserByIdDetail
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public RoleType RoleType { get; set; }
        public bool IsActive { get; set; }
    }
}
