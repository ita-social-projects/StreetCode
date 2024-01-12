using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Streetcode.BLL.DTO.Authentication.RefreshToken;
using Streetcode.BLL.Interfaces.Users;
using Streetcode.BLL.MediatR.Authentication.Login;
using Streetcode.BLL.MediatR.Authentication.RefreshToken;
using Streetcode.BLL.MediatR.Authentication.Register;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Authentication.RefreshToken
{
    public class HandleRefreshTokenTest
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
        private readonly Mock<ITokenService> _mockTokenService;

        /// <summary>
        /// Initializes a new instance of the <see cref="HandleRefreshTokenTest"/> class.
        /// </summary>
        public HandleRefreshTokenTest()
        {
            this._mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            this._mockMapper = new Mock<IMapper>();
            this._mockTokenService = new Mock<ITokenService>();
        }

        [Fact]
        public async Task ShouldCorrectData_ValidInputData()
        {
            // Arrange.
            JwtSecurityToken expectedToken = this.GenerateToken();
            string expectedTokenString = new JwtSecurityTokenHandler().WriteToken(expectedToken);
            this._mockTokenService
                .Setup(service => service.RefreshToken(It.IsAny<string>()))
                .Returns(expectedToken);
            var handler = this.GetRefreshTokenHandler();

            // Act.
            var result = await handler.Handle(new RefreshTokenQuery(this.GetRefreshTokenRequestDTO()), CancellationToken.None);

            // Assert.
            Assert.Equal(expectedTokenString, result.Value.Token);
        }

        private RefreshTokenRequestDTO GetRefreshTokenRequestDTO()
        {
            return new RefreshTokenRequestDTO();
        }

        private RefreshTokenHandler GetRefreshTokenHandler()
        {
            return new RefreshTokenHandler(this._mockTokenService.Object);
        }

        private JwtSecurityToken GenerateToken()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Expires = new DateTime(2000, 10, 20),
                NotBefore = new DateTime(2000, 10, 10),
            };

            return tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
        }
    }
}
