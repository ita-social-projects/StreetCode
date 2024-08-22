using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Streetcode.BLL.Services.Authentication;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Persistence;
using Xunit;

namespace Streetcode.XUnitTest.Services.Authentication.TokenServiceTest
{
    public class RefreshTokenTest
    {
        private readonly string jwtKey = "s_dkcLEWRlcksdmcQWE_124";
        private readonly string jwtIssuer = "Jwt_Issuer";
        private readonly string jwtAudience = "Jwt_Audience";
        private readonly string accessTokenLifetimeInMinutes = "15";
        private readonly Mock<StreetcodeDbContext> mockDbContext;
        private readonly IConfiguration fakeConfiguration;
        private readonly TokenService tokenService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshTokenTest"/> class.
        /// </summary>
        public RefreshTokenTest()
        {
            this.mockDbContext = new Mock<StreetcodeDbContext>();
            this.fakeConfiguration = this.GetFakeConfiguration();

            this.tokenService = this.GetTokenService();
        }

        private enum CreateTokenOptions
        {
            ValidToken,
            InvalidToken,
        }

        [Fact]
        public void ShouldThrowException_InvalidInputAccessTokenSignature()
        {
            // Arrange.
            string testRefreshToken = "TestRefreshToken";
            var invalidToken = this.GetToken(CreateTokenOptions.InvalidToken, new User(), string.Empty);
            string invalidTokenStringified = new JwtSecurityTokenHandler().WriteToken(invalidToken);
            var exceptionAction = this.tokenService.RefreshToken;

            // Act.

            // Assert.
            Assert.Throws<SecurityTokenValidationException>(() => exceptionAction(invalidTokenStringified, testRefreshToken));
        }

        [Fact]
        public void ShouldThrowException_UserFromInputAccessTokenNotExists()
        {
            // Arrange.
            string testRefreshToken = "TestRefreshToken";
            var validToken = this.GetToken(CreateTokenOptions.ValidToken, this.GetUser(), string.Empty);
            string validTokenStringified = new JwtSecurityTokenHandler().WriteToken(validToken);
            this.SetupMockDbContext(null);
            var exceptionAction = this.tokenService.RefreshToken;

            // Act.

            // Assert.
            Assert.Throws<NullReferenceException>(() => exceptionAction(validTokenStringified, testRefreshToken));
        }

        [Fact]
        public void ShouldThrowException_InvalidInputRefreshToken()
        {
            // Arrange.
            string testRefreshToken = "TestRefreshToken";
            User testUser = this.GetUser();
            var invalidToken = this.GetToken(CreateTokenOptions.ValidToken, this.GetUser(), string.Empty);
            string invalidTokenStringified = new JwtSecurityTokenHandler().WriteToken(invalidToken);
            this.SetupMockDbContext(testUser);
            var exceptionAction = this.tokenService.RefreshToken;

            // Act.

            // Assert.
            var exception = Assert.Throws<Exception>(() => exceptionAction(invalidTokenStringified, testRefreshToken));
            Assert.Equal(TokenService.InvalidRefreshTokenErrorMessage, exception.Message);
        }

        [Fact]
        public void ShouldThrowException_RefreshExpiredToken()
        {
            // Arrange.
            string testRefreshToken = "TestRefreshToken";
            User testUser = this.GetUser(testRefreshToken);
            var invalidToken = this.GetToken(CreateTokenOptions.ValidToken, this.GetUser(), string.Empty);
            string invalidTokenStringified = new JwtSecurityTokenHandler().WriteToken(invalidToken);
            this.SetupMockDbContext(testUser);
            var exceptionAction = this.tokenService.RefreshToken;

            // Act.

            // Assert.
            var exception = Assert.Throws<Exception>(() => exceptionAction(invalidTokenStringified, testRefreshToken));
            Assert.Equal(TokenService.InvalidRefreshTokenErrorMessage, exception.Message);
        }

        [Fact]
        public void ShouldReturnSuccess_ValidInputToken()
        {
            // Arrange.
            string testRefreshToken = "TestRefreshToken";
            var user = this.GetUser(testRefreshToken, DateTime.Now.AddDays(1));
            string role = "User";
            var invalidToken = this.GetToken(CreateTokenOptions.ValidToken, user, role);
            string invalidTokenStringified = new JwtSecurityTokenHandler().WriteToken(invalidToken);
            this.SetupMockDbContext(user);

            // Act.
            var actualToken = this.tokenService.RefreshToken(invalidTokenStringified, testRefreshToken);

            // Assert.
            Assert.NotNull(actualToken);
        }

        [Fact]
        public void ShouldTokenWithCorrectData_ValidInputToken()
        {
            // Arrange.
            string testRefreshToken = "TestRefreshToken";
            User expectedUser = this.GetUser(testRefreshToken, DateTime.Now.AddDays(1));
            string expectedRole = "User";
            var invalidToken = this.GetToken(CreateTokenOptions.ValidToken, expectedUser, expectedRole);
            string invalidTokenStringified = new JwtSecurityTokenHandler().WriteToken(invalidToken);
            this.SetupMockDbContext(expectedUser);

            // Act.
            var actualToken = this.tokenService.RefreshToken(invalidTokenStringified, testRefreshToken);

            // Assert.
            Assert.Equal(expectedUser.Email, actualToken.Claims.First(claim => claim.Type == "email").Value);
            Assert.Equal(expectedRole, actualToken.Claims.First(claim => claim.Type == "role").Value);
        }

        private void SetupMockDbContext(User? userToReturn)
        {
            var mockDbSet = this.GetConfiguredMockDbSet<User>(new List<User>()
            {
                userToReturn ?? new User(),
            });
            this.mockDbContext
                .Setup(context => context.Users)
                .Returns(mockDbSet.Object);
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
                    new Claim(ClaimTypes.Email, user.Email!),
                    new Claim(ClaimTypes.Role, userRoleName),
                }),
                SigningCredentials = this.GetSigningCredentials(),
                Issuer = this.jwtIssuer,
                Audience = this.jwtAudience,
            };
            var token = new JwtSecurityTokenHandler().CreateJwtSecurityToken(tokenDescriptor);
            return token;
        }

        private User GetUser(string refreshToken = "", DateTime? refreshTokenExpireTime = null)
        {
            return new User()
            {
                Id = "1",
                Name = "John",
                Surname = "Doe",
                Email = "JohnDoe@gmail.com",
                RefreshToken = refreshToken,
                RefreshTokenExpiry = refreshTokenExpireTime ?? DateTime.MinValue,
            };
        }

        private SigningCredentials GetSigningCredentials()
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.jwtKey));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            return signingCredentials;
        }

        private IConfiguration GetFakeConfiguration()
        {
            var appSettingsStub = new Dictionary<string, string?>
            {
                { "Jwt:Key", this.jwtKey },
                { "Jwt:Issuer", this.jwtIssuer },
                { "Jwt:Audience", this.jwtAudience },
                { "Jwt:AccessTokenLifetimeInMinutes", this.accessTokenLifetimeInMinutes },
            };
            var fakeConfiguration = new ConfigurationBuilder()
                .AddInMemoryCollection(appSettingsStub)
                .Build();

            return fakeConfiguration;
        }

        private Mock<DbSet<T>> GetConfiguredMockDbSet<T>(IEnumerable<T> entities)
            where T : class
        {
            var dbSet = new Mock<DbSet<T>>();

            // Set up the DbSet as an IQueryable so it can be enumerated.
            var queryable = entities.AsQueryable();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);

            return dbSet;
        }

        private TokenService GetTokenService()
        {
            return new TokenService(
                this.fakeConfiguration,
                this.mockDbContext.Object);
        }
    }
}
