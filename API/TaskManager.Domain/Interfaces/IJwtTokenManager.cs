
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces
{
    public interface IJwtTokenManager
    {
        Task<Tuple<string, DateTime>> GenerateToken(UserDetail user);
        Task<ClaimsPrincipal> DecodeJwtAccessToken(string token);


    }
}
