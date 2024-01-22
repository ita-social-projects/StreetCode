using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Streetcode.BLL.Interfaces.Authentication;
using Streetcode.BLL.Services.Authentication;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Persistence;
using Streetcode.WebApi.Extensions;

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

            this.ObtainTokens();
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

        private void ObtainTokens()
        {
            this.AdminToken = this._tokenService.GenerateJWTToken(this._users["Admin"]).RawData;
            this.UserToken = this._tokenService.GenerateJWTToken(this._users["User"]).RawData;
        }

        private void SeedDatabaseWithInitialUsers()
        {
            IdentityRole adminRole = this.GetRoleFromDb(nameof(UserRole.Admin));
            IdentityRole userRole = this.GetRoleFromDb(nameof(UserRole.User));
            this._users["Admin"] = this.GetUserFromDb(nameof(UserRole.Admin));
            this._users["User"] = this.GetUserFromDb(nameof(UserRole.User));

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

        private IdentityRole GetRoleFromDb(string roleName)
        {
            IdentityRole testRole = this.GetTestRole(roleName);
            IdentityRole? actualRole = this._streetcodeDbContext
                .Roles
                .FirstOrDefault(roleFromDb => roleFromDb.Id == testRole.Id);
            if (actualRole is null)
            {
                actualRole = this._streetcodeDbContext.Roles.Add(testRole).Entity;
                this._streetcodeDbContext.SaveChanges();
            }

            return actualRole;
        }

        private User GetUserFromDb(string roleName)
        {
            User testUser = this.GetTestUser(roleName);
            User? actualUser = this._streetcodeDbContext
                .Users
                .FirstOrDefault(roleFromDb => roleFromDb.Id == testUser.Id);
            if (actualUser is null)
            {
                actualUser = this._streetcodeDbContext.Users.Add(testUser).Entity;
                this._streetcodeDbContext.SaveChanges();
            }

            return actualUser;
        }

        private void AddUserRole(User user, IdentityRole role)
        {
            bool exists = this._streetcodeDbContext
                .UserRoles
                .AsNoTracking()
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

        private IdentityRole GetTestRole(string roleName)
        {
           return new IdentityRole
            {
                Id = $"test_role_{roleName}_clsdkmcd29384IJDAlnfsdfd",
                Name = roleName,
            };
        }

        private User GetTestUser(string userRole)
        {
            return new User
            {
                Id = $"test_user_{userRole}_clsdkmcd29384IJDAlnfsdfd",
                Name = $"Test_{userRole}",
                Surname = $"Test_{userRole}",
                Email = $"test_{userRole}@test.com",
                UserName = $"test_{userRole}_T",
            };
        }
    }
}
