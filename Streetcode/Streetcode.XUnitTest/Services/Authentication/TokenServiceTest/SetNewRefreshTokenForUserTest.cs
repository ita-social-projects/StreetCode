using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Streetcode.BLL.Services.Authentication;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Persistence;
using Xunit;

namespace Streetcode.XUnitTest.Services.Authentication.TokenServiceTest
{
    public class SetNewRefreshTokenForUserTest
    {
        private readonly string refreshTokenLifetimeInDays = "5";

        private readonly Mock<StreetcodeDbContext> mockDbContext;
        private readonly IConfiguration fakeConfiguration;
        private readonly TokenService tokenService;

        public SetNewRefreshTokenForUserTest()
        {
            this.mockDbContext = new Mock<StreetcodeDbContext>();
            this.fakeConfiguration = this.GetFakeConfiguration();

            this.tokenService = this.GetTokenService();
        }

        [Fact]
        public void ShouldThrowException_UserNotExists()
        {
            // Arrange.
            User inputUser = this.GetUser();
            this.SetupMockDbContext(null);
            var exceptionAction = this.tokenService.SetNewRefreshTokenForUser;

            // Act.

            // Assert.
            Assert.Throws<NullReferenceException>(() => exceptionAction(inputUser));
        }

        [Fact]
        public void ShouldReturnToken_ValidUser()
        {
            // Arrange.
            string testRefreshToken = "TestToken";
            User inputUser = this.GetUser(testRefreshToken);
            this.SetupMockDbContext(null);
            this.SetupMockDbContext(inputUser);

            // Act.
            string token = this.tokenService.SetNewRefreshTokenForUser(inputUser);

            // Assert.
            Assert.False(string.IsNullOrEmpty(token));
        }

        private IConfiguration GetFakeConfiguration()
        {
            var appSettingsStub = new Dictionary<string, string?>
            {
                { "Jwt:Key", "TestKey" },
                { "Jwt:Issuer", "TestIssuer" },
                { "Jwt:Audience", "TestAudience" },
                { "Jwt:RefreshTokenLifetimeInDays", this.refreshTokenLifetimeInDays },
            };
            var fakeConfiguration = new ConfigurationBuilder()
                .AddInMemoryCollection(appSettingsStub)
                .Build();

            return fakeConfiguration;
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
