using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Users.Password;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Users.UpdateForgotPassword;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Users;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Users;

public class UpdateForgotPasswordHandlerTests
{
    private readonly Mock<ILoggerService> _loggerMock;
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<IStringLocalizer<UserSharedResource>> _localizerMock;
    private readonly UpdateForgotPasswordHandler _handler;

    public UpdateForgotPasswordHandlerTests()
    {
        _loggerMock = new Mock<ILoggerService>();
        _localizerMock = new Mock<IStringLocalizer<UserSharedResource>>();

        var userStoreMock = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(
            userStoreMock.Object, null, null, null, null, null, null, null, null);

        _handler = new UpdateForgotPasswordHandler(
            _loggerMock.Object,
            _userManagerMock.Object,
            _localizerMock.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsSuccess()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var request = new UpdateForgotPasswordCommand(new UpdateForgotPasswordDTO
        {
            Username = Uri.EscapeDataString("testUser"),
            Token = Uri.EscapeDataString("validToken"),
            Password = "NewSecurePassword123!",
        });

        var user = new User { UserName = "testUser", Id = "123" };

        _userManagerMock.Setup(um => um.FindByNameAsync("testUser")).ReturnsAsync(user);
        _userManagerMock.Setup(um => um.ResetPasswordAsync(user, "validToken", request.UpdateForgotPasswordDto.Password))
            .ReturnsAsync(IdentityResult.Success);

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
        var request = new UpdateForgotPasswordCommand(new UpdateForgotPasswordDTO
        {
            Username = Uri.EscapeDataString("nonExistentUser"),
            Token = Uri.EscapeDataString("validToken"),
            Password = "NewSecurePassword123!",
        });

        _userManagerMock.Setup(um => um.FindByNameAsync("nonExistentUser")).ReturnsAsync((User)null!);

        _localizerMock.Setup(l => l["UserWithSuchUsernameNotExists"])
            .Returns(new LocalizedString("UserWithSuchUsernameNotExists", "User does not exist"));

        // Act
        var result = await _handler.Handle(request, cancellationToken);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_InvalidPasswordReset_ReturnsErrors()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var request = new UpdateForgotPasswordCommand(new UpdateForgotPasswordDTO
        {
            Username = Uri.EscapeDataString("testUser"),
            Token = Uri.EscapeDataString("validToken"),
            Password = "weak",
        });

        var user = new User { UserName = "testUser", Id = "123" };

        _userManagerMock.Setup(um => um.FindByNameAsync("testUser")).ReturnsAsync(user);
        _userManagerMock.Setup(um => um.ResetPasswordAsync(user, "validToken", request.UpdateForgotPasswordDto.Password))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Password too weak" }));

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
        var request = new UpdateForgotPasswordCommand(new UpdateForgotPasswordDTO
        {
            Username = Uri.EscapeDataString("testUser"),
            Token = Uri.EscapeDataString("validToken"),
            Password = "NewSecurePassword123!",
        });

        _userManagerMock.Setup(um => um.FindByNameAsync("testUser")).ThrowsAsync(new Exception("Database connection failed"));

        // Act
        var result = await _handler.Handle(request, cancellationToken);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task Handle_UserNameOrTokenIsNull_ReturnsFailure()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var request = new UpdateForgotPasswordCommand(new UpdateForgotPasswordDTO
        {
            Username = string.Empty,
            Token = string.Empty,
            Password = "NewSecurePassword123!",
        });

        // Act
        var result = await _handler.Handle(request, cancellationToken);

        // Assert
        Assert.False(result.IsSuccess);
    }
}