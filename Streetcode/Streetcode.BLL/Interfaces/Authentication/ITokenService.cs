using System.IdentityModel.Tokens.Jwt;
using Streetcode.DAL.Entities.Users;

namespace Streetcode.BLL.Interfaces.Authentication
{
    public interface ITokenService
    {
        public Task<JwtSecurityToken> GenerateAccessTokenAsync(User user);
        public JwtSecurityToken RefreshToken(string accessToken, string refreshToken);
        public string GetRefreshTokenData(User user);
    }
}
