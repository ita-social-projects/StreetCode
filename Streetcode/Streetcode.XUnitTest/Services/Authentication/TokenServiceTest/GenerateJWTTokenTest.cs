using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Persistence;
using Xunit;
using Microsoft.Extensions.Configuration;
using Streetcode.BLL.Services.Authentication;

namespace Streetcode.XUnitTest.Services.Authentication.TokenServiceTest
{
    public class GenerateJWTTokenTest
    {
        private const string _JwtAudience = "JWT_Audience";
        private const string _JwtIssuer = "JWT_Issuer";
        private const string _JwtKey = "LKqwleLVcdsl234po14lckd34lkdcdDlakjc";
        private readonly Mock<StreetcodeDbContext> _mockDbContext;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly TokenService _tokenService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenerateJWTTokenTest"/> class.
        /// </summary>
        public GenerateJWTTokenTest()
        {
            this._mockDbContext = new Mock<StreetcodeDbContext>();
            this._mockConfiguration = new Mock<IConfiguration>();

            this.SetupMockConfiguration();
            this._tokenService = this.GetTokenService();
        }

        [Fact]
        public void ShouldThrowException_InputParameterIsNull()
        {
            // Arrange.
            var exceptionAction = this._tokenService.GenerateJWTToken;

            // Act.

            // Assert.
            Assert.Throws<ArgumentNullException>(() => exceptionAction(null));
        }

        [Fact]
        public void ShouldReturnNotNullToken_InputUserIsValid()
        {
            // Arrange.
            this.SetupMockDbContextGetRoles();
            this.SetupMockDbContextGetUserRoles();
            User user = this.GetUser();

            // Act.
            var token = this._tokenService.GenerateJWTToken(user);

            // Assert.
            Assert.NotNull(token);
        }

        [Fact]
        public void ShouldReturnCorrectData_InputUserIsValid()
        {
            // Arrange.
            User expectedUser = GetUser();
            string expectedRole = "Admin";
            this.SetupMockDbContextGetRoles();
            this.SetupMockDbContextGetUserRoles();

            // Act.
            var token = this._tokenService.GenerateJWTToken(expectedUser);

            // Assert.
            Assert.Equal(expectedUser.Email, token.Claims.FirstOrDefault(claim => claim.Type == "email") !.Value);
            Assert.Equal(expectedRole, token.Claims.FirstOrDefault(claim => claim.Type == "role") !.Value);
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

        private void SetupMockDbContextGetUserRoles()
        {
            var mockDbSet = this.GetConfiguredMockDbSet(GetUserRoles());
            this._mockDbContext
                .Setup(context => context.UserRoles)
                .Returns(mockDbSet.Object);
        }

        private void SetupMockDbContextGetRoles()
        {
            var mockDbSet = this.GetConfiguredMockDbSet(GetRoles());
            this._mockDbContext
                .Setup(context => context.Roles)
                .Returns(mockDbSet.Object);
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
                this._mockConfiguration.Object,
                this._mockDbContext.Object);
        }
    }
}
