using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using Streetcode.DAL.Entities.Users;

namespace Streetcode.BLL.Interfaces.Authentication
{
    public interface ITokenService
    {
        public Task<JwtSecurityToken> GenerateAccessTokenAsync(User user);
        public JwtSecurityToken RefreshToken(string token);
        public Task<string> GetRefreshTokenAsync(User user);
    }
}
