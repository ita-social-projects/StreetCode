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

namespace Streetcode.XIntegrationTest.ControllerTests.BaseController
{
    public abstract class BaseControllerTests<T> : IntegrationTestBase, IClassFixture<CustomWebApplicationFactory<Program>>, IDisposable
    {
        protected T client;

        public BaseControllerTests(CustomWebApplicationFactory<Program> factory, string secondPartUrl)
        {
            client = ClientInitializer<T>.Initialize(factory.CreateClient(), secondPartUrl);
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
