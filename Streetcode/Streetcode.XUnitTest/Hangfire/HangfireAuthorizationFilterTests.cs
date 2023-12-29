using System.Security.Claims;
using Hangfire;
using Hangfire.Annotations;
using Hangfire.Dashboard;
using Moq;
using Streetcode.BLL.Services.Hangfire;
using Streetcode.DAL.Enums;
using Xunit;

namespace Streetcode.XUnitTest.Hangfire;

public class HangfireAuthorizationFilterTests
{
    public static IEnumerable<object[]> NonMainAdministratorRoles =>
        new List<object[]>
        {
            new object[] { UserRole.Administrator.ToString() },
            new object[] { UserRole.Moderator.ToString() },
            new object[] { string.Empty },
        };

    private class MockDashboardContext : DashboardContext
    {
        public MockDashboardContext([NotNull] JobStorage storage, [NotNull] DashboardOptions options)
            : base(storage, options)
        {
        }
    }

    public class MockHangfireDashboardAuthorizationFilter : HangfireDashboardAuthorizationFilter
    {
        private readonly string _userRole;

        public MockHangfireDashboardAuthorizationFilter(string userRole)
        {
            this._userRole = userRole;
        }

        public override ClaimsPrincipal GetUser(DashboardContext context)
        {
            return new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Role, _userRole) }));
        }
    }

    [Fact]
    public void IsMainAdministrator_UserIsMainAdministrator_ReturnsTrue()
    {
        // Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Role, UserRole.MainAdministrator.ToString()) }));
        var filter = new HangfireDashboardAuthorizationFilter();

        // Act
        bool result = filter.IsMainAdministrator(user);

        // Assert
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(NonMainAdministratorRoles))]
    public void IsMainAdministrator_UserIsNotMainAdministrator_ReturnsFalse(string role)
    {
        // Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Role, role) }));
        var filter = new HangfireDashboardAuthorizationFilter();

        // Act
        bool result = filter.IsMainAdministrator(user);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Authorize_UserIsMainAdministrator_ReturnsTrue()
    {
        // Arrange
        var mockDashboardContext = new MockDashboardContext(new Mock<JobStorage>().Object, new Mock<DashboardOptions>().Object);
        var mockFilter = new MockHangfireDashboardAuthorizationFilter(UserRole.MainAdministrator.ToString());

        // Act
        bool result = mockFilter.Authorize(mockDashboardContext);

        // Assert
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(NonMainAdministratorRoles))]
    public void Authorize_UserIsNotMainAdministrator_ReturnsFalse(string role)
    {
        // Arrange
        var mockDashboardContext = new MockDashboardContext(new Mock<JobStorage>().Object, new Mock<DashboardOptions>().Object);
        var mockFilter = new MockHangfireDashboardAuthorizationFilter(role);

        // Act
        bool result = mockFilter.Authorize(mockDashboardContext);

        // Assert
        Assert.False(result);
    }
}