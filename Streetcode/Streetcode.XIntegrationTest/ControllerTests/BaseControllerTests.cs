using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Streetcode.BLL.Interfaces.Authentication;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Persistence;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Base;
using System.IdentityModel.Tokens.Jwt;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests
{
    public abstract class BaseControllerTests<T> : IntegrationTestBase, IClassFixture<CustomWebApplicationFactory<Program>>, IDisposable
    {
        protected T client;
        protected string _userToken;
        protected string _adminToken;
        private ITokenService _tokenService;
        private UserManager<User> _userManager;

        public BaseControllerTests(CustomWebApplicationFactory<Program> factory, string secondPartUrl = "")
        {
            this.client = ClientInitializer<T>.Initialize(factory.CreateClient(), secondPartUrl);
            this._tokenService = this.GetTokenService(factory.Services);
            this._userManager = this.GetUserManager(factory.Services);
            this._userToken = this.GetUserJwtToken();
            this._adminToken = this.GetAdminJwtToken();
        }

        public static SqlDbHelper GetSqlDbHelper()
        {
            var sqlConnectionString = new ConfigurationBuilder()
                .AddJsonFile("appsettings.IntegrationTests.json")
                .Build()
                .GetConnectionString("DefaultConnection");
            var optionBuilder = new DbContextOptionsBuilder<StreetcodeDbContext>();
            optionBuilder.UseSqlServer(sqlConnectionString);
            return new SqlDbHelper(optionBuilder.Options);
        }

        private ITokenService GetTokenService(IServiceProvider serviceProvider)
        {
            var scope = serviceProvider.CreateScope();
            ITokenService tokenService = scope.ServiceProvider.GetRequiredService<ITokenService>();
            return tokenService;
        }

        private UserManager<User> GetUserManager(IServiceProvider serviceProvider)
        {
            var scope = serviceProvider.CreateScope();
            UserManager<User> userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            return userManager;
        }

        private string GetUserJwtToken()
        {
            return this.GetJwtToken(UserRole.User);
        }

        private string GetAdminJwtToken()
        {
            return this.GetJwtToken(UserRole.Admin);
        }

        private string GetJwtToken(UserRole userRole)
        {
            var sqlDbHealper = GetSqlDbHelper();
            (User testUser, string password) = this.GetTestUser(userRole);
            User userFromDb = sqlDbHealper.GetExistItem<User>(user => user.Email == testUser.Email);
            if (userFromDb is null)
            {
                this._userManager.CreateAsync(testUser, password).GetAwaiter().GetResult();
                this._userManager.AddToRoleAsync(testUser, userRole.ToString()).GetAwaiter().GetResult();
            }

            JwtSecurityToken securityToken = this._tokenService.GenerateJWTToken(testUser);
            return securityToken.RawData;
        }

        private (User, string password) GetTestUser(UserRole userRole)
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

        public abstract void Dispose();
    }

    public class BaseControllerTests : BaseControllerTests<BaseClient>
    {
        public BaseControllerTests(CustomWebApplicationFactory<Program> factory, ITokenService tokenService, UserManager<User> userManager, string secondPartUrl = "")
            : base(factory, secondPartUrl)
        {
        }

        public override void Dispose()
        {
        }
    }
}
