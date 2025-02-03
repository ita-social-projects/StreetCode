using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Streetcode.BLL.DTO.Authentication.RefreshToken;
using Streetcode.BLL.Interfaces.Authentication;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Authentication.RefreshToken;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Authentication.RefreshToken
{
    public class HandleRefreshTokenTest
    {
        private readonly Mock<ITokenService> mockTokenService;
        private readonly Mock<ILoggerService> mockLogger;

        /// <summary>
        /// Initializes a new instance of the <see cref="HandleRefreshTokenTest"/> class.
        /// </summary>
        public HandleRefreshTokenTest()
        {
            this.mockTokenService = new Mock<ITokenService>();
            this.mockLogger = new Mock<ILoggerService>();
        }

        [Fact]
        public async Task ShouldReturnCorrectData_ValidInputToken()
        {
            // Arrange.
            JwtSecurityToken expectedToken = this.GenerateToken();
            string expectedTokenString = new JwtSecurityTokenHandler().WriteToken(expectedToken);
            this.mockTokenService
                .Setup(service => service.RefreshToken(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(expectedToken);
            var handler = this.GetRefreshTokenHandler();

            // Act.
            var result = await handler.Handle(new RefreshTokenQuery(this.GetRefreshTokenRequestDTO()), CancellationToken.None);

            // Assert.
            Assert.Equal(expectedTokenString, result.Value.AccessToken);
        }

        [Fact]
        public async Task ShouldReturnFail_InvalidInputTokenData()
        {
            // Arrange.
            this.mockTokenService
                .Setup(service => service.RefreshToken(It.IsAny<string>(), It.IsAny<string>()))
                .Throws<SecurityTokenValidationException>();
            var handler = this.GetRefreshTokenHandler();

            // Act.
            var result = await handler.Handle(new RefreshTokenQuery(this.GetRefreshTokenRequestDTO()), CancellationToken.None);

            // Assert.
            Assert.True(result.IsFailed);
        }

        [Fact]
        public async Task ShouldReturnFail_InvalidInputToken()
        {
            // Arrange.
            this.mockTokenService
                .Setup(service => service.RefreshToken(It.IsAny<string>(), It.IsAny<string>()))
                .Throws<Exception>();
            var handler = this.GetRefreshTokenHandler();

            // Act.
            var result = await handler.Handle(new RefreshTokenQuery(this.GetRefreshTokenRequestDTO()), CancellationToken.None);

            // Assert.
            Assert.True(result.IsFailed);
        }

        private RefreshTokenRequestDto GetRefreshTokenRequestDTO()
        {
            return new RefreshTokenRequestDto();
        }

        private RefreshTokenHandler GetRefreshTokenHandler()
        {
            return new RefreshTokenHandler(
                this.mockTokenService.Object,
                this.mockLogger.Object);
        }

        private JwtSecurityToken GenerateToken()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Expires = new DateTime(2000, 10, 20, 0, 0, 0, DateTimeKind.Utc),
                NotBefore = new DateTime(2000, 10, 10, 0, 0, 0, DateTimeKind.Utc),
            };

            return tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
        }
    }
}
