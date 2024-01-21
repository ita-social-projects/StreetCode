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

        public TokenStorage()
        {
            this._streetcodeDbContext = this.GetDbContext();
            this.SeedDatabaseWithInitialUsers();

            this._configuration = this.GetConfiguration();
            this._tokenService = new TokenService(this._configuration, this._streetcodeDbContext);

            this.ObtainTokens();
        }

        public void Dispose()
        {
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
            User testAdmin = this.GetUserFromDb(nameof(UserRole.Admin));
            User testUser = this.GetUserFromDb(nameof(UserRole.User));

            this.AdminToken = this._tokenService.GenerateJWTToken(testAdmin).RawData;
            this.UserToken = this._tokenService.GenerateJWTToken(testUser).RawData;
        }

        private void SeedDatabaseWithInitialUsers()
        {
            IdentityRole adminRole = this.GetRoleFromDb(nameof(UserRole.Admin));
            IdentityRole userRole = this.GetRoleFromDb(nameof(UserRole.User));
            User testAdmin = this.GetUserFromDb(nameof(UserRole.Admin));
            User testUser = this.GetUserFromDb(nameof(UserRole.User));

            this.AddUserRole(testAdmin, adminRole);
            this.AddUserRole(testUser, userRole);
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
            IdentityUserRole<string>? actualUserRole = this._streetcodeDbContext
                .UserRoles
                .AsNoTracking()
                .FirstOrDefault(userRole => userRole.UserId == user.Id && userRole.RoleId == role.Id);

            if (actualUserRole is null)
            {
                actualUserRole = new IdentityUserRole<string>
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
