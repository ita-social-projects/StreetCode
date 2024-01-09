using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using Streetcode.BLL.Interfaces.Users;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Persistence;

namespace Streetcode.BLL.Services.Users
{
    public sealed class TokenService : ITokenService
    {
        private readonly SigningCredentials _signInCridentials;
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;
        private readonly string _jwtKey;
        private readonly IStringLocalizer<TokenService> _stringLocalizer;
        private readonly StreetcodeDbContext _dbContext;

        public TokenService(IConfiguration configuration, IStringLocalizer<TokenService> stringLocalizer, StreetcodeDbContext dbContext)
        {
            _jwtIssuer = configuration["Jwt:Issuer"];
            _jwtAudience = configuration["Jwt:Audience"];
            _jwtKey = configuration["Jwt:Key"];

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            _signInCridentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            _stringLocalizer = stringLocalizer;
            _dbContext = dbContext;
        }

        public JwtSecurityToken GenerateJWTToken(User user)
        {
            // Get roles of user.
            var userRoleId = _dbContext.UserRoles.AsNoTracking()
                .Where(userRole => userRole.UserId == user.Id)
                .Select(userRole => userRole.RoleId)
                .FirstOrDefault();
            var userRoleName = _dbContext.Roles.AsNoTracking()
                .FirstOrDefault(role => role.Id == userRoleId) !.Name;

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Surname, user.Surname),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, userRoleName),
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = _signInCridentials,
                Issuer = _jwtIssuer,
                Audience = _jwtAudience
            };
            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

            return token;
        }

        public JwtSecurityToken RefreshToken(string token)
        {
            var principles = GetPrinciplesFromToken(token);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(principles.Claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = _signInCridentials,
                Issuer = _jwtIssuer,
                Audience = _jwtAudience
            };
            var newToken = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

            return newToken;
        }

        private ClaimsPrincipal GetPrinciplesFromToken(string token)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidAudience = _jwtAudience,
                ValidIssuer = _jwtIssuer,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                LifetimeValidator = (DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters) =>
                {
                    if (expires == null)
                    {
                        return false;
                    }

                    return (DateTime.Now.AddHours(1) - expires).Value.TotalSeconds > 0;
                },
                ValidateLifetime = true
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException(_stringLocalizer["InvalidToken"].Value);
            }

            return principal;
        }
    }
}
