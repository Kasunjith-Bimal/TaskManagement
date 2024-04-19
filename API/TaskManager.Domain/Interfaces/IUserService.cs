
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;

namespace TaskManager.Domain.Interfaces
{
    public interface IUserService
    {
        Task<UserDetail> GetUserByIdAsync(string UserId);

        Task<UserDetail> GetUserByEmailAsync(string email);
        Task<List<UserDetail>> GetAllUsersAsync();

        Task<IList<string>> GetUserRolesAsync(UserDetail user);
        Task<UserDetail> AddUser(UserDetail user, RoleType roleType);
        Task<UserDetail> UpdateUser(UserDetail user);
        Task<bool> DeleteUser(UserDetail user);

        Task SendEmailAsync(string email, string subject, string message, string fullName);

        Task<List<UserDetail>> GetAllAdminsAsync();

        Task<bool> CheckPasswordAsync(UserDetail user, string passWord);

        Task<bool> ChangePasswordAsync(UserDetail user, string oldPassword, string NewPassword);

        Task<UserDetail> RemoveAndAddRolesAsync(UserDetail user, IList<string> oldRoleType, RoleType newRoleType);




    }
}
