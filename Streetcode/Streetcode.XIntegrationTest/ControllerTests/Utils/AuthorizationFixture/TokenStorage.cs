using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Streetcode.BLL.Interfaces.Authentication;
using Streetcode.BLL.Services.Authentication;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Persistence;
using Streetcode.WebApi.Extensions;
using static Streetcode.XIntegrationTest.Constants.ControllerTests.AuthConstants;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.AuthorizationFixture
{
    public class TokenStorage : IDisposable
    {
        private readonly StreetcodeDbContext _streetcodeDbContext;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;
        private readonly Dictionary<string, User> _users;

        private bool disposed;

        public TokenStorage()
        {
            _users = new Dictionary<string, User>
            {
                ["Admin"] = TEST_USER_ADMIN,
                ["User"] = TEST_USER_USER,
            };

            _streetcodeDbContext = GetDbContext();
            _configuration = GetConfiguration();

            _tokenService = new TokenService(_configuration, _streetcodeDbContext);

            ObtainTokensAsync().GetAwaiter().GetResult();
        }

        public string AdminAccessToken { get; private set; } = null!;

        public string UserAccessToken { get; private set; } = null!;

        public string AdminRefreshToken { get; private set; } = null!;

        public string UserRefreshToken { get; private set; } = null!;

        public async Task GenerateNewTokens(User user)
        {
            UserAccessToken = (await _tokenService.GenerateAccessTokenAsync(user)).RawData;
            UserRefreshToken = _tokenService.SetNewRefreshTokenForUser(user);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _streetcodeDbContext.Dispose();
                }

                disposed = true;
            }
        }

        private static IConfiguration GetConfiguration()
        {
            Environment.SetEnvironmentVariable("STREETCODE_ENVIRONMENT", "IntegrationTests");
            var environment = Environment.GetEnvironmentVariable("STREETCODE_ENVIRONMENT") ?? "Local";

            IConfigurationBuilder configBuilder = new ConfigurationBuilder()
                .ConfigureCustom(environment);

            return configBuilder.Build();
        }

        private StreetcodeDbContext GetDbContext()
        {
            var sqlConnectionString = new ConfigurationBuilder()
                .AddJsonFile("appsettings.IntegrationTests.json")
                .Build()
                .GetConnectionString("DefaultConnection");
            var optionBuilder = new DbContextOptionsBuilder<StreetcodeDbContext>();
            optionBuilder.UseSqlServer(sqlConnectionString);
            return new StreetcodeDbContext(optionBuilder.Options);
        }

        private async Task ObtainTokensAsync()
        {
            AdminAccessToken = (await _tokenService.GenerateAccessTokenAsync(_users["Admin"])).RawData;
            UserAccessToken = (await _tokenService.GenerateAccessTokenAsync(_users["User"])).RawData;
            AdminRefreshToken = _tokenService.SetNewRefreshTokenForUser(_users["Admin"]);
            UserRefreshToken = _tokenService.SetNewRefreshTokenForUser(_users["User"]);
        }
    }
}
