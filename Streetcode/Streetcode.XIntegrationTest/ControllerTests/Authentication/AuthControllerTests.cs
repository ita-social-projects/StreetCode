using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Authentication;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Authentication;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.BLL.DTO.Authentication.Login;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Entities.Users;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.Authentication;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.BLL.DTO.Authentication.RefreshToken;

namespace Streetcode.XIntegrationTest.ControllerTests.Authentication
{
    using System.Net;
    using Xunit;

    [Collection("Authorization")]
    public class AuthControllerTests : BaseAuthorizationControllerTests<AuthenticationClient>, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private User _testUser;
        private string _testPassword;

        public AuthControllerTests(CustomWebApplicationFactory<Program> factory, TokenStorage tokenStorage)
           : base(factory, "/api/Auth", tokenStorage)
        {
            (this._testUser, this._testPassword) = UserExtracter.Extract(
                userId: Guid.NewGuid().ToString(),
                password: this.GenerateTestPassword(),
                nameof(UserRole.User),
                nameof(UserRole.Admin));
        }

        public override void Dispose()
        {
            UserExtracter.Remove(this._testUser);
        }

        [Fact]
        [ExtractRegisterRequest]
        public async Task Register_ReturnsSuccessStatusCode()
        {
            // Arrange.
            var registerRequest = ExtractRegisterRequest.RegisterRequest;

            // Act.
            var response = await this.client.Register(registerRequest);

            // Assert.
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        [ExtractRegisterRequest]
        public async Task Register_InvalidInputData_Returns404BadRequest()
        {
            // Arrange.
            var registerRequest = ExtractRegisterRequest.RegisterRequest;
            registerRequest.Email = string.Empty;

            // Act.
            var response = await this.client.Register(registerRequest);

            // Assert.
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        [ExtractRegisterRequest]
        public async Task Register_WithGivenEmailAlreadyInDatabase_Returns404BadRequest()
        {
            // Arrange.
            var registerRequest = ExtractRegisterRequest.RegisterRequest;
            registerRequest.Email = this._testUser.Email;

            // Act.
            var response = await this.client.Register(registerRequest);

            // Assert.
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        [ExtractRegisterRequest]
        public async Task Register_WithGivenUsernameAlreadyInDatabase_Returns404BadRequest()
        {
            // Arrange.
            var registerRequest = ExtractRegisterRequest.RegisterRequest;
            registerRequest.UserName = this._testUser.UserName;

            // Act.
            var response = await this.client.Register(registerRequest);

            // Assert.
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Login_ReturnsSuccessStatusCode()
        {
            // Arrange.
            LoginRequestDTO loginRequest = this.GetLoginRequestDTO();

            // Act.
            var response = await this.client.Login(loginRequest);

            // Assert.
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Login_InvalidInputData_Returns404BadRequest()
        {
            // Arrange.
            LoginRequestDTO loginRequest = this.GetLoginRequestDTO();
            loginRequest.Login = string.Empty;

            // Act.
            var response = await this.client.Login(loginRequest);

            // Assert.
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task RefreshToken_ReturnsSuccess()
        {
            // Arrange.
            RefreshTokenRequestDTO refreshTokenRequestDTO = new RefreshTokenRequestDTO()
            {
                Token = this._tokenStorage.UserToken,
            };

            // Act.
            var response = await this.client.RefreshToken(refreshTokenRequestDTO);

            // Assert.
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task RefreshToken_InvalidToken_ReturnsBadRequest()
        {
            // Arrange.
            RefreshTokenRequestDTO refreshTokenRequestDTO = new RefreshTokenRequestDTO()
            {
                Token = "invalid_Token",
            };

            // Act.
            var response = await this.client.RefreshToken(refreshTokenRequestDTO);

            // Assert.
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        private LoginRequestDTO GetLoginRequestDTO()
        {
            return new LoginRequestDTO()
            {
                Login = this._testUser.Email,
                Password = this._testPassword,
            };
        }

        private string GenerateTestPassword()
        {
            string guid = Guid.NewGuid().ToString();
            return $"TestPass123_{guid.Substring(0, 10)}";
        }
    }
}
