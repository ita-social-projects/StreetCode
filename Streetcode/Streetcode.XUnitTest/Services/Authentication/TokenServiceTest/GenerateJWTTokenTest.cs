using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Streetcode.BLL.Services.Authentication;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Persistence;
using Xunit;

namespace Streetcode.XUnitTest.Services.Authentication.TokenServiceTest
{
    public class GenerateJwtTokenTest
    {
        private const string JwtKey = "s_dkcLEWRlcksdmcQWE_124";
        private const string JwtIssuer = "Jwt_Issuer";
        private const string JwtAudience = "Jwt_Audience";
        private const string AccessTokenLifetimeInMinutes = "15";
        private readonly Mock<StreetcodeDbContext> _mockDbContext;
        private readonly IConfiguration _fakeConfiguration;
        private readonly TokenService _tokenService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenerateJwtTokenTest"/> class.
        /// </summary>
        public GenerateJwtTokenTest()
        {
            _mockDbContext = new Mock<StreetcodeDbContext>();
            _fakeConfiguration = GetFakeConfiguration();

            _tokenService = GetTokenService();
        }

        [Fact]
        public void ShouldThrowException_InputParameterIsNull()
        {
            // Arrange.
            var exceptionAction = _tokenService.GenerateAccessTokenAsync;

            // Act.

            // Assert.
            Assert.ThrowsAsync<ArgumentNullException>(() => exceptionAction(null));
        }

        [Fact]
        public async Task ShouldReturnNotNullToken_InputUserIsValid()
        {
            // Arrange.
            SetupMockDbContextGetRoles();
            SetupMockDbContextGetUserRoles();
            User user = GetUser();

            // Act.
            var token = await _tokenService.GenerateAccessTokenAsync(user);

            // Assert.
            Assert.NotNull(token);
        }

        [Fact]
        public async Task ShouldReturnCorrectData_InputUserIsValid()
        {
            // Arrange.
            User expectedUser = GetUser();
            string expectedRole = "Admin";
            SetupMockDbContextGetRoles();
            SetupMockDbContextGetUserRoles();

            // Act.
            var token = await _tokenService.GenerateAccessTokenAsync(expectedUser);

            // Assert.
            Assert.Equal(expectedUser.Email, token.Claims.First(claim => claim.Type == "email").Value);
            Assert.Equal(expectedRole, token.Claims.First(claim => claim.Type == "role").Value);
        }

        private static IQueryable<IdentityUserRole<string>> GetUserRoles()
        {
            var userRoles = new List<IdentityUserRole<string>>()
            {
                new IdentityUserRole<string>() { UserId = "1", RoleId = "1" },
                new IdentityUserRole<string>() { UserId = "2", RoleId = "1" },
                new IdentityUserRole<string>() { UserId = "3", RoleId = "2" },
            };
            return userRoles.AsQueryable();
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

        private static User GetUser()
        {
            return new User()
            {
                Id = "1",
                Name = "John",
                Surname = "Doe",
                Email = "JohnDoe@gmail.com",
            };
        }

        private static IConfiguration GetFakeConfiguration()
        {
            var appSettingsStub = new Dictionary<string, string?>
            {
                { "Jwt:Key", JwtKey },
                { "Jwt:Issuer", JwtIssuer },
                { "Jwt:Audience", JwtAudience },
                { "Jwt:AccessTokenLifetimeInMinutes", AccessTokenLifetimeInMinutes },
            };
            var fakeConfiguration = new ConfigurationBuilder()
                .AddInMemoryCollection(appSettingsStub)
                .Build();

            return fakeConfiguration;
        }

        private static Mock<DbSet<T>> GetConfiguredMockDbSet<T>(IEnumerable<T> entities)
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

        private void SetupMockDbContextGetUserRoles()
        {
            var mockDbSet = GetConfiguredMockDbSet(GetUserRoles());
            _mockDbContext
                .Setup(context => context.UserRoles)
                .Returns(mockDbSet.Object);
        }

        private void SetupMockDbContextGetRoles()
        {
            var mockDbSet = GetConfiguredMockDbSet(GetRoles());
            _mockDbContext
                .Setup(context => context.Roles)
                .Returns(mockDbSet.Object);
        }

        private TokenService GetTokenService()
        {
            return new TokenService(
                _fakeConfiguration,
                _mockDbContext.Object);
        }
    }
}
