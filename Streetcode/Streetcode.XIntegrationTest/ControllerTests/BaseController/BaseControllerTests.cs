using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Streetcode.BLL.Interfaces.Authentication;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Persistence;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Base;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.BaseController
{
#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable S3881 // "IDisposable" should be implemented correctly

    public abstract class BaseControllerTests<T> : IntegrationTestBase, IClassFixture<CustomWebApplicationFactory<Program>>, IDisposable
    {
        protected BaseControllerTests(CustomWebApplicationFactory<Program> factory, string secondPartUrl)
        {
            this.Client = ClientInitializer<T>.Initialize(factory.CreateClient(), secondPartUrl);
        }

        protected T Client { get; set; }

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

#pragma warning restore S3881 // "IDisposable" should be implemented correctly
#pragma warning restore SA1402 // File may only contain a single type

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
