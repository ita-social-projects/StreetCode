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
        private const string RefreshTokenLifetimeInDays = "5";

        private readonly Mock<StreetcodeDbContext> _mockDbContext;
        private readonly IConfiguration _fakeConfiguration;
        private readonly TokenService _tokenService;

        public SetNewRefreshTokenForUserTest()
        {
            _mockDbContext = new Mock<StreetcodeDbContext>();
            _fakeConfiguration = GetFakeConfiguration();

            _tokenService = GetTokenService();
        }

        [Fact]
        public void ShouldThrowException_UserNotExists()
        {
            // Arrange.
            User inputUser = GetUser();
            SetupMockDbContext(null);
            var exceptionAction = _tokenService.SetNewRefreshTokenForUser;

            // Act.

            // Assert.
            Assert.Throws<NullReferenceException>(() => exceptionAction(inputUser));
        }

        [Fact]
        public void ShouldReturnToken_ValidUser()
        {
            // Arrange.
            string testRefreshToken = "TestToken";
            User inputUser = GetUser(testRefreshToken);
            SetupMockDbContext(null);
            SetupMockDbContext(inputUser);

            // Act.
            string token = _tokenService.SetNewRefreshTokenForUser(inputUser);

            // Assert.
            Assert.False(string.IsNullOrEmpty(token));
        }

        private static User GetUser(string refreshToken = "", DateTime? refreshTokenExpireTime = null)
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

        private static IConfiguration GetFakeConfiguration()
        {
            var appSettingsStub = new Dictionary<string, string?>
            {
                { "Jwt:Key", "TestKey" },
                { "Jwt:Issuer", "TestIssuer" },
                { "Jwt:Audience", "TestAudience" },
                { "Jwt:RefreshTokenLifetimeInDays", RefreshTokenLifetimeInDays },
            };
            var fakeConfiguration = new ConfigurationBuilder()
                .AddInMemoryCollection(appSettingsStub)
                .Build();

            return fakeConfiguration;
        }

        private void SetupMockDbContext(User? userToReturn)
        {
            var mockDbSet = GetConfiguredMockDbSet<User>(new List<User>()
            {
                userToReturn ?? new User(),
            });
            _mockDbContext
                .Setup(context => context.Users)
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
