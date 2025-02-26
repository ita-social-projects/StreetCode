using System.Linq.Expressions;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.MediatR.Authentication.Logout;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.DAL.Repositories.Interfaces.Users;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Authentication.Logout
{
    public class LogoutHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _repositoryWrapperMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly LogoutHandler _handler;

        public LogoutHandlerTests()
        {
            _repositoryWrapperMock = new Mock<IRepositoryWrapper>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _userRepositoryMock = new Mock<IUserRepository>();

            _repositoryWrapperMock.Setup(r => r.UserRepository).Returns(_userRepositoryMock.Object);
            var fakeEmail = "test@example.com";
            var fakeUserName = "test_username";
            var fakeUserId = Guid.NewGuid().ToString();
            var claims = new List<Claim>
            {
                new (ClaimTypes.NameIdentifier, fakeUserId),
                new (ClaimTypes.Name, fakeUserName),
                new (ClaimTypes.Email, fakeEmail),
            };

            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            var context = new DefaultHttpContext { User = claimsPrincipal };

            _httpContextAccessorMock.Setup(_ => _.HttpContext).Returns(context);
            _handler = new LogoutHandler(
                _repositoryWrapperMock.Object,
                _httpContextAccessorMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnFail_WhenUserNotFound()
        {
            // Arrange
            var request = new LogoutCommand();

            _userRepositoryMock.Setup(repo => repo.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<User, bool>>>(),
                    It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
                .ReturnsAsync((User)null!);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailed);
            Assert.Contains("User not found.", result.Errors[0].Message);
        }

        [Fact]
        public async Task Handle_ShouldReturnOk_WhenLogoutIsSuccessful()
        {
            // Arrange
            var request = new LogoutCommand();
            var user = new User { UserName = "testUser", RefreshToken = "oldToken", RefreshTokenExpiry = DateTime.UtcNow };

            _userRepositoryMock.Setup(repo => repo.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<User, bool>>>(),
                    It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
                .ReturnsAsync(user);

            _repositoryWrapperMock.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task Handle_ShouldReturnFail_WhenDatabaseSaveFails()
        {
            // Arrange
            var request = new LogoutCommand();
            var user = new User { UserName = "testUser", RefreshToken = "oldToken", RefreshTokenExpiry = DateTime.UtcNow };

            _userRepositoryMock.Setup(repo => repo.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<User, bool>>>(),
                    It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
                .ReturnsAsync(user);

            _repositoryWrapperMock.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(0);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailed);
            Assert.Contains("Failed to logout", result.Errors[0].Message);
        }
    }
}