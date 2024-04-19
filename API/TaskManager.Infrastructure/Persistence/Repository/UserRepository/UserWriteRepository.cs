
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
            var result = await _userManager.CreateAsync(user, tempoyryPassword);
            if (result.Succeeded)
            {
                  await _userManager.AddToRoleAsync(user, Enum.GetName(typeof(RoleType), roleType));
                  return user;
            }
            else
            {
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
                // remove strong passsword validater
                _userManager.PasswordValidators.Clear();
                var changePasswordResult = await _userManager.ChangePasswordAsync(user, oldPassword, NewPassword);
                if (changePasswordResult.Succeeded)
                {
                    return await Task.FromResult(true);
                }
                else
                {
                    return await Task.FromResult(false);
                }
            }
            catch (Exception ex)
            {

                return await Task.FromResult(false);
            }
        }

        public async Task<bool> DeleteUser(UserDetail user)
        {
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<UserDetail> RemoveAndAddRolesAsync(UserDetail user, IList<string> oldRoleType, RoleType newRoleType)
        {
            try
            {
               
                var result =  await _userManager.RemoveFromRolesAsync(user, oldRoleType);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Enum.GetName(typeof(RoleType), newRoleType));
                    return user;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
            
        }

        public async Task<UserDetail> UpdateUser(UserDetail user)
        {
            try
            {
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
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

                return null;
            }
           
        }
    }
}
