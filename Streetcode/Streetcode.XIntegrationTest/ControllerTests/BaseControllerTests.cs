﻿namespace Streetcode.XIntegrationTest.ControllerTests
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Streetcode.DAL.Persistence;
    using Streetcode.XIntegrationTest.ControllerTests.Utils;
    using Xunit;

    public class BaseControllerTests : IntegrationTestBase, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        protected StreetcodeClient client;

        public BaseControllerTests(CustomWebApplicationFactory<Program> factory, string secondPartUrl = "")
        {
            this.client = new StreetcodeClient(factory.CreateClient(), secondPartUrl);
        }

        public static SqlDbHelper GetSqlDbHelper()
        {
            var sqlConnectionString = new ConfigurationBuilder()
                .AddJsonFile("appsettings.IntegrationTests.json")
                .Build().GetConnectionString("DefaultConnection");
            var optionBuilder = new DbContextOptionsBuilder<StreetcodeDbContext>();
            optionBuilder.UseSqlServer(sqlConnectionString);
            return new SqlDbHelper(optionBuilder.Options);
        }
    }
}
