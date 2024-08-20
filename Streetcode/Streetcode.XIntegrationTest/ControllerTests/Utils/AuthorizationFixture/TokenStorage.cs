using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Streetcode.BLL.Interfaces.Authentication;
using Streetcode.BLL.Services.Authentication;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Persistence;
using Streetcode.WebApi.Extensions;
using static Streetcode.XIntegrationTest.Constants.ControllerTests.AuthConstants;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils
{
    public class TokenStorage : IDisposable
    {
        private readonly StreetcodeDbContext _streetcodeDbContext;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;
        private readonly Dictionary<string, User> _users;

        private bool _disposed = false;

        public TokenStorage()
        {
            this._users = new Dictionary<string, User>
            {
                ["Admin"] = TEST_USER_ADMIN,
                ["User"] = TEST_USER_USER,
            };

            this._streetcodeDbContext = this.GetDbContext();
            this._configuration = GetConfiguration();

            this._tokenService = new TokenService(this._configuration, this._streetcodeDbContext);

            this.ObtainTokensAsync().GetAwaiter().GetResult();
        }

        public string AdminAccessToken { get; private set; } = null!;

        public string UserAccessToken { get; private set; } = null!;

        public string AdminRefreshToken { get; private set; } = null!;

        public string UserRefreshToken { get; private set; } = null!;

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this._disposed)
            {
                return;
            }

            if (disposing)
            {
                this._streetcodeDbContext.Dispose();
            }

            this._disposed = true;
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
            this.AdminAccessToken = (await this._tokenService.GenerateAccessTokenAsync(this._users["Admin"])).RawData;
            this.UserAccessToken = (await this._tokenService.GenerateAccessTokenAsync(this._users["User"])).RawData;
            this.AdminRefreshToken = this._tokenService.SetNewRefreshTokenForUser(this._users["Admin"]);
            this.UserRefreshToken = this._tokenService.SetNewRefreshTokenForUser(this._users["User"]);
        }
    }
}
