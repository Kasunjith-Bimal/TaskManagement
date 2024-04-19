
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
                this.logger.LogInformation($"[UserReadRepository:GetAllUsersAsync] recieved event");
                var userList = await this._userManager.GetUsersInRoleAsync("USER");

                if(userList != null)
                {
                    return (List<UserDetail>)userList;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                this.logger.LogDebug($"[UserReadRepository:GetAllUsersAsync]  exception occurred: {ex.Message} - Stacktrace: {ex.StackTrace}");
                return null;
            }
        }

        public async Task<UserDetail> GetUserByIdAsync(string userId)
        {
            try
            {
                this.logger.LogInformation($"[UserReadRepository:GetUserByIdAsync] recieved event userId :{userId}");
                var user = await this._userManager.FindByIdAsync(userId);

                if (user != null)
                {
                    return user;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                this.logger.LogDebug($"[UserReadRepository:GetUserByIdAsync]  userId :{userId}  exception occurred: {ex.Message} - Stacktrace: {ex.StackTrace}");
                return null;
            }
        }

        public async Task<UserDetail> GetUserByEmailAsync(string userEmail)
        {
            try
            {
                this.logger.LogInformation($"[UserReadRepository:GetUserByEmailAsync] recieved event userEmail :{userEmail}");
                var user = await this._userManager.FindByEmailAsync(userEmail);

                if (user != null)
                {
                    return user;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                this.logger.LogDebug($"[UserReadRepository:GetUserByEmailAsync]  userEmail :{userEmail}  exception occurred: {ex.Message} - Stacktrace: {ex.StackTrace}");
                return null;
            }
        }

        public async Task<List<UserDetail>> GetAllAdminsAsync()
        {
            try
            {
                this.logger.LogInformation($"[UserReadRepository:GetAllAdminsAsync] recieved event");
                var userList = await this._userManager.GetUsersInRoleAsync("ADMIN");

                if (userList != null)
                {
                    return (List<UserDetail>)userList;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                this.logger.LogDebug($"[UserReadRepository:GetAllAdminsAsync]  exception occurred: {ex.Message} - Stacktrace: {ex.StackTrace}");
                return null;
            }
        }

        public async Task<bool> CheckPasswordAsync(UserDetail user, string passWord)
        {
            try
            {
                this.logger.LogInformation($"[UserReadRepository:CheckPasswordAsync] recieved event");
                return await _userManager.CheckPasswordAsync(user, passWord);
            }
            catch (Exception ex)
            {
                this.logger.LogDebug($"[UserReadRepository:CheckPasswordAsync]  exception occurred: {ex.Message} - Stacktrace: {ex.StackTrace}");
                return await Task.FromResult(false);
            }
        }

        public async Task<IList<string>> GetUserRolesAsync(UserDetail user)
        {
            try
            {
                this.logger.LogInformation($"[UserReadRepository:GetUserRolesAsync] recieved event");
                return await _userManager.GetRolesAsync(user);
            }
            catch (Exception ex)
            {
                this.logger.LogDebug($"[UserReadRepository:GetUserRolesAsync]  exception occurred: {ex.Message} - Stacktrace: {ex.StackTrace}");
                return null;
            }
        }
    }
}
