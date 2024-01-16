using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Streetcode.DAL.Persistence;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Xunit;
namespace Streetcode.XIntegrationTest.ControllerTests
{
    public abstract class BaseControllerTests<T> : IntegrationTestBase, IClassFixture<CustomWebApplicationFactory<Program>>, IDisposable
    {
        protected T client;

        public BaseControllerTests(CustomWebApplicationFactory<Program> factory, string secondPartUrl = "")
        {
            this.client = ClientInitializer<T>.Initialize(factory.CreateClient(), secondPartUrl);
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

    public class BaseControllerTests : BaseControllerTests<StreetcodeClient>
    {
        public BaseControllerTests(CustomWebApplicationFactory<Program> factory, string secondPartUrl = "")
            : base(factory, secondPartUrl)
        {
        }

        public override void Dispose()
        {
        }
    }
}
