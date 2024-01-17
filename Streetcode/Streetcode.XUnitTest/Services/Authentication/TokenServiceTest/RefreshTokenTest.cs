using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Streetcode.BLL.Services.Authentication;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Persistence;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace Streetcode.XUnitTest.Services.Authentication.TokenServiceTest
{
    public class RefreshTokenTest
    {
        private enum CreateTokenOptions
        {
            ValidToken,
            InvalidToken,
        }

        private const string _JwtAudience = "JWT_Audience";
        private const string _JwtIssuer = "JWT_Issuer";
        private const string _JwtKey = "LKqwleLVcdsl234po14lckd34lkdcdDlakjc";
        private readonly Mock<StreetcodeDbContext> _mockDbContext;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly TokenService _tokenService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshTokenTest"/> class.
        /// </summary>
        public RefreshTokenTest()
        {
            this._mockDbContext = new Mock<StreetcodeDbContext>();
            this._mockConfiguration = new Mock<IConfiguration>();

            this.SetupMockConfiguration();
            this._tokenService = this.GetTokenService();
        }

        [Fact]
        public void ShouldThrowException_InvalidInputToken()
        {
            // Arrange.
            var invalidToken = this.GetToken(CreateTokenOptions.InvalidToken, new User(), string.Empty);
            string invalidTokenStringified = new JwtSecurityTokenHandler().WriteToken(invalidToken);
            var exceptionAction = this._tokenService.RefreshToken;

            // Act.

            // Assert.
            Assert.Throws<SecurityTokenValidationException>(() => exceptionAction(invalidTokenStringified));
        }

        [Fact]
        public void ShouldReturnSuccess_ValidInputToken()
        {
            // Arrange.
            var user = this.GetUser();
            string role = "User";
            var invalidToken = this.GetToken(CreateTokenOptions.ValidToken, user, role);
            string invalidTokenStringified = new JwtSecurityTokenHandler().WriteToken(invalidToken);

            // Act.
            var actualToken = this._tokenService.RefreshToken(invalidTokenStringified);

            // Assert.
            Assert.NotNull(actualToken);
        }

        [Fact]
        public void ShouldTokenWithCorrectData_ValidInputToken()
        {
            // Arrange.
            var expectedUser = this.GetUser();
            string expectedRole = "User";
            var invalidToken = this.GetToken(CreateTokenOptions.ValidToken, expectedUser, expectedRole);
            string invalidTokenStringified = new JwtSecurityTokenHandler().WriteToken(invalidToken);

            // Act.
            var actualToken = this._tokenService.RefreshToken(invalidTokenStringified);

            // Assert.
            Assert.Equal(expectedUser.Email, actualToken.Claims.FirstOrDefault(claim => claim.Type == "email") !.Value);
            Assert.Equal(expectedRole, actualToken.Claims.FirstOrDefault(claim => claim.Type == "role") !.Value);
        }

        private JwtSecurityToken? GetToken(CreateTokenOptions createTokenOptions, User user, string userRoleName)
        {
            if (createTokenOptions == CreateTokenOptions.InvalidToken)
            {
                return new JwtSecurityToken();
            }

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Surname, user.Surname),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, userRoleName),
                }),
                SigningCredentials = this.GetSigningCredentials(),
                Issuer = _JwtIssuer,
                Audience = _JwtAudience,
            };
            var token = new JwtSecurityTokenHandler().CreateJwtSecurityToken(tokenDescriptor);
            return token;
        }

        private User GetUser()
        {
            return new User()
            {
                Id = "1",
                Name = "John",
                Surname = "Doe",
                Email = "JohnDoe@gmail.com",
            };
        }

        private SigningCredentials GetSigningCredentials()
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_JwtKey));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            return signingCredentials;
        }

        private void SetupMockConfiguration()
        {
            this._mockConfiguration
                .SetupGet(configuration => configuration["Jwt:Issuer"])
                .Returns(_JwtIssuer);
            this._mockConfiguration
                .SetupGet(configuration => configuration["Jwt:Audience"])
                .Returns(_JwtAudience);
            this._mockConfiguration
                .SetupGet(configuration => configuration["Jwt:Key"])
                .Returns(_JwtKey);
        }

        private TokenService GetTokenService()
        {
            return new TokenService(
                this._mockConfiguration.Object,
                this._mockDbContext.Object);
        }
    }
}
