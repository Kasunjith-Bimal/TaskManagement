using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using MimeKit;
using MailKit.Net.Smtp;
using System.Reflection.Metadata;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Interfaces;
using TaskManager.Domain.Interfaces.IUserRepository;

namespace TaskManager.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IUserReadRepository _readRepository;
        private readonly IUserWriteRepository _writeRepository;
        private readonly ILogger<UserService> logger;
        private readonly IConfiguration _configuration;

        public UserService(IUserReadRepository readRepository, IUserWriteRepository writeRepository, ILogger<UserService> logger, IConfiguration configuration)
        {
            _readRepository = readRepository;
            _writeRepository = writeRepository;
            this.logger = logger;
            _configuration = configuration;
        }

        public async Task<UserDetail> GetUserByIdAsync(string userId)
        {
            try
            {
                this.logger.LogInformation($"[UserService:GetUserByIdAsync] UserId : {userId} recieved event");
                return await _readRepository.GetUserByIdAsync(userId);     
            }
            catch (Exception ex)
            {
                this.logger.LogDebug($"[UserService:GetUserByIdAsync] UserId {userId} exception occurred: {ex.Message} - Stacktrace: {ex.StackTrace}");
                return null;
            }
          
        }

        public async Task<List<UserDetail>> GetAllUsersAsync()
        {
            try
            {
                this.logger.LogInformation($"[UserService:GetAllUsersAsync] recieved event");
                return await _readRepository.GetAllUsersAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogDebug($"[UserService:GetAllUsersAsync] exception occurred: {ex.Message} - Stacktrace: {ex.StackTrace}");
                return null;

            }
        }

        public async Task<UserDetail> AddUser(UserDetail user,RoleType roleType)
        {
            try
            {
                this.logger.LogInformation($"[UserService:AddUser] recieved event User email : {user.Email}");
                var generateTemporyPassword = GenerateTemporaryPassword(10);
                this.logger.LogInformation($"[UserService:AddUser] recieved event User email : {user.Email} tempory password :{generateTemporyPassword}");
                var UserDetails =  await _writeRepository.AddUser(user, generateTemporyPassword, roleType);

                if (UserDetails != null)
                {
                    this.logger.LogInformation($"[UserService:AddUser] send email : {user.Email} to temporypassword : {generateTemporyPassword}");
                    if (!string.IsNullOrEmpty(generateTemporyPassword))
                    {
                        await this.SendEmailAsync(UserDetails.Email, "Your temporary password", $"Your temporary password is: {generateTemporyPassword}", user.FullName);
                    }
                    return UserDetails;
                }
                else
                {
                    this.logger.LogInformation($"[UserService:AddUser] fail to add email : {user.Email} to temporypassword : {generateTemporyPassword}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                this.logger.LogDebug($"[UserService:AddUser] exception occurred: {ex.Message} - Stacktrace: {ex.StackTrace}");
                return null;
                
            }
           
        }

        private string GenerateTemporaryPassword(int length = 10)
        {
            try
            {
                this.logger.LogInformation($"[UserService:GenerateTemporaryPassword] recieved event");
                const string lowerChars = "abcdefghijklmnopqrstuvwxyz";
                const string upperChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                const string numberChars = "0123456789";
                const string specialChars = "!@#$%^&*";

                Random random = new Random();

                // Ensure the password contains at least one character of each type
                var password = new char[length];
                password[0] = lowerChars[random.Next(lowerChars.Length)];
                password[1] = upperChars[random.Next(upperChars.Length)];
                password[2] = numberChars[random.Next(numberChars.Length)];
                password[3] = specialChars[random.Next(specialChars.Length)];

                // Fill the rest of the password with random characters from all types
                string allChars = lowerChars + upperChars + numberChars + specialChars;
                for (int i = 4; i < length; i++)
                {
                    password[i] = allChars[random.Next(allChars.Length)];
                }

                this.logger.LogInformation($"[UserService:GenerateTemporaryPassword] Success event");
                // Shuffle the password to randomize character positions
                return new string(password.OrderBy(x => random.Next()).ToArray());

            }
            catch (Exception ex)
            {

                return "";
            }
            
        }

        public async Task<UserDetail> UpdateUser(UserDetail user)
        {
            try
            {
                this.logger.LogInformation($"[UserService:UpdateUser] recieved event User id: {user.Id} email : {user.Email}");
                return await _writeRepository.UpdateUser(user);
            }
            catch (Exception ex)
            {

                this.logger.LogDebug($"[UserService:UpdateUser] User id: {user.Id} exception occurred: {ex.Message} - Stacktrace: {ex.StackTrace}");
                return null;
            }

        }

        public async Task<bool> DeleteUser(UserDetail user)
        {
            try
            {

                this.logger.LogInformation($"[UserService:DeleteUser] recieved event User id: {user.Id}");
                return await _writeRepository.DeleteUser(user);
            }
            catch (Exception ex)
            {
                this.logger.LogDebug($"[UserService:DeleteUser] User id: {user.Id} exception occurred: {ex.Message} - Stacktrace: {ex.StackTrace}");
                return await Task.FromResult(false);
            }
            
        }


        public async Task SendEmailAsync(string email, string subject, string message, string fullName)
        {
            try
            {
                this.logger.LogInformation($"[UserService:SendEmailAsync] send  email : {email} message :{message}");

                var emailMessage = new MimeMessage();

                emailMessage.From.Add(new MailboxAddress(_configuration["EmailSettings:SenderName"], _configuration["EmailSettings:Sender"]));
                emailMessage.To.Add(new MailboxAddress(fullName, email));
                emailMessage.Subject = subject;
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message };

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    // Gmail SMTP server address
                    await client.ConnectAsync(_configuration["EmailSettings:MailServer"], Convert.ToInt32(_configuration["EmailSettings:MailPort"]), false);
                    await client.AuthenticateAsync(_configuration["EmailSettings:Sender"], _configuration["EmailSettings:Password"]);
                    await client.SendAsync(emailMessage);

                    await client.DisconnectAsync(true);

                    this.logger.LogInformation($"[UserService:SendEmailAsync] recieved  email : {email} message :{message}");
                }

            }
            catch (Exception ex)
            {
                this.logger.LogDebug($"[UserService:DeleteUser] User email: {email} exception occurred: {ex.Message} - Stacktrace: {ex.StackTrace}");
            }
        }

        public async Task<UserDetail> GetUserByEmailAsync(string email)
        {
            try
            {
                this.logger.LogInformation($"[UserService:GetUserByEmailAsync] email : {email} recieved event");
                return await _readRepository.GetUserByEmailAsync(email);
            }
            catch (Exception ex)
            {
                this.logger.LogDebug($"[UserService:GetUserByEmailAsync] email {email} exception occurred: {ex.Message} - Stacktrace: {ex.StackTrace}");
                return null;
            }
        }

        public async Task<List<UserDetail>> GetAllAdminsAsync()
        {
            try
            {
                this.logger.LogInformation($"[UserService:GetAllAdminsAsync] recieved event");
                return await _readRepository.GetAllAdminsAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogDebug($"[UserService:GetAllAdminsAsync] exception occurred: {ex.Message} - Stacktrace: {ex.StackTrace}");
                return null;

            }
        }

        public async Task<bool> CheckPasswordAsync(UserDetail user, string passWord)
        {
            try
            {
                this.logger.LogInformation($"[UserService:CheckPasswordAsync] recieved event");
                return await _readRepository.CheckPasswordAsync(user, passWord);
            }
            catch (Exception ex)
            {
                this.logger.LogDebug($"[UserService:CheckPasswordAsync] exception occurred: {ex.Message} - Stacktrace: {ex.StackTrace}");
                return await Task.FromResult(false);

            }
        }

        public async Task<IList<string>> GetUserRolesAsync(UserDetail user)
        {
            try
            {
                return await _readRepository.GetUserRolesAsync(user);
            }
            catch (Exception ex)
            {

                this.logger.LogDebug($"[UserService:CheckPasswordAsync] exception occurred: {ex.Message} - Stacktrace: {ex.StackTrace}");
                return null;

            }
        }

        public async Task<bool> ChangePasswordAsync(UserDetail user, string oldPassword, string NewPassword)
        {
            try
            {
                return await _writeRepository.ChangePasswordAsync(user, oldPassword,NewPassword);
            }
            catch (Exception ex)
            {

                this.logger.LogDebug($"[UserService:CheckPasswordAsync] exception occurred: {ex.Message} - Stacktrace: {ex.StackTrace}");
                return await Task.FromResult(false);

            }
        }

        public async Task<UserDetail> RemoveAndAddRolesAsync(UserDetail user, IList<string> oldRoleType, RoleType newRoleType)
        {
            try
            {
                return await _writeRepository.RemoveAndAddRolesAsync(user, oldRoleType, newRoleType);
            }
            catch (Exception ex)
            {

                this.logger.LogDebug($"[UserService:RemoveAndAddRolesAsync] exception occurred: {ex.Message} - Stacktrace: {ex.StackTrace}");
                return null;

            }
        }
    }
}
