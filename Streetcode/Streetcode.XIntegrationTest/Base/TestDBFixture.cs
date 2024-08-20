using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Streetcode.DAL.Persistence;

namespace Streetcode.XIntegrationTest.Base
{
    public class TestDBFixture : IntegrationTestBase
    {
        private static readonly object _lock = new ();
        private static bool _dbIsCreated;
        private readonly string connectionString;

        public TestDBFixture()
        {
            this.connectionString = this.Configuration.GetConnectionString("DefaultConnection") !;
            lock (_lock)
            {
                if (!_dbIsCreated)
                {
                    using (var context = CreateContext(this.connectionString))
                    {
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();
                    }

                    _dbIsCreated = true;
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
