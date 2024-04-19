
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;


namespace TaskManager.Application.Command.AuthenticationReleted.Register
{
    public class RegisterUserResponse
    {
        public UserDetail user { get; set; }

    }
}
