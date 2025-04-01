using System.Net;
using Moq;
using Streetcode.BLL.DTO.Authentication.GoogleLogin;
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
    public class AuthControllerTests : BaseAuthorizationControllerTests<AuthenticationClient>
    {
        private readonly User _testUser;
        private readonly string _testPassword;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public AuthControllerTests(CustomWebApplicationFactory<Program> factory, TokenStorage tokenStorage)
           : base(factory, "/api/Auth", tokenStorage)
        {
            _factory = factory;
            _factory.GoogleServiceMock.Reset();
            _factory.EmailServiceMock.Reset();

            TokenStorage = new TokenStorage();
            (_testUser, _testPassword) = UserExtracter.Extract(
                userId: Guid.NewGuid().ToString(),
                userName: Guid.NewGuid().ToString(),
                password: GenerateTestPassword(),
                isEmailConfirmed: true,
                nameof(UserRole.User),
                nameof(UserRole.Admin));
        }

        [Fact]
        [ExtractRegisterRequest]
        public async Task Register_ValidInput_ReturnsSuccessStatusCode()
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
        public async Task Register_InvalidInputData_ReturnsBadRequest()
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
        public async Task Register_EmailAlreadyExists_ReturnsBadRequest()
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
        public async Task Login_ValidInput_ReturnsSuccessStatusCode()
        {
            // Arrange.
            LoginRequestDTO loginRequest = GetLoginRequestDTO();

            // Act.
            var response = await Client.Login(loginRequest);

            // Assert.
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Login_InvalidInputData_ReturnsBadRequest()
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
        public async Task RefreshToken_ValidTokens_ReturnsSuccess()
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
        public async Task Logout_ValidToken_ReturnsSuccessStatusCode()
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

        [Fact]
        public async Task LoginGoogle_ValidToken_ReturnsSuccessAndUserData()
        {
            // Arrange
            var validToken = "valid_google_id_token";
            var request = new GoogleLoginRequest
            {
                IdToken = validToken,
            };
            _factory.SetupMockGoogleLogin(_testUser, validToken);

            // Act
            var response = await Client.GoogleLogin(request);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<LoginResponseDTO>(response.Content);

            // Assert
            _factory.GoogleServiceMock.Verify(es => es.ValidateGoogleToken(It.IsAny<string>()), Times.Once);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(returnedValue);
            Assert.NotNull(returnedValue.User);
            Assert.False(string.IsNullOrEmpty(returnedValue.AccessToken));
        }

        [Fact]
        public async Task LoginGoogle_InvalidToken_ReturnsBadRequest()
        {
            // Arrange
            var invalidToken = "invalid_google_id_token";
            var request = new GoogleLoginRequest
            {
                IdToken = invalidToken,
            };
            _factory.SetupMockGoogleLogin(_testUser);

            // Act
            var response = await Client.GoogleLogin(request);

            // Assert
            _factory.GoogleServiceMock.Verify(es => es.ValidateGoogleToken(It.IsAny<string>()), Times.Once);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task LoginGoogle_NewUser_CreatesAccountAndReturnsToken()
        {
            // Arrange
            var newUserToken = "new_google_id_token";
            var request = new GoogleLoginRequest
            {
                IdToken = newUserToken,
            };
            _testUser.Email = "newemail@test.com";
            _factory.SetupMockGoogleLogin(_testUser, newUserToken);

            // Act
            var response = await Client.GoogleLogin(request);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<LoginResponseDTO>(response.Content);

            // Assert
            _factory.GoogleServiceMock.Verify(es => es.ValidateGoogleToken(It.IsAny<string>()), Times.Once);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(returnedValue);
            Assert.NotNull(returnedValue.User);
            Assert.False(string.IsNullOrEmpty(returnedValue.AccessToken));
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
