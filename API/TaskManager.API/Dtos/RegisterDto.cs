

using TaskManager.Domain.Enums;

namespace TaskManager.API.Dtos
{
    public class RegisterDto
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        //public RoleType RoleType { get; set; }
    }
}
