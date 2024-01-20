using Microsoft.AspNetCore.Identity;
using Streetcode.BLL.Interfaces.Authentication;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Enums;
using System.IdentityModel.Tokens.Jwt;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils
{
    public static class TokenStorage
    {
        private static UserManager<User> _userManager;
        private static ITokenService _tokenService;

        private static readonly object _configurationLock = new object();
        private static readonly object _userTokenLock = new object();
        private static readonly object _adminTokenLock = new object();
        private static string _userToken = string.Empty;
        private static string _adminToken = string.Empty;

        public static string UserToken
        {
            get
            {
                lock (_userTokenLock)
                {
                    if (string.IsNullOrEmpty(_userToken))
                    {
                        _userToken = GetUserJwtToken();
                    }

                    return _userToken;
                }
            }
        }

        public static string AdminToken
        {
            get
            {
                lock (_adminTokenLock)
                {
                    if (string.IsNullOrEmpty(_adminToken))
                    {
                        _adminToken = GetAdminJwtToken();
                    }

                    return _adminToken;
                }
            }
        }

        public static void Configure(UserManager<User> userManager, ITokenService tokenService)
        {
            lock (_configurationLock)
            {
                _userManager = userManager;
                _tokenService = tokenService;
            }
        }

        private static string GetUserJwtToken()
        {
            return GetJwtTokenAsync(nameof(UserRole.User)).GetAwaiter().GetResult();
        }

        private static string GetAdminJwtToken()
        {
            return GetJwtTokenAsync(nameof(UserRole.Admin)).GetAwaiter().GetResult();
        }

        private static async Task<string> GetJwtTokenAsync(string userRole)
        {
            (User testUser, string password) = GetTestUser(userRole);
            User actualUser = await _userManager.FindByIdAsync(testUser.Id);
            if (actualUser is null)
            {
                actualUser = testUser;
                await _userManager.CreateAsync(actualUser, password);
            }

            if (!(await _userManager.IsInRoleAsync(actualUser, userRole)))
            {
                await _userManager.AddToRoleAsync(actualUser, userRole);
            }

            JwtSecurityToken securityToken = _tokenService.GenerateJWTToken(actualUser);
            return securityToken.RawData;
        }

        private static (User, string password) GetTestUser(string userRole)
        {
            User testUser = new User
            {
                Id = $"test_{userRole}_clsdkmcd29384IJDAlnfsdfd",
                Name = $"Test_{userRole}",
                Surname = $"Test_{userRole}",
                Email = $"test_{userRole}@test.com",
                UserName = $"test_{userRole}_T",
            };

            return (testUser, password: "cdsLMC123(*sda1!@$sa");
        }
    }
}
