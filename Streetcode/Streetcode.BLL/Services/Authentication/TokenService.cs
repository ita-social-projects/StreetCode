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
        private readonly UserManager<User> _userManager;

        public TokenService(IConfiguration configuration, StreetcodeDbContext dbContext, UserManager<User> userManager)
        {
            _jwtOptions = configuration
              .GetSection("Jwt")
              .Get<JwtOptions>()!;

            _dbContext = dbContext;
            _userManager = userManager;

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
            _signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        }

        public async Task<JwtSecurityToken> GenerateAccessTokenAsync(User? user)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var tokenDescriptor = await GetTokenDescriptorAsync(user);
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

        public async Task<string> GetRefreshTokenAsync(User user)
        {
            string tokenPurpose = "refresh";
            string tokenName = "RefreshToken";
            string tokenProvider = _jwtOptions.Issuer;
            string? previousRefreshToken = await _userManager.GetAuthenticationTokenAsync(user, tokenProvider, tokenName);

            // Delete old refresh token, generate new one and add it to DB.
            if (previousRefreshToken is not null)
            {
                Console.WriteLine(previousRefreshToken);
                await _userManager.RemoveAuthenticationTokenAsync(user, _jwtOptions.Issuer, tokenName);
            }

            string refreshToken = await _userManager
                .GenerateUserTokenAsync(user!, _jwtOptions.Issuer, tokenPurpose);
            await _userManager
                .SetAuthenticationTokenAsync(user, _jwtOptions.Issuer, tokenName, refreshToken);

            return refreshToken;
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

        private async Task<SecurityTokenDescriptor> GetTokenDescriptorAsync(User user)
        {
            var userRolesList = await _userManager.GetRolesAsync(user);
            string userRole = userRolesList.FirstOrDefault() ?? string.Empty;

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Surname, user.Surname),
                    new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                    new Claim(ClaimTypes.Role, userRole),
                }),
                Expires = DateTime.UtcNow.AddHours(_jwtOptions.LifetimeInHours),
                SigningCredentials = _signingCredentials,
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience
            };

            return tokenDescriptor;
        }
    }
}
