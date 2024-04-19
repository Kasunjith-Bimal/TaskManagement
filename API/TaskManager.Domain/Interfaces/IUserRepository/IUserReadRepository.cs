using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces.IUserRepository
{
    public interface IUserReadRepository
    {
        Task<UserDetail> GetUserByIdAsync(string usereId);

        Task<UserDetail> GetUserByEmailAsync(string usereEmail);

        Task<List<UserDetail>> GetAllUsersAsync();

        Task<List<UserDetail>> GetAllAdminsAsync();

        Task<IList<string>> GetUserRolesAsync(UserDetail usere);


        Task<bool> CheckPasswordAsync(UserDetail usere ,string passWord);
    }
}
