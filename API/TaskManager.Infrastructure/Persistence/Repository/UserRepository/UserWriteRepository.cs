
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Interfaces.IUserRepository;

namespace Employee.Infrastructure.Persistence.Repository.UserRepository
{
    public class UserWriteRepository : IUserWriteRepository
    {
      
        private readonly ILogger<UserWriteRepository> logger;
        private readonly UserManager<UserDetail> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserWriteRepository(ILogger<UserWriteRepository> logger, UserManager<UserDetail> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.logger = logger;
            this._userManager = userManager;
            this._roleManager = roleManager;
        }

        public async Task<UserDetail> AddUser(UserDetail user,string tempoyryPassword,RoleType roleType)
        {
            try
            {
                this.logger.LogInformation($"[UserWriteRepository:AddUser] recieved event");
                var result = await _userManager.CreateAsync(user, tempoyryPassword);
                if (result.Succeeded)
                {
                    this.logger.LogInformation($"[UserWriteRepository:AddUser]  success event");
                    await _userManager.AddToRoleAsync(user, Enum.GetName(typeof(RoleType), roleType));
                    return user;
                }
                else
                {
                    this.logger.LogInformation($"[UserWriteRepository:AddUser] not success event");
                    return null;
                }
            }
            catch (Exception ex)
            {
                this.logger.LogDebug($"[UserWriteRepository:AddUser]  exception occurred: {ex.Message} - Stacktrace: {ex.StackTrace}");
                return null;
            }
            
                
        }

        public Task<UserDetail> AddRoleToUser(UserDetail user, RoleType roleType)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ChangePasswordAsync(UserDetail user, string oldPassword, string NewPassword)
        {
            try
            {
                this.logger.LogInformation($"[UserWriteRepository:ChangePasswordAsync] recieved event");
                // remove strong passsword validater
                _userManager.PasswordValidators.Clear();
                var changePasswordResult = await _userManager.ChangePasswordAsync(user, oldPassword, NewPassword);
                if (changePasswordResult.Succeeded)
                {
                    this.logger.LogInformation($"[UserWriteRepository:ChangePasswordAsync] success event");
                    return await Task.FromResult(true);
                }
                else
                {
                    this.logger.LogInformation($"[UserWriteRepository:ChangePasswordAsync] not success event");
                    return await Task.FromResult(false);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogDebug($"[UserWriteRepository:ChangePasswordAsync]  exception occurred: {ex.Message} - Stacktrace: {ex.StackTrace}");
                return await Task.FromResult(false);
            }
        }

        public async Task<bool> DeleteUser(UserDetail user)
        {
            try
            {
                this.logger.LogInformation($"[UserWriteRepository:DeleteUser] recieved event");
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    this.logger.LogInformation($"[UserWriteRepository:DeleteUser] success event");
                    return true;
                }
                else
                {
                    this.logger.LogInformation($"[UserWriteRepository:DeleteUser] not success event");
                    return false;
                }
            }
            catch (Exception ex)
            {

                this.logger.LogDebug($"[UserWriteRepository:DeleteUser]  exception occurred: {ex.Message} - Stacktrace: {ex.StackTrace}");
                return false;
            }
          
        }

        public async Task<UserDetail> RemoveAndAddRolesAsync(UserDetail user, IList<string> oldRoleType, RoleType newRoleType)
        {
            try
            {
                this.logger.LogInformation($"[UserWriteRepository:RemoveAndAddRolesAsync] recieved event");
                var result =  await _userManager.RemoveFromRolesAsync(user, oldRoleType);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Enum.GetName(typeof(RoleType), newRoleType));
                    this.logger.LogInformation($"[UserWriteRepository:RemoveAndAddRolesAsync] success event");
                    return user;
                }
                else
                {
                    this.logger.LogInformation($"[UserWriteRepository:RemoveAndAddRolesAsync] not success event");
                    return null;
                }
            }
            catch (Exception ex)
            {
                this.logger.LogDebug($"[UserWriteRepository:RemoveAndAddRolesAsync]  exception occurred: {ex.Message} - Stacktrace: {ex.StackTrace}");
                return null;
            }
            
        }

        public async Task<UserDetail> UpdateUser(UserDetail user)
        {
            try
            {
                this.logger.LogInformation($"[UserWriteRepository:UpdateUser] recieved event");
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    this.logger.LogInformation($"[UserWriteRepository:UpdateUser] success event");
                    return user;
                }
                else
                {
                    this.logger.LogInformation($"[UserWriteRepository:UpdateUser] not success event");
                    return null;
                }
            }
            catch (Exception ex)
            {
                this.logger.LogDebug($"[UserWriteRepository:UpdateUser]  exception occurred: {ex.Message} - Stacktrace: {ex.StackTrace}");
                return null;
            }
           
        }
    }
}
