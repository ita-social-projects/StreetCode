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
        private readonly StreetcodeDbContext streetcodeDbContext;
        private readonly IConfiguration configuration;
        private readonly ITokenService tokenService;
        private readonly Dictionary<string, User> users;

        private bool disposed = false;

        public TokenStorage()
        {
            this.users = new Dictionary<string, User>
            {
                ["Admin"] = TEST_USER_ADMIN,
                ["User"] = TEST_USER_USER,
            };

            this.streetcodeDbContext = this.GetDbContext();
            this.configuration = GetConfiguration();

            this.tokenService = new TokenService(this.configuration, this.streetcodeDbContext);

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
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                this.streetcodeDbContext.Dispose();
            }

            this.disposed = true;
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
            this.AdminAccessToken = (await this.tokenService.GenerateAccessTokenAsync(this.users["Admin"])).RawData;
            this.UserAccessToken = (await this.tokenService.GenerateAccessTokenAsync(this.users["User"])).RawData;
            this.AdminRefreshToken = this.tokenService.SetNewRefreshTokenForUser(this.users["Admin"]);
            this.UserRefreshToken = this.tokenService.SetNewRefreshTokenForUser(this.users["User"]);
        }
    }
}
