using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Streetcode.BLL.Services.Authentication;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Enums;
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
            mockDbContext = new Mock<StreetcodeDbContext>();
            fakeConfiguration = GetFakeConfiguration();

            tokenService = GetTokenService();
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
            var invalidToken = GetToken(CreateTokenOptions.InvalidToken, new User(), string.Empty);
            string invalidTokenStringified = new JwtSecurityTokenHandler().WriteToken(invalidToken);
            var exceptionAction = tokenService.RefreshToken;

            // Act.

            // Assert.
            Assert.Throws<SecurityTokenException>(() => exceptionAction(invalidTokenStringified, testRefreshToken));
        }

        [Fact]
        public void ShouldThrowException_UserFromInputAccessTokenNotExists()
        {
            // Arrange.
            string testRefreshToken = "TestRefreshToken";
            var validToken = GetToken(CreateTokenOptions.ValidToken, GetUser(), string.Empty);
            string validTokenStringified = new JwtSecurityTokenHandler().WriteToken(validToken);
            SetupMockDbContext(null);
            var exceptionAction = tokenService.RefreshToken;

            // Act.

            // Assert.
            Assert.Throws<NullReferenceException>(() => exceptionAction(validTokenStringified, testRefreshToken));
        }

        [Fact]
        public void ShouldThrowException_InvalidInputRefreshToken()
        {
            // Arrange.
            string testRefreshToken = "TestRefreshToken";
            User testUser = GetUser();
            var invalidToken = GetToken(CreateTokenOptions.ValidToken, GetUser(), string.Empty);
            string invalidTokenStringified = new JwtSecurityTokenHandler().WriteToken(invalidToken);
            SetupMockDbContext(testUser);
            var exceptionAction = tokenService.RefreshToken;

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
            User testUser = GetUser(testRefreshToken);
            var invalidToken = GetToken(CreateTokenOptions.ValidToken, GetUser(), string.Empty);
            string invalidTokenStringified = new JwtSecurityTokenHandler().WriteToken(invalidToken);
            SetupMockDbContext(testUser);
            var exceptionAction = tokenService.RefreshToken;

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
            var user = GetUser(testRefreshToken, DateTime.Now.AddDays(1));
            string role = "User";
            var invalidToken = GetToken(CreateTokenOptions.ValidToken, user, role);
            string invalidTokenStringified = new JwtSecurityTokenHandler().WriteToken(invalidToken);
            SetupMockDbContext(user);
            SetupMockDbContextGetUserRoles();
            SetupMockDbContextGetRoles();

            // Act.
            var actualToken = tokenService.RefreshToken(invalidTokenStringified, testRefreshToken);

            // Assert.
            Assert.NotNull(actualToken);
        }

        [Fact]
        public void ShouldTokenWithCorrectData_ValidInputToken()
        {
            // Arrange.
            string testRefreshToken = "TestRefreshToken";
            User expectedUser = GetUser(testRefreshToken, DateTime.Now.AddDays(1));
            string expectedRole = "User";
            var invalidToken = GetToken(CreateTokenOptions.ValidToken, expectedUser, expectedRole);
            string invalidTokenStringified = new JwtSecurityTokenHandler().WriteToken(invalidToken);
            SetupMockDbContext(expectedUser);
            SetupMockDbContextGetUserRoles();
            SetupMockDbContextGetRoles();

            // Act.
            var actualToken = tokenService.RefreshToken(invalidTokenStringified, testRefreshToken);

            // Assert.
            Assert.Equal(expectedUser.Email, actualToken.Claims.First(claim => claim.Type == "email").Value);
            Assert.Equal(expectedRole, actualToken.Claims.First(claim => claim.Type == "role").Value);
        }

        private void SetupMockDbContext(User? userToReturn)
        {
            var mockDbSet = GetConfiguredMockDbSet<User>(new List<User>()
            {
                userToReturn ?? new User(),
            });
            mockDbContext
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
                SigningCredentials = GetSigningCredentials(),
                Issuer = jwtIssuer,
                Audience = jwtAudience,
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
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            return signingCredentials;
        }

        private IConfiguration GetFakeConfiguration()
        {
            var appSettingsStub = new Dictionary<string, string?>
            {
                { "Jwt:Key", jwtKey },
                { "Jwt:Issuer", jwtIssuer },
                { "Jwt:Audience", jwtAudience },
                { "Jwt:AccessTokenLifetimeInMinutes", accessTokenLifetimeInMinutes },
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

        private static IQueryable<IdentityRole> GetRoles()
        {
            var roles = new List<IdentityRole>()
            {
                new IdentityRole() { Id = "1", Name = nameof(UserRole.Admin) },
                new IdentityRole() { Id = "2", Name = nameof(UserRole.User) },
            };
            return roles.AsQueryable();
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

        private static IQueryable<IdentityUserRole<string>> GetUserRoles()
        {
            var userRoles = new List<IdentityUserRole<string>>()
            {
                new IdentityUserRole<string>() { UserId = "1", RoleId = "2" },
                new IdentityUserRole<string>() { UserId = "2", RoleId = "1" },
                new IdentityUserRole<string>() { UserId = "3", RoleId = "2" },
            };
            return userRoles.AsQueryable();
        }

        private void SetupMockDbContextGetUserRoles()
        {
            var mockDbSet = GetConfiguredMockDbSet(GetUserRoles());
            mockDbContext
                .Setup(context => context.UserRoles)
                .Returns(mockDbSet.Object);
        }

        private void SetupMockDbContextGetRoles()
        {
            var mockDbSet = GetConfiguredMockDbSet(GetRoles());
            mockDbContext
                .Setup(context => context.Roles)
                .Returns(mockDbSet.Object);
        }

        private TokenService GetTokenService()
        {
            return new TokenService(
                fakeConfiguration,
                mockDbContext.Object);
        }
    }
}
