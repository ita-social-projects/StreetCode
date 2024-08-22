using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Streetcode.DAL.Persistence;

namespace Streetcode.XIntegrationTest.Base
{
    public class TestDBFixture : IntegrationTestBase
    {
        private static readonly object @lock = new ();
        private static bool dbIsCreated;
        private readonly string connectionString;

        public TestDBFixture()
        {
            this.connectionString = this.Configuration.GetConnectionString("DefaultConnection") !;
            lock (@lock)
            {
                if (!dbIsCreated)
                {
                    using (var context = CreateContext(this.connectionString))
                    {
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();
                    }

                    dbIsCreated = true;
                }
            }
        }

        public static StreetcodeDbContext CreateContext(string connectionString)
            => new StreetcodeDbContext(
                new DbContextOptionsBuilder<StreetcodeDbContext>()
                    .UseSqlServer(connectionString)
                    .Options);
    }
}
