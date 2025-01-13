using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Streetcode.BLL.Interfaces.Authentication;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Persistence;
using Streetcode.BLL.Utils.Options;

namespace Streetcode.BLL.Services.Authentication
{
    public sealed class TokenService : ITokenService
    {
        public const string InvalidRefreshTokenErrorMessage = "Invalid refresh token";
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

        public async Task<JwtSecurityToken> GenerateAccessTokenAsync(User user)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var tokenDescriptor = await GetTokenDescriptorAsync(user);
            var token = _jwtSecurityTokenHandler.CreateJwtSecurityToken(tokenDescriptor);

            return token;
        }

        public JwtSecurityToken RefreshToken(string accessToken, string refreshToken)
        {
            var principles = GetPrinciplesFromExpiredToken(accessToken);
            string userEmail = principles?.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value ?? string.Empty;

            User user = GetUserFromDb(userEmail);
            if (user.RefreshToken != refreshToken || user.RefreshTokenExpiry <= DateTime.UtcNow)
            {
                throw new Exception(InvalidRefreshTokenErrorMessage);
            }

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(principles?.Claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenLifetimeInMinutes),
                SigningCredentials = _signingCredentials,
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience
            };
            var newToken = _jwtSecurityTokenHandler.CreateJwtSecurityToken(tokenDescriptor);

            return newToken;
        }

        public string SetNewRefreshTokenForUser(User user)
        {
            User userFromDb = GetUserFromDb(user.Email!);

            string refreshToken = GenerateRefreshToken();
            userFromDb.RefreshToken = refreshToken;
            userFromDb.RefreshTokenExpiry = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenLifetimeInDays);
            _dbContext.Users.Update(userFromDb);
            _dbContext.SaveChanges();

            return refreshToken;
        }

        private User GetUserFromDb(string userEmail)
        {
            User? userFromDb = _dbContext.Users
                .FirstOrDefault(userInDb => userInDb.Email == (userEmail ?? string.Empty));
            string errorMessage = $"Cannot find in database user with Email: {userEmail}";

            return userFromDb ?? throw new NullReferenceException(errorMessage);
        }

        private ClaimsPrincipal GetPrinciplesFromExpiredToken(string token)
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
                ValidateLifetime = false,
                ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256 },
            };

            ClaimsPrincipal claimsPrincipal;
            try
            {
                claimsPrincipal = _jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out _);
            }
            catch (SecurityTokenValidationException ex)
            {
                throw new SecurityTokenValidationException(ex.Message, ex);
            }

            return claimsPrincipal;
        }

        private Task<SecurityTokenDescriptor> GetTokenDescriptorAsync(User user)
        {
            var userRoleId = _dbContext.UserRoles
                .AsNoTracking()
                .Where(userRole => userRole.UserId == user.Id)
                .Select(userRole => userRole.RoleId)
                .FirstOrDefault();
            var userRole = _dbContext.Roles
                .AsNoTracking()
                .FirstOrDefault(role => role.Id == userRoleId);
            string userRoleName = userRole?.Name ??
                throw new NullReferenceException($"Cannot find role for user ${user.UserName}");

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                    new Claim(ClaimTypes.Surname, user.Surname),
                    new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                    new Claim(ClaimTypes.Role, userRoleName),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id)
                }),
                Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenLifetimeInMinutes),
                SigningCredentials = _signingCredentials,
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience
            };

            return Task.FromResult(tokenDescriptor);
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
