using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TaskManager.Application.Command.AuthenticationReleted.Login
{
    public class LoginUserResponse
    {
        public string Id { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public bool IsFirstLogin { get; set; }
        public LoginUserTokenResponse? TokenDetail { get; set; }

     

    }


    public class LoginUserTokenResponse
    {
        public string? AccessToken { get; set; }
        public DateTime Expire { get; set; }
    }
}
