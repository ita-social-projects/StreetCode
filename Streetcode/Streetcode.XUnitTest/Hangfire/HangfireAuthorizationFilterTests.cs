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
    public static IEnumerable<object[]> NonAdminRoles =>
        new List<object[]>
        {
            new object[] { nameof(UserRole.User) },
            new object[] { string.Empty },
        };

    [Fact]
    public void IsAdmin_UserIsAdmin_ReturnsTrue()
    {
        // Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Role, nameof(UserRole.Admin).ToString()) }));
        var filter = new HangfireDashboardAuthorizationFilter();

        // Act
        bool result = filter.IsAdmin(user);

        // Assert
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(NonAdminRoles))]
    public void IsAdmin_UserIsNotAdmin_ReturnsFalse(string role)
    {
        // Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Role, role) }));
        var filter = new HangfireDashboardAuthorizationFilter();

        // Act
        bool result = filter.IsAdmin(user);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Authorize_UserIsAdmin_ReturnsTrue()
    {
        // Arrange
        var mockDashboardContext = new MockDashboardContext(new Mock<JobStorage>().Object, new Mock<DashboardOptions>().Object);
        var mockFilter = new MockHangfireDashboardAuthorizationFilter(nameof(UserRole.Admin));

        // Act
        bool result = mockFilter.Authorize(mockDashboardContext);

        // Assert
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(NonAdminRoles))]
    public void Authorize_UserIsNotAdmin_ReturnsFalse(string role)
    {
        // Arrange
        var mockDashboardContext = new MockDashboardContext(new Mock<JobStorage>().Object, new Mock<DashboardOptions>().Object);
        var mockFilter = new MockHangfireDashboardAuthorizationFilter(role);

        // Act
        bool result = mockFilter.Authorize(mockDashboardContext);

        // Assert
        Assert.False(result);
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
            return new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Role, this._userRole) }));
        }
    }

    private class MockDashboardContext : DashboardContext
    {
        public MockDashboardContext([NotNull] JobStorage storage, [NotNull] DashboardOptions options)
            : base(storage, options)
        {
        }
    }
}