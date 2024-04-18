
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Domain.Entities
{
    public class UserDetail : IdentityUser
    {
        public string FullName { get; set; }
        public DateTime JoinDate { get; set; }
        public bool IsFirstLogin { get; set; }
        public bool IsActive { get; set; }
       
    }
}
