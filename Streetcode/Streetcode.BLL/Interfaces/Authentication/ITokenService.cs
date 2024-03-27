using System.IdentityModel.Tokens.Jwt;
using Streetcode.DAL.Entities.Users;

namespace Streetcode.BLL.Interfaces.Authentication
{
    public interface ITokenService
    {
        public JwtSecurityToken GenerateJWTToken(User user);
        public JwtSecurityToken RefreshToken(string token);
    }
}
