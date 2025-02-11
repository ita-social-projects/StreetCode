using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Users.Password;
using Streetcode.BLL.Factories.MessageDataFactory.Abstracts;
using Streetcode.BLL.Interfaces.Email;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Email;
using Streetcode.BLL.MediatR.Users.ForgotPassword;
using Streetcode.BLL.Models.Email.Messages.Base;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Users;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Users
{
    public class ForgotPasswordTests
    {
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly Mock<IStringLocalizer<UserSharedResource>> _localizerUserSharedResourceMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<IMessageDataAbstractFactory> _messageFactoryMock;
        private readonly Mock<ILoggerService> _loggerMock;
        private readonly Mock<IStringLocalizer<SendEmailHandler>> _emailLocalizerMock;

        private readonly ForgotPasswordHandler _handler;

        public ForgotPasswordTests()
        {
            _userManagerMock = new Mock<UserManager<User>>(
                Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);

            _emailServiceMock = new Mock<IEmailService>();
            _localizerUserSharedResourceMock = new Mock<IStringLocalizer<UserSharedResource>>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _messageFactoryMock = new Mock<IMessageDataAbstractFactory>();
            _loggerMock = new Mock<ILoggerService>();
            _emailLocalizerMock = new Mock<IStringLocalizer<SendEmailHandler>>();

            _handler = new ForgotPasswordHandler(
                _loggerMock.Object,
                _userManagerMock.Object,
                _emailServiceMock.Object,
                _localizerUserSharedResourceMock.Object,
                _httpContextAccessorMock.Object,
                _messageFactoryMock.Object,
                _emailLocalizerMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnFail_WhenUserDoesNotExist()
        {
            // Arrange
            var request = new ForgotPasswordCommand(new ForgotPasswordDTO { Email = "nonexistent@example.com" });

            _userManagerMock.Setup(um => um.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((User)null!);

            _localizerUserSharedResourceMock.Setup(l => l["UserWithSuchEmailNotFound"])
                .Returns(new LocalizedString("UserWithSuchEmailNotFound", "User not found"));

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailed);
            Assert.Contains("User not found", result.Errors[0].Message);
            _loggerMock.Verify(l => l.LogError(request, "User not found"), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenEmailSentSuccessfully()
        {
            // Arrange
            var user = new User { UserName = "TestUser", Email = "test@example.com" };
            var request = new ForgotPasswordCommand(new ForgotPasswordDTO { Email = "test@example.com" });

            SetupUserManagerMock(user);
            SetupMessageFactoryMock();
            SetupEmailServiceMock(true);

            _httpContextAccessorMock.Setup(h => h.HttpContext).Returns(new DefaultHttpContext());

            _userManagerMock.Setup(um => um.FindByEmailAsync(request.ForgotPasswordDto.Email))
                .ReturnsAsync(user);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(Unit.Value, result.Value);
        }

        [Fact]
        public async Task Handle_ShouldReturnFail_WhenEmailSendingFails()
        {
            // Arrange
            var user = new User { UserName = "TestUser", Email = "test@example.com" };
            var request = new ForgotPasswordCommand(new ForgotPasswordDTO { Email = "test@example.com" });

            SetupUserManagerMock(user);
            SetupMessageFactoryMock();
            SetupEmailServiceMock(false);

            _emailLocalizerMock.Setup(l => l["FailedToSendEmailMessage"])
                .Returns(new LocalizedString("FailedToSendEmailMessage", "Failed to send email"));

            _userManagerMock.Setup(um => um.FindByEmailAsync(request.ForgotPasswordDto.Email))
                .ReturnsAsync(user);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailed);
            Assert.Contains("Failed to send email", result.Errors[0].Message);
            _loggerMock.Verify(l => l.LogError(request, "Failed to send email"), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFail_WhenExceptionIsThrown()
        {
            // Arrange
            var request = new ForgotPasswordCommand(new ForgotPasswordDTO { Email = "test@example.com" });

            _userManagerMock.Setup(um => um.FindByEmailAsync(It.IsAny<string>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailed);
            Assert.Contains("Database error", result.Errors[0].Message);
            _loggerMock.Verify(l => l.LogError(request, "Database error"), Times.Once);
        }

        private void SetupMessageFactoryMock()
        {
            _messageFactoryMock.Setup(mf => mf.CreateForgotPasswordMessageData(
                    It.IsAny<string[]>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(new Mock<MessageData>().Object);
        }

        private void SetupUserManagerMock(User user)
        {
            _userManagerMock.Setup(um => um.GeneratePasswordResetTokenAsync(user))
                .ReturnsAsync("mocked-token");
        }

        private void SetupEmailServiceMock(bool isSuccess)
        {
            _emailServiceMock.Setup(es => es.SendEmailAsync(It.IsAny<MessageData>()))
                .ReturnsAsync(isSuccess);
        }
    }
}