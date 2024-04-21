using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;


namespace TaskManager.Domain.Services
{
    public class JwtTokenManager : IJwtTokenManager
    {
        private readonly IConfiguration configuration;
        private readonly IUserService userService;
        public JwtTokenManager(IConfiguration configuration, IUserService userService)
        {
            this.configuration = configuration;
            this.userService = userService;
        }

        public async Task<ClaimsPrincipal> DecodeJwtAccessToken(string token)
        {
            try
            {
                //decrypt token
                var tokenHandler = new JwtSecurityTokenHandler();

                // Prepare token validation parameters
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = configuration["JWT:ValidAudience"],
                    ValidIssuer = configuration["JWT:ValidIssuer"],
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
                };

                // Validate the token signature
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out validatedToken);

                return principal;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Tuple<string, DateTime>> GenerateToken(UserDetail user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(this.configuration["JWT:Secret"]);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName)
                        // Add other claims as needed
            };

            var roles = await userService.GetUserRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            DateTime expiry = DateTime.UtcNow.AddDays(7);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiry,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = this.configuration["JWT:ValidIssuer"],
                Audience = this.configuration["JWT:ValidAudience"],
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            string exportedToken = tokenHandler.WriteToken(token);

            Tuple<string, DateTime> output = new Tuple<string, DateTime>(exportedToken, expiry);

            return await Task.FromResult<Tuple<string, DateTime>>(output);
        }
    }
}
