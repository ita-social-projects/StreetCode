using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Authentication.Login;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Interfaces.Authentication;
using Streetcode.BLL.MediatR.Authentication.Login;
using Streetcode.DAL.Entities.Users;
using Xunit;
using System.IdentityModel.Tokens.Jwt;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.BLL.DTO.Users;

namespace Streetcode.XUnitTest.MediatRTests.Authentication.Login
{
    public class HandleLoginTest
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<UserManager<User>> _mockUserManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="HandleLoginTest"/> class.
        /// </summary>
        public HandleLoginTest()
        {
            this._mockMapper = new Mock<IMapper>();
            this._mockLogger = new Mock<ILoggerService>();
            this._mockTokenService = new Mock<ITokenService>();

            var store = new Mock<IUserStore<User>>();
            this._mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        }

        [Fact]
        public async Task ShouldReturnSuccess_ValidInputData()
        {
            // Arrange.
            this.SetupMockUserManagerCheckPassword(true);
            this.SetupMockTokenService();
            this.SetupMockMapper();
            var handler = this.GetLoginHandler();

            // Act.
            var result = await handler.Handle(new LoginQuery(GetExistingCredentials()), CancellationToken.None);

            // Assert.
            Assert.Multiple(
                () => Assert.True(result.IsSuccess),
                () => Assert.IsType<LoginResponseDTO>(result.ValueOrDefault));
        }

        [Fact]
        public async Task ShouldReturnFail_UserNotExistInDb()
        {
            // Arrange.
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
            this.SetupMockUserManagerCheckPassword(false);
            var handler = this.GetLoginHandler();

            // Act.
            var result = await handler.Handle(new LoginQuery(GetExistingCredentials()), CancellationToken.None);

            // Assert.
            Assert.True(result.IsFailed);
        }

        private static IEnumerable<User> GetUserCollection()
        {
            return new List<User>()
            {
                new User()
                {
                    Id = "1",
                    Email = "one@gmail.com",
                    UserName = "One_one",
                },
            };
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

        private void SetupMockUserManagerCheckPassword(bool checkReturn)
        {
            this._mockUserManager
                .Setup(manager => manager
                    .CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(checkReturn);
        }

        private void SetupMockTokenService()
        {
            this._mockTokenService
                .Setup(service => service
                    .GenerateAccessTokenAsync(It.IsAny<User>()))
                .ReturnsAsync(new JwtSecurityToken());
        }

        private void SetupMockMapper()
        {
            this._mockMapper
                .Setup(x => x
                .Map<UserDTO>(It.IsAny<User>()))
                .Returns(GetUserDTO());
        }

        private LoginHandler GetLoginHandler()
        {
            return new LoginHandler(
                this._mockMapper.Object,
                this._mockTokenService.Object,
                this._mockLogger.Object,
                this._mockUserManager.Object);
        }
    }
}
