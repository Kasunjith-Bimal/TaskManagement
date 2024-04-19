
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces.IUserRepository;


namespace Employee.Infrastructure.Persistence.Repository.UserRepository
{
    public class UserReadRepository : IUserReadRepository
    {
     
        private readonly ILogger<UserReadRepository> logger;
        private readonly UserManager<UserDetail> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserReadRepository( ILogger<UserReadRepository> logger, UserManager<UserDetail> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.logger = logger;
            this._userManager = userManager;
            this._roleManager = roleManager;
        }

        public async Task<List<UserDetail>> GetAllUsersAsync()
        {
            try
            {
                var employeeList = await this._userManager.GetUsersInRoleAsync("USER");

                if(employeeList != null)
                {
                    return (List<UserDetail>)employeeList;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                return null;
            }
        }

        public async Task<UserDetail> GetUserByIdAsync(string userId)
        {
            try
            {
                var employee = await this._userManager.FindByIdAsync(userId);

                if (employee != null)
                {
                    return employee;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                return null;
            }
        }

        public async Task<UserDetail> GetUserByEmailAsync(string userEmail)
        {
            try
            {
                var employee = await this._userManager.FindByEmailAsync(userEmail);

                if (employee != null)
                {
                    return employee;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                return null;
            }
        }

        public async Task<List<UserDetail>> GetAllAdminsAsync()
        {
            try
            {
                var employeeList = await this._userManager.GetUsersInRoleAsync("ADMIN");

                if (employeeList != null)
                {
                    return (List<UserDetail>)employeeList;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                return null;
            }
        }

        public async Task<bool> CheckPasswordAsync(UserDetail user, string passWord)
        {
            try
            {
                return await _userManager.CheckPasswordAsync(user, passWord);
            }
            catch (Exception)
            {

                return await Task.FromResult(false);
            }
        }

        public async Task<IList<string>> GetUserRolesAsync(UserDetail user)
        {
            try
            {
                return await _userManager.GetRolesAsync(user);
            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}
