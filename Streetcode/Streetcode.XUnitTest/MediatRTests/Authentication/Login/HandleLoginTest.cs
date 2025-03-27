using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Authentication.Login;
using Streetcode.BLL.DTO.Users;
using Streetcode.BLL.Factories.MessageDataFactory.Abstracts;
using Streetcode.BLL.Interfaces.Authentication;
using Streetcode.BLL.Interfaces.Email;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Authentication.Login;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Users;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Authentication.Login
{
    public class HandleLoginTest
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly Mock<ICaptchaService> _mockCaptchaService;
        private readonly Mock<IStringLocalizer<UserSharedResource>> _mockLocalizer;
        private readonly Mock<IEmailService> _emailService;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor;
        private readonly Mock<IMessageDataAbstractFactory> _messageDataAbstractFactory;

        public HandleLoginTest()
        {
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerService>();
            _mockTokenService = new Mock<ITokenService>();
            _mockCaptchaService = new Mock<ICaptchaService>();
            _emailService = new Mock<IEmailService>();
            _httpContextAccessor = new Mock<IHttpContextAccessor>();
            _messageDataAbstractFactory = new Mock<IMessageDataAbstractFactory>();

            var store = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            _mockLocalizer = new Mock<IStringLocalizer<UserSharedResource>>();
        }

        [Fact]
        public async Task ShouldReturnSuccess_ValidInputData()
        {
            // Arrange.
            SetupServicesForSuccessfulResult();
            SetupMockUserManagerIsEmailConfirmed(true);
            var handler = GetLoginHandler();

            // Act.
            var result = await handler.Handle(new LoginQuery(GetExistingCredentials()), CancellationToken.None);

            // Assert.
            Assert.True(result.IsSuccess);
            Assert.IsType<LoginResponseDTO>(result.ValueOrDefault);
        }

        [Fact]
        public async Task ShouldReturnFail_UserNotExistInDb()
        {
            // Arrange.
            SetupMockCaptchaService(true);
            var handler = GetLoginHandler();

            // Act.
            var result = await handler.Handle(new LoginQuery(GetNonExistingCredentials()), CancellationToken.None);

            // Assert.
            Assert.True(result.IsFailed);
        }

        [Fact]
        public async Task ShouldReturnFail_InvalidPassword()
        {
            // Arrange.
            SetupMockCaptchaService(true);
            SetupMockUserManagerCheckPassword(false);
            var handler = GetLoginHandler();

            // Act.
            var result = await handler.Handle(new LoginQuery(GetExistingCredentials()), CancellationToken.None);

            // Assert.
            Assert.True(result.IsFailed);
        }

        [Fact]
        public async Task ShouldReturnFail_CaptchaValidationUnsuccessful()
        {
            // Arrange.
            string errorMessage = "Captcha validation error";
            SetupMockCaptchaService(isValidationSuccessful: false, errorMessage);
            var handler = GetLoginHandler();

            // Act.
            var result = await handler.Handle(new LoginQuery(GetExistingCredentials()), CancellationToken.None);

            // Assert.
            Assert.True(result.IsFailed);
            Assert.Equal(errorMessage, result.Errors[0].Message);
        }

        [Fact]
        public async Task ShouldReturnFail_ExceptionThrown()
        {
            // Arrange.
            string expectedErrorMessage = "Expected error message";
            SetupMockUserManagerThrowException(expectedErrorMessage);
            SetupMockCaptchaService(true);
            var handler = GetLoginHandler();

            // Act.
            var result = await handler.Handle(new LoginQuery(GetExistingCredentials()), CancellationToken.None);

            // Assert.
            Assert.True(result.IsFailed);
            Assert.Equal(expectedErrorMessage, result.Errors[0].Message);
        }

        private static UserDTO GetUserDTO()
        {
            return new ()
            {
                Email = "one@gmail.com",
                UserName = "One_one",
            };
        }

        private static LoginRequestDTO GetNonExistingCredentials()
        {
            return new LoginRequestDTO
            {
                Login = "dummyLogin@gmail.com",
                Password = "qwertyQWE123!@#",
            };
        }

        private static LoginRequestDTO GetExistingCredentials()
        {
            return new LoginRequestDTO
            {
                Login = "one@gmail.com",
                Password = "One111oneOne#@#",
            };
        }

        private void SetupServicesForSuccessfulResult()
        {
            SetupMockUserManagerCheckPassword(true);
            SetupMockTokenService();
            SetupMockMapper();
            SetupMockUserManagerFindUserByEmailOrUsername(new User());
            SetupMockUserManagerGetRolesAsync();
            SetupMockUserManagerGetRolesAsync();
            SetupMockCaptchaService(true);
        }

        private void SetupMockUserManagerCheckPassword(bool checkReturn)
        {
            _mockUserManager
                .Setup(manager => manager
                    .CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(checkReturn);
        }

        private void SetupMockUserManagerThrowException(string errorMessage = "Default error message")
        {
            _mockUserManager
                .Setup(manager => manager.FindByEmailAsync(It.IsAny<string>()))
                .ThrowsAsync(new Exception(errorMessage));
        }

        private void SetupMockCaptchaService(bool isValidationSuccessful, string errorMessage = "Error")
        {
            var result = isValidationSuccessful ?
                Result.Ok() : Result.Fail(errorMessage);
            _mockCaptchaService
                .Setup(service => service.ValidateReCaptchaAsync(It.IsAny<string>(), It.IsAny<CancellationToken?>()))
                .ReturnsAsync(result);
        }

        private void SetupMockUserManagerFindUserByEmailOrUsername(User? userToReturn = null)
        {
            _mockUserManager
                .Setup(manager => manager.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(userToReturn);
        }

        private void SetupMockUserManagerIsEmailConfirmed(bool toReturn)
        {
            _mockUserManager
                .Setup(manager => manager.IsEmailConfirmedAsync(It.IsAny<User>()))
                .ReturnsAsync(toReturn);
        }

        private void SetupMockUserManagerGetRolesAsync()
        {
            _mockUserManager
                .Setup(manager => manager.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string> { "User" });
        }

        private void SetupMockTokenService()
        {
            _mockTokenService
                .Setup(service => service
                    .GenerateAccessTokenAsync(It.IsAny<User>()))
                .ReturnsAsync(new JwtSecurityToken());
        }

        private void SetupMockMapper()
        {
            _mockMapper
                .Setup(x => x
                .Map<UserDTO>(It.IsAny<User>()))
                .Returns(GetUserDTO());
        }

        private LoginHandler GetLoginHandler()
        {
            return new LoginHandler(
                _mockMapper.Object,
                _mockTokenService.Object,
                _mockLogger.Object,
                _mockUserManager.Object,
                _mockCaptchaService.Object,
                _mockLocalizer.Object,
                _emailService.Object,
                _messageDataAbstractFactory.Object,
                _httpContextAccessor.Object);
        }
    }
}
