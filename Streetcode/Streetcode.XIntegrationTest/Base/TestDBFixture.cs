using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Polly;
using Streetcode.DAL.Persistence;

namespace Streetcode.XIntegrationTest.Base
{
    public class TestDBFixture : IntegrationTestBase
    {
        private readonly string ConnectionString;
        private static readonly object _lock = new();
        private static bool _dbIsCreated;

        public TestDBFixture()
        {
            ConnectionString = Configuration.GetConnectionString("DefaultConnection");
            lock (_lock)
            {
                if (!_dbIsCreated)
                {
                    using (var context = CreateContext(ConnectionString))
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
