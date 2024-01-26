using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using Streetcode.BLL.Interfaces.Authentication;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Persistence;
using Streetcode.BLL.Utils.Options;

namespace Streetcode.BLL.Services.Authentication
{
    public sealed class TokenService : ITokenService
    {
        private readonly SigningCredentials _signingCredentials;
        private readonly JwtOptions _jwtOptions;
        private readonly StreetcodeDbContext _dbContext;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

        public TokenService(IConfiguration configuration, StreetcodeDbContext dbContext)
        {
            _jwtOptions = configuration
              .GetSection("Jwt")
              .Get<JwtOptions>() !;

            _dbContext = dbContext;

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
            _signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        }

        public JwtSecurityToken GenerateJWTToken(User? user)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var userRoleId = _dbContext.UserRoles
                .AsNoTracking()
                .Where(userRole => userRole.UserId == user.Id)
                .Select(userRole => userRole.RoleId)
                .FirstOrDefault();
            var userRole = _dbContext.Roles
                .AsNoTracking()
                .FirstOrDefault(role => role.Id == userRoleId);
            string userRoleName = userRole!.Name;

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Surname, user.Surname),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, userRoleName),
                }),
                Expires = DateTime.UtcNow.AddHours(_jwtOptions.LifetimeInHours),
                SigningCredentials = _signingCredentials,
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience
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
                Expires = DateTime.UtcNow.AddHours(_jwtOptions.LifetimeInHours),
                SigningCredentials = _signingCredentials,
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience
            };
            var newToken = _jwtSecurityTokenHandler.CreateJwtSecurityToken(tokenDescriptor);

            return newToken;
        }

        private ClaimsPrincipal GetPrinciplesFromToken(string token)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidAudience = _jwtOptions.Audience,
                ValidIssuer = _jwtOptions.Issuer,
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

            return claimsPrincipal;
        }
    }
}
