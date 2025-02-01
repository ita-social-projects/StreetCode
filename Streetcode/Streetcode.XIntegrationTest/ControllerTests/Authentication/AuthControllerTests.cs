using System.Net;
using Streetcode.BLL.DTO.Authentication.Login;
using Streetcode.BLL.DTO.Authentication.RefreshToken;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Enums;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.AuthorizationFixture;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Authentication;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Authentication;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.Authentication;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Authentication
{
    [Collection("Authorization")]
    public class AuthControllerTests : BaseAuthorizationControllerTests<AuthenticationClient>, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly User _testUser;
        private readonly string _testPassword;

        public AuthControllerTests(CustomWebApplicationFactory<Program> factory, TokenStorage tokenStorage)
           : base(factory, "/api/Auth", tokenStorage)
        {
            TokenStorage = new TokenStorage();
            (_testUser, _testPassword) = UserExtracter.Extract(
                userId: Guid.NewGuid().ToString(),
                userName: Guid.NewGuid().ToString(),
                password: GenerateTestPassword(),
                nameof(UserRole.User),
                nameof(UserRole.Admin));
        }

        [Fact]
        [ExtractRegisterRequest]
        public async Task Register_ReturnsSuccessStatusCode()
        {
            // Arrange.
            var registerRequest = ExtractRegisterRequestAttribute.RegisterRequest;

            // Act.
            var response = await Client.Register(registerRequest);

            // Assert.
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        [ExtractRegisterRequest]
        public async Task Register_InvalidInputData_Returns404BadRequest()
        {
            // Arrange.
            var registerRequest = ExtractRegisterRequestAttribute.RegisterRequest;
            registerRequest.Email = string.Empty;

            // Act.
            var response = await Client.Register(registerRequest);

            // Assert.
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        [ExtractRegisterRequest]
        public async Task Register_WithGivenEmailAlreadyInDatabase_Returns404BadRequest()
        {
            // Arrange.
            var registerRequest = ExtractRegisterRequestAttribute.RegisterRequest;
            registerRequest.Email = _testUser.Email!;

            // Act.
            var response = await Client.Register(registerRequest);

            // Assert.
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Login_ReturnsSuccessStatusCode()
        {
            // Arrange.
            LoginRequestDTO loginRequest = GetLoginRequestDTO();

            // Act.
            var response = await Client.Login(loginRequest);

            // Assert.
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Login_InvalidInputData_Returns404BadRequest()
        {
            // Arrange.
            LoginRequestDTO loginRequest = GetLoginRequestDTO();
            loginRequest.Login = string.Empty;

            // Act.
            var response = await Client.Login(loginRequest);

            // Assert.
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task RefreshToken_ReturnsSuccess()
        {
            // Arrange.
            RefreshTokenRequestDTO refreshTokenRequestDTO = new RefreshTokenRequestDTO()
            {
                AccessToken = TokenStorage.UserAccessToken,
                RefreshToken = TokenStorage.UserRefreshToken,
            };

            // Act.
            var response = await Client.RefreshToken(refreshTokenRequestDTO);

            // Assert.
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task RefreshToken_InvalidAccessToken_ReturnsBadRequest()
        {
            // Arrange.
            RefreshTokenRequestDTO refreshTokenRequestDTO = new RefreshTokenRequestDTO()
            {
                AccessToken = "invalid_Token",
            };

            // Act.
            var response = await Client.RefreshToken(refreshTokenRequestDTO);

            // Assert.
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task RefreshToken_InvalidRefreshToken_ReturnsUnauthorized()
        {
            // Arrange.
            RefreshTokenRequestDTO refreshTokenRequestDTO = new RefreshTokenRequestDTO()
            {
                AccessToken = TokenStorage.UserAccessToken,
                RefreshToken = "invalid_token",
            };

            // Act.
            var response = await Client.RefreshToken(refreshTokenRequestDTO);

            // Assert.
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Logout_InvalidToken_ReturnsUnauthorized()
        {
            // Act.
            var response = await Client.Logout();

            // Assert.
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Logout_ValidToken_ReturnsStatusOk()
        {
            // Arrange.
            await TokenStorage.GenerateNewTokens(_testUser);
            var validToken = TokenStorage.UserAccessToken;

            // Act.
            var response = await Client.Logout(validToken);

            // Assert.
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Logout_InvalidUserId_ReturnsBadRequest()
        {
            // Arrange.
            _testUser.UserName = "invalid_username";
            _testUser.NormalizedUserName = "INVALID_USERNAME";
            await TokenStorage.GenerateNewTokens(_testUser);
            var invalidToken = TokenStorage.UserAccessToken;

            // Act.
            var response = await Client.Logout(invalidToken);

            // Assert.
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                UserExtracter.Remove(_testUser);
            }

            base.Dispose(disposing);
        }

        private LoginRequestDTO GetLoginRequestDTO()
        {
            return new LoginRequestDTO()
            {
                Login = _testUser.Email!,
                Password = _testPassword,
                CaptchaToken = "test_captcha_token",
            };
        }

        private string GenerateTestPassword()
        {
            string guid = Guid.NewGuid().ToString();
            return $"TestPass123_{guid.Substring(0, 10)}";
        }
    }
}
