using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Users.Delete;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Users;

public class DeleteUserHandlerTests
{
    private readonly Mock<IRepositoryWrapper> _repositoryWrapperMock;
    private readonly Mock<ILoggerService> _loggerMock;
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly Mock<IStringLocalizer<UserSharedResource>> _localizerMock;
    private readonly DeleteUserHandler _handler;

    public DeleteUserHandlerTests()
    {
        _repositoryWrapperMock = new Mock<IRepositoryWrapper>();
        _loggerMock = new Mock<ILoggerService>();
        _localizerMock = new Mock<IStringLocalizer<UserSharedResource>>();
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        var userStoreMock = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(
            userStoreMock.Object, null, null, null, null, null, null, null, null);

        var claims = new List<Claim>
        {
            new (ClaimTypes.Email, "user@example.com"),
        };

        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var claimsPrincipal = new ClaimsPrincipal(identity);
        var context = new DefaultHttpContext { User = claimsPrincipal };

        _httpContextAccessorMock.Setup(_ => _.HttpContext).Returns(context);

        _handler = new DeleteUserHandler(
            _repositoryWrapperMock.Object,
            _loggerMock.Object,
            _userManagerMock.Object,
            _httpContextAccessorMock.Object,
            _localizerMock.Object);
    }

    [Fact]
    public async Task Handle_ValidUserDeletion_ReturnsSuccess()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var testEmail = "user@example.com";
        var user = new User { Email = testEmail };

        var request = new DeleteUserCommand(testEmail);

        _userManagerMock.Setup(um => um.FindByEmailAsync(testEmail))
            .ReturnsAsync(user);

        _repositoryWrapperMock.Setup(r => r.UserRepository.Delete(user));
        _repositoryWrapperMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(request, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_UserNotFound_ReturnsFailure()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var request = new DeleteUserCommand("user@example.com");

        _userManagerMock.Setup(um => um.FindByEmailAsync("user@example.com"))
            .ReturnsAsync((User)null!);

        // Act
        var result = await _handler.Handle(request, cancellationToken);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_EmailMismatch_ReturnsFailure()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var request = new DeleteUserCommand("different@example.com");

        var user = new User { Email = "user@example.com" };

        _userManagerMock.Setup(um => um.FindByEmailAsync("user@example.com"))
            .ReturnsAsync(user);

        // Act
        var result = await _handler.Handle(request, cancellationToken);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_ExceptionThrown_ReturnsFailure()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var request = new DeleteUserCommand("user@example.com");

        _userManagerMock.Setup(um => um.FindByEmailAsync("user@example.com"))
            .ThrowsAsync(new Exception("Database connection failed"));

        // Act
        var result = await _handler.Handle(request, cancellationToken);

        // Assert
        Assert.False(result.IsSuccess);
    }
}