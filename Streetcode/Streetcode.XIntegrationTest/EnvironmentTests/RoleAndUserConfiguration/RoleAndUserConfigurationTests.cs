using Microsoft.Extensions.DependencyInjection;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Persistence;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.EnvironmentTests.Utils.BeforeAndAfterTestAtribute.RoleAndUserConfiguration;
using Xunit;
using static Streetcode.WebApi.Utils.Constants.UserDatabaseSeedingConstants;

namespace Streetcode.XIntegrationTest.EnvironmentTests.RoleAndUserConfiguration
{
    public class RoleAndUserConfigurationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> factory;
        private readonly StreetcodeDbContext dbContext;
        private readonly IServiceScope scope;

        public RoleAndUserConfigurationTests(CustomWebApplicationFactory<Program> factory)
        {
            this.factory = factory;
            this.scope = this.factory.Services.CreateScope();
            this.dbContext = this.scope.ServiceProvider.GetRequiredService<StreetcodeDbContext>();
            Environment.SetEnvironmentVariable("ADMIN_PASSWORD", "qweQWE123!@#cccc");
        }

        ~RoleAndUserConfigurationTests()
        {
            Environment.SetEnvironmentVariable("ADMIN_PASSWORD", null);
            this.dbContext.Dispose();
            this.scope.Dispose();
        }

        [Fact]
        [CleanInitialAdminFromDatabase]
        public async Task AddUsersAndRoles_AddInitialAdminToDb()
        {
            // Arrange.

            // Act.
            await WebApi.Configuration.RoleAndUserConfiguration.AddUsersAndRoles(this.scope.ServiceProvider);
            User? initialAdmin = this.dbContext.Users.FirstOrDefault(user => user.Email == AdminEmail);

            // Assert.
            Assert.NotNull(initialAdmin);
            Assert.Equal(AdminUsername, initialAdmin.UserName);
        }
    }
}
