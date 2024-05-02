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

        public TokenStorage()
        {
            this._users = new Dictionary<string, User>
            {
                ["Admin"] = TEST_USER_ADMIN,
                ["User"] = TEST_USER_USER,
            };

            this._streetcodeDbContext = GetDbContext();
            this._configuration = GetConfiguration();

            this._tokenService = new TokenService(this._configuration, this._streetcodeDbContext);

            this.ObtainTokens();
        }

        public string AdminToken { get; private set; }

        public string UserToken { get; private set; }

        public void Dispose()
        {
            this._streetcodeDbContext.Dispose();
        }

        private static StreetcodeDbContext GetDbContext()
        {
            var sqlConnectionString = new ConfigurationBuilder()
                .AddJsonFile("appsettings.IntegrationTests.json")
                .Build()
                .GetConnectionString("DefaultConnection");
            var optionBuilder = new DbContextOptionsBuilder<StreetcodeDbContext>();
            optionBuilder.UseSqlServer(sqlConnectionString);
            return new StreetcodeDbContext(optionBuilder.Options);
        }

        private static IConfiguration GetConfiguration()
        {
            Environment.SetEnvironmentVariable("STREETCODE_ENVIRONMENT", "IntegrationTests");
            var environment = Environment.GetEnvironmentVariable("STREETCODE_ENVIRONMENT") ?? "Local";

            IConfigurationBuilder configBuilder = new ConfigurationBuilder()
                .ConfigureCustom(environment);

            return configBuilder.Build();
        }

        private void ObtainTokens()
        {
            this.AdminToken = this._tokenService.GenerateJWTToken(this._users["Admin"]).RawData;
            this.UserToken = this._tokenService.GenerateJWTToken(this._users["User"]).RawData;
        }
    }
}
