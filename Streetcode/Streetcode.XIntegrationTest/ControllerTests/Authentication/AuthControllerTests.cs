using Streetcode.BLL.DTO.Authentication.Login;
using Streetcode.BLL.DTO.Authentication.RefreshToken;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Enums;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Authentication;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Authentication;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.Authentication;

namespace Streetcode.XIntegrationTest.ControllerTests.Authentication
{
    using System.Net;
    using Xunit;

    [Collection("Authorization")]
    public class AuthControllerTests : BaseAuthorizationControllerTests<AuthenticationClient>, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly User testUser;
        private readonly string testPassword;

        public AuthControllerTests(CustomWebApplicationFactory<Program> factory, TokenStorage tokenStorage)
           : base(factory, "/api/Auth", tokenStorage)
        {
            (this.testUser, this.testPassword) = UserExtracter.Extract(
                userId: Guid.NewGuid().ToString(),
                userName: Guid.NewGuid().ToString(),
                password: this.GenerateTestPassword(),
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
            var response = await this.Client.Register(registerRequest);

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
            var response = await this.Client.Register(registerRequest);

            // Assert.
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        [ExtractRegisterRequest]
        public async Task Register_WithGivenEmailAlreadyInDatabase_Returns404BadRequest()
        {
            // Arrange.
            var registerRequest = ExtractRegisterRequestAttribute.RegisterRequest;
            registerRequest.Email = this.testUser.Email!;

            // Act.
            var response = await this.Client.Register(registerRequest);

            // Assert.
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Login_ReturnsSuccessStatusCode()
        {
            // Arrange.
            LoginRequestDto loginRequest = this.GetLoginRequestDTO();

            // Act.
            var response = await this.Client.Login(loginRequest);

            // Assert.
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Login_InvalidInputData_Returns404BadRequest()
        {
            // Arrange.
            LoginRequestDto loginRequest = this.GetLoginRequestDTO();
            loginRequest.Login = string.Empty;

            // Act.
            var response = await this.Client.Login(loginRequest);

            // Assert.
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task RefreshToken_ReturnsSuccess()
        {
            // Arrange.
            RefreshTokenRequestDto refreshTokenRequestDTO = new RefreshTokenRequestDto()
            {
                AccessToken = this.TokenStorage.UserAccessToken,
                RefreshToken = this.TokenStorage.UserRefreshToken,
            };

            // Act.
            var response = await this.Client.RefreshToken(refreshTokenRequestDTO);

            // Assert.
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task RefreshToken_InvalidAccessToken_ReturnsBadRequest()
        {
            // Arrange.
            RefreshTokenRequestDto refreshTokenRequestDTO = new RefreshTokenRequestDto()
            {
                AccessToken = "invalid_Token",
            };

            // Act.
            var response = await this.Client.RefreshToken(refreshTokenRequestDTO);

            // Assert.
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task RefreshToken_InvalidRefreshToken_ReturnsUnauthorized()
        {
            // Arrange.
            RefreshTokenRequestDto refreshTokenRequestDTO = new RefreshTokenRequestDto()
            {
                AccessToken = this.TokenStorage.UserAccessToken,
                RefreshToken = "invalid_token",
            };

            // Act.
            var response = await this.Client.RefreshToken(refreshTokenRequestDTO);

            // Assert.
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                UserExtracter.Remove(this.testUser);
            }

            base.Dispose(disposing);
        }

        private LoginRequestDto GetLoginRequestDTO()
        {
            return new LoginRequestDto()
            {
                Login = this.testUser.Email!,
                Password = this.testPassword,
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
