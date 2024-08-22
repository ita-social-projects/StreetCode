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
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.MaintainabilityRules",
        "SA1402:File may only contain a single type",
        Justification = "It's ok to have two classes that differ only by a generic argument in one file")]
    public abstract class BaseControllerTests<T> : IntegrationTestBase, IClassFixture<CustomWebApplicationFactory<Program>>, IDisposable
    {
        private bool disposed = false;

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

            if (disposing && this.Client is IDisposable disposableClient)
            {
                disposableClient.Dispose();
            }

            this.disposed = true;
        }
    }

    public class BaseControllerTests : BaseControllerTests<BaseClient>
    {
        public BaseControllerTests(CustomWebApplicationFactory<Program> factory, ITokenService tokenService, UserManager<User> userManager, string secondPartUrl = "")
            : base(factory, secondPartUrl)
        {
        }
    }
}
