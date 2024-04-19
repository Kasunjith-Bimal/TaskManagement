
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;


namespace TaskManager.Domain.Interfaces.IUserRepository
{
    public interface IUserWriteRepository
    {
        Task<UserDetail> AddUser(UserDetail user, string tempoyryPassword, RoleType roleType);
        Task<UserDetail> UpdateUser(UserDetail user);
        Task<bool> DeleteUser(UserDetail user);

        Task<bool> ChangePasswordAsync(UserDetail user, string oldPassword, string NewPassword);

        Task<UserDetail> RemoveAndAddRolesAsync(UserDetail user, IList<string> oldRoleType, RoleType newRoleType);
    }
}
