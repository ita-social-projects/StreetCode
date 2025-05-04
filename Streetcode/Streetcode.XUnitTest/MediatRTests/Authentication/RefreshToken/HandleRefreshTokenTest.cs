﻿using System.IdentityModel.Tokens.Jwt;
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
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly Mock<ILoggerService> _mockLogger;

        /// <summary>
        /// Initializes a new instance of the <see cref="HandleRefreshTokenTest"/> class.
        /// </summary>
        public HandleRefreshTokenTest()
        {
            _mockTokenService = new Mock<ITokenService>();
            _mockLogger = new Mock<ILoggerService>();
        }

        [Fact]
        public async Task ShouldReturnCorrectData_ValidInputToken()
        {
            // Arrange.
            JwtSecurityToken expectedToken = GenerateToken();
            string expectedTokenString = new JwtSecurityTokenHandler().WriteToken(expectedToken);
            _mockTokenService
                .Setup(service => service.RefreshToken(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(expectedToken);
            var handler = GetRefreshTokenHandler();

            // Act.
            var result = await handler.Handle(new RefreshTokenQuery(GetRefreshTokenRequestDto()), CancellationToken.None);

            // Assert.
            Assert.Equal(expectedTokenString, result.Value.AccessToken);
        }

        [Fact]
        public async Task ShouldReturnFail_InvalidInputTokenData()
        {
            // Arrange.
            _mockTokenService
                .Setup(service => service.RefreshToken(It.IsAny<string>(), It.IsAny<string>()))
                .Throws<SecurityTokenValidationException>();
            var handler = GetRefreshTokenHandler();

            // Act.
            var result = await handler.Handle(new RefreshTokenQuery(GetRefreshTokenRequestDto()), CancellationToken.None);

            // Assert.
            Assert.True(result.IsFailed);
        }

        [Fact]
        public async Task ShouldReturnFail_InvalidInputToken()
        {
            // Arrange.
            _mockTokenService
                .Setup(service => service.RefreshToken(It.IsAny<string>(), It.IsAny<string>()))
                .Throws<Exception>();
            var handler = GetRefreshTokenHandler();

            // Act.
            var result = await handler.Handle(new RefreshTokenQuery(GetRefreshTokenRequestDto()), CancellationToken.None);

            // Assert.
            Assert.True(result.IsFailed);
        }

        private static JwtSecurityToken GenerateToken()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Expires = new DateTime(2000, 10, 20, 0, 0, 0, DateTimeKind.Utc),
                NotBefore = new DateTime(2000, 10, 10, 0, 0, 0, DateTimeKind.Utc),
            };

            return tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
        }

        private static RefreshTokenRequestDTO GetRefreshTokenRequestDto()
        {
            return new RefreshTokenRequestDTO();
        }

        private RefreshTokenHandler GetRefreshTokenHandler()
        {
            return new RefreshTokenHandler(
                _mockTokenService.Object,
                _mockLogger.Object);
        }
    }
}
