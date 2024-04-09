using Microsoft.AspNetCore.Identity;
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
            this._streetcodeDbContext = this.GetDbContext();
            this._users = new Dictionary<string, User>();
            this.SeedDatabaseWithInitialUsers();

            this._configuration = this.GetConfiguration();
            this._tokenService = new TokenService(this._configuration, this._streetcodeDbContext);

            this.ObtainTokensAsync().GetAwaiter().GetResult();
        }

        public void Dispose()
        {
            this.RemoveTestUsers();
            this._streetcodeDbContext.Dispose();
        }

        public string AdminToken { get; private set; }

        public string UserToken { get; private set; }

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

        private IConfiguration GetConfiguration()
        {
            Environment.SetEnvironmentVariable("STREETCODE_ENVIRONMENT", "IntegrationTests");
            var environment = Environment.GetEnvironmentVariable("STREETCODE_ENVIRONMENT") ?? "Local";

            IConfigurationBuilder configBuilder = new ConfigurationBuilder()
                .ConfigureCustom(environment);

            return configBuilder.Build();
        }

        private async Task ObtainTokensAsync()
        {
            this.AdminToken = (await this._tokenService.GenerateAccessTokenAsync(this._users["Admin"])).RawData;
            this.UserToken = (await this._tokenService.GenerateAccessTokenAsync(this._users["User"])).RawData;
        }

        private void SeedDatabaseWithInitialUsers()
        {
            IdentityRole adminRole = TEST_ROLE_ADMIN;
            IdentityRole userRole = TEST_ROLE_USER;
            this.AddRoleToDb(adminRole);
            this.AddRoleToDb(userRole);

            this._users["Admin"] = TEST_USER_ADMIN;
            this._users["User"] = TEST_USER_USER;
            this.AddUserToDb(this._users["Admin"]);
            this.AddUserToDb(this._users["User"]);

            this.AddUserRole(this._users["Admin"], adminRole);
            this.AddUserRole(this._users["User"], userRole);
        }

        private void RemoveTestUsers()
        {
            foreach (string key in this._users.Keys)
            {
                var userFromDb = this._streetcodeDbContext.Users.FirstOrDefault(user => user.Id == this._users[key].Id);
                if (userFromDb is not null)
                {
                    this._streetcodeDbContext.Remove(userFromDb);
                    this._streetcodeDbContext.SaveChanges();
                }
            }
        }

        private void AddRoleToDb(IdentityRole role)
        {
            bool exists = this._streetcodeDbContext.Roles.Any(roleFromDb => roleFromDb.Id == role.Id);
            if (!exists)
            {
                this._streetcodeDbContext.Roles.Add(role);
                this._streetcodeDbContext.SaveChanges();
            }
        }

        private void AddUserToDb(User user)
        {
            bool exists = this._streetcodeDbContext.Users.Any(userFromDb => userFromDb.Id == user.Id);
            if (!exists)
            {
                this._streetcodeDbContext.Users.Add(user);
                this._streetcodeDbContext.SaveChanges();
            }
        }

        private void AddUserRole(User user, IdentityRole role)
        {
            bool exists = this._streetcodeDbContext
                .UserRoles
                .Any(userRole => userRole.UserId == user.Id && userRole.RoleId == role.Id);

            if (!exists)
            {
                var actualUserRole = new IdentityUserRole<string>
                {
                    UserId = user.Id,
                    RoleId = role.Id,
                };
                this._streetcodeDbContext.UserRoles.Add(actualUserRole);
                this._streetcodeDbContext.SaveChanges();
            }
        }
    }
}
