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

namespace Streetcode.BLL.Services.Authentication
{
    public sealed class TokenService : ITokenService
    {
        private readonly SigningCredentials _signingCredentials;
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;
        private readonly string _jwtKey;
        private readonly IStringLocalizer<TokenService> _stringLocalizer;
        private readonly StreetcodeDbContext _dbContext;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

        public TokenService(IConfiguration configuration, IStringLocalizer<TokenService> stringLocalizer, StreetcodeDbContext dbContext)
        {
            _jwtIssuer = configuration["Jwt:Issuer"] !;
            _jwtAudience = configuration["Jwt:Audience"] !;
            _jwtKey = configuration["Jwt:Key"] !;

            _stringLocalizer = stringLocalizer;
            _dbContext = dbContext;

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            _signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        }

        public JwtSecurityToken GenerateJWTToken(User? user)
        {
            // Throw exception if argument is null.
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            // Get roles of user.
            var userRoleId = _dbContext.UserRoles
                .AsNoTracking()
                .Where(userRole => userRole.UserId == user.Id)
                .Select(userRole => userRole.RoleId)
                .FirstOrDefault();
            var userRoleName = _dbContext.Roles
                .AsNoTracking()
                .FirstOrDefault(role => role.Id == userRoleId) !.Name;

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
                SigningCredentials = _signingCredentials,
                Issuer = _jwtIssuer,
                Audience = _jwtAudience
            };
            var token = _jwtSecurityTokenHandler.CreateJwtSecurityToken(tokenDescriptor);

            return token;
        }

        public JwtSecurityToken RefreshToken(string token)
        {
            var principles = GetPrinciplesFromToken(token);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(principles.Claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = _signingCredentials,
                Issuer = _jwtIssuer,
                Audience = _jwtAudience
            };
            var newToken = _jwtSecurityTokenHandler.CreateJwtSecurityToken(tokenDescriptor);

            return newToken;
        }

        private ClaimsPrincipal GetPrinciplesFromToken(string token)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidAudience = _jwtAudience,
                ValidIssuer = _jwtIssuer,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                ValidateLifetime = true,
                ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256 },
            };

            ClaimsPrincipal claimsPrincipal;
            SecurityToken securityToken;
            try
            {
                claimsPrincipal = _jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            }
            catch (SecurityTokenValidationException ex)
            {
                throw new SecurityTokenValidationException(ex.Message, ex);
            }

            var jwtSecurityToken = securityToken as JwtSecurityToken;
            return claimsPrincipal;
        }
    }
}
