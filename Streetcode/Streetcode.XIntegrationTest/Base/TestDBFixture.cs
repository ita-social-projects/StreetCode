using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Streetcode.DAL.Persistence;

namespace Streetcode.XIntegrationTest.Base
{
    public class TestDBFixture : IntegrationTestBase
    {
        private static readonly object Lock = new ();
        private static bool _dbIsCreated;
        private readonly string _connectionString;

        public TestDBFixture()
        {
            this._connectionString = this.Configuration.GetConnectionString("DefaultConnection") !;
            lock (Lock)
            {
                if (!_dbIsCreated)
                {
                    using (var context = CreateContext(this._connectionString))
                    {
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();
                    }

                    _dbIsCreated = true;
                }
            }
        }

        private static StreetcodeDbContext CreateContext(string connectionString)
            => new StreetcodeDbContext(
                new DbContextOptionsBuilder<StreetcodeDbContext>()
                    .UseSqlServer(connectionString)
                    .Options);
    }
}
