using Microsoft.Extensions.DependencyInjection;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Persistence;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.EnvironmentTests.Utils.BeforeAndAfterTestAtribute.RoleAndUserConfiguration;
using System.Net;
using Xunit;
using static Streetcode.WebApi.Utils.Constants.UserDatabaseSeedingConstants;

namespace Streetcode.XIntegrationTest.EnvironmentTests.RoleAndUserConfiguration
{
    public class RoleAndUserConfigurationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;
        private readonly StreetcodeDbContext _dbContext;
        private readonly IServiceScope _scope;

        public RoleAndUserConfigurationTests(CustomWebApplicationFactory<Program> factory)
        {
            this._factory = factory;
            this._scope = this._factory.Services.CreateScope();
            this._dbContext = this._scope.ServiceProvider.GetRequiredService<StreetcodeDbContext>();
            Environment.SetEnvironmentVariable("ADMIN_PASSWORD", "qweQWE123!@#cccc");
        }

        ~RoleAndUserConfigurationTests()
        {
            Environment.SetEnvironmentVariable("ADMIN_PASSWORD", null);
            this._dbContext.Dispose();
            this._scope.Dispose();
        }

        [Fact]
        [CleanInitialAdminFromDatabase]
        public async Task AddUsersAndRoles_AddInitialAdminToDb()
        {
            // Arrange.

            // Act.
            await WebApi.Configuration.RoleAndUserConfiguration.AddUsersAndRoles(this._scope.ServiceProvider);
            User? initialAdmin = this._dbContext.Users.FirstOrDefault(user => user.Email == AdminEmail);

            // Assert.
            Assert.NotNull(initialAdmin);
            Assert.Equal(AdminUsername, initialAdmin.UserName);
        }
    }
}
