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
using System.Security.AccessControl;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests
{
    public abstract class BaseControllerTests<T> : IntegrationTestBase, IClassFixture<CustomWebApplicationFactory<Program>>, IDisposable
    {
        protected T client;

        public BaseControllerTests(CustomWebApplicationFactory<Program> factory, string secondPartUrl = "")
        {
            this.client = ClientInitializer<T>.Initialize(factory.CreateClient(), secondPartUrl);
            ITokenService tokenService = this.GetTokenService(factory.Services);
            UserManager<User> userManager = this.GetUserManager(factory.Services);
            TokenStorage.Configure(userManager, tokenService);
        }

        protected static string UserToken { get => TokenStorage.UserToken; }

        protected static string AdminToken { get => TokenStorage.AdminToken; }

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

        public abstract void Dispose();

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
