
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Enums;


namespace TaskManager.Application.Command.AuthenticationReleted.Register
{
    public class RegisterUserCommand
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public RoleType RoleType { get; set; }

     

    }
}
