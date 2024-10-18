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
    public class GenerateJWTTokenTest
    {
        private readonly string jwtKey = "s_dkcLEWRlcksdmcQWE_124";
        private readonly string jwtIssuer = "Jwt_Issuer";
        private readonly string jwtAudience = "Jwt_Audience";
        private readonly string accessTokenLifetimeInMinutes = "15";
        private readonly Mock<StreetcodeDbContext> mockDbContext;
        private readonly IConfiguration fakeConfiguration;
        private readonly TokenService _tokenService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenerateJWTTokenTest"/> class.
        /// </summary>
        public GenerateJWTTokenTest()
        {
            this.mockDbContext = new Mock<StreetcodeDbContext>();
            this.fakeConfiguration = this.GetFakeConfiguration();

            this._tokenService = this.GetTokenService();
        }

        [Fact]
        public void ShouldThrowException_InputParameterIsNull()
        {
            // Arrange.
            var exceptionAction = this._tokenService.GenerateAccessTokenAsync;

            // Act.

            // Assert.
            Assert.ThrowsAsync<ArgumentNullException>(() => exceptionAction(null));
        }

        [Fact]
        public async Task ShouldReturnNotNullToken_InputUserIsValid()
        {
            // Arrange.
            this.SetupMockDbContextGetRoles();
            this.SetupMockDbContextGetUserRoles();
            User user = this.GetUser();

            // Act.
            var token = await this._tokenService.GenerateAccessTokenAsync(user);

            // Assert.
            Assert.NotNull(token);
        }

        [Fact]
        public async Task ShouldReturnCorrectData_InputUserIsValid()
        {
            // Arrange.
            User expectedUser = this.GetUser();
            string expectedRole = "Admin";
            this.SetupMockDbContextGetRoles();
            this.SetupMockDbContextGetUserRoles();

            // Act.
            var token = await this._tokenService.GenerateAccessTokenAsync(expectedUser);

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

        private void SetupMockDbContextGetUserRoles()
        {
            var mockDbSet = this.GetConfiguredMockDbSet(GetUserRoles());
            this.mockDbContext
                .Setup(context => context.UserRoles)
                .Returns(mockDbSet.Object);
        }

        private void SetupMockDbContextGetRoles()
        {
            var mockDbSet = this.GetConfiguredMockDbSet(GetRoles());
            this.mockDbContext
                .Setup(context => context.Roles)
                .Returns(mockDbSet.Object);
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
