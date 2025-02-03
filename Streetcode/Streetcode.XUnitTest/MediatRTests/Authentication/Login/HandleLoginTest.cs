using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Authentication.Login;
using Streetcode.BLL.DTO.Users;
using Streetcode.BLL.Interfaces.Authentication;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Authentication.Login;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Entities.Users;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Authentication.Login
{
    public class HandleLoginTest
    {
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<ITokenService> mockTokenService;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly Mock<UserManager<User>> mockUserManager;
        private readonly Mock<ICaptchaService> mockCaptchaService;
        private readonly Mock<IStringLocalizer<UserSharedResource>> mockLocalizer;
        /// <summary>
        /// Initializes a new instance of the <see cref="HandleLoginTest"/> class.
        /// </summary>
        public HandleLoginTest()
        {
            this.mockMapper = new Mock<IMapper>();
            this.mockLogger = new Mock<ILoggerService>();
            this.mockTokenService = new Mock<ITokenService>();
            this.mockCaptchaService = new Mock<ICaptchaService>();

            var store = new Mock<IUserStore<User>>();
            this.mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            this.mockLocalizer = new Mock<IStringLocalizer<UserSharedResource>>();
        }

        [Fact]
        public async Task ShouldReturnSuccess_ValidInputData()
        {
            // Arrange.
            this.SetupServicesForSuccessfulResult();
            var handler = this.GetLoginHandler();

            // Act.
            var result = await handler.Handle(new LoginQuery(GetExistingCredentials()), CancellationToken.None);

            // Assert.
            Assert.True(result.IsSuccess);
            Assert.IsType<LoginResponseDto>(result.ValueOrDefault);
        }

        [Fact]
        public async Task ShouldReturnFail_UserNotExistInDb()
        {
            // Arrange.
            this.SetupMockCaptchaService(true);
            var handler = this.GetLoginHandler();

            // Act.
            var result = await handler.Handle(new LoginQuery(GetNonExistingCredentials()), CancellationToken.None);

            // Assert.
            Assert.True(result.IsFailed);
        }

        [Fact]
        public async Task ShouldReturnFail_InvalidPassword()
        {
            // Arrange.
            this.SetupMockCaptchaService(true);
            this.SetupMockUserManagerCheckPassword(false);
            var handler = this.GetLoginHandler();

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
            this.SetupMockCaptchaService(isValidationSuccessful: false, errorMessage);
            var handler = this.GetLoginHandler();

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
            this.SetupMockUserManagerThrowException(expectedErrorMessage);
            this.SetupMockCaptchaService(true);
            var handler = this.GetLoginHandler();

            // Act.
            var result = await handler.Handle(new LoginQuery(GetExistingCredentials()), CancellationToken.None);

            // Assert.
            Assert.True(result.IsFailed);
            Assert.Equal(expectedErrorMessage, result.Errors[0].Message);
        }

        private static UserDto GetUserDTO()
        {
            return new ()
            {
                Email = "one@gmail.com",
                UserName = "One_one",
            };
        }

        private static LoginRequestDto GetNonExistingCredentials()
        {
            return new LoginRequestDto
            {
                Login = "dummyLogin@gmail.com",
                Password = "qwertyQWE123!@#",
            };
        }

        private static LoginRequestDto GetExistingCredentials()
        {
            return new LoginRequestDto
            {
                Login = "one@gmail.com",
                Password = "One111oneOne#@#",
            };
        }

        private void SetupServicesForSuccessfulResult()
        {
            this.SetupMockUserManagerCheckPassword(true);
            this.SetupMockTokenService();
            this.SetupMockMapper();
            this.SetupMockUserManagerFindUserByEmailOrUsername(new User());
            this.SetupMockUserManagerGetRolesAsync();
            this.SetupMockUserManagerGetRolesAsync();
            this.SetupMockCaptchaService(true);
        }

        private void SetupMockUserManagerCheckPassword(bool checkReturn)
        {
            this.mockUserManager
                .Setup(manager => manager
                    .CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(checkReturn);
        }

        private void SetupMockUserManagerThrowException(string errorMessage = "Default error message")
        {
            this.mockUserManager
                .Setup(manager => manager.FindByEmailAsync(It.IsAny<string>()))
                .ThrowsAsync(new Exception(errorMessage));
        }

        private void SetupMockCaptchaService(bool isValidationSuccessful, string errorMessage = "Error")
        {
            var result = isValidationSuccessful ?
                Result.Ok() : Result.Fail(errorMessage);
            this.mockCaptchaService
                .Setup(service => service.ValidateReCaptchaAsync(It.IsAny<string>(), It.IsAny<CancellationToken?>()))
                .ReturnsAsync(result);
        }

        private void SetupMockUserManagerFindUserByEmailOrUsername(User? userToReturn = null)
        {
            this.mockUserManager
                .Setup(manager => manager.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(userToReturn);
        }

        private void SetupMockUserManagerGetRolesAsync()
        {
            this.mockUserManager
                .Setup(manager => manager.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string> { "User" });
        }

        private void SetupMockTokenService()
        {
            this.mockTokenService
                .Setup(service => service
                    .GenerateAccessTokenAsync(It.IsAny<User>()))
                .ReturnsAsync(new JwtSecurityToken());
        }

        private void SetupMockMapper()
        {
            this.mockMapper
                .Setup(x => x
                .Map<UserDto>(It.IsAny<User>()))
                .Returns(GetUserDTO());
        }

        private LoginHandler GetLoginHandler()
        {
            return new LoginHandler(
                this.mockMapper.Object,
                this.mockTokenService.Object,
                this.mockLogger.Object,
                this.mockUserManager.Object,
                this.mockCaptchaService.Object,
                this.mockLocalizer.Object);
        }
    }
}
