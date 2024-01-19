using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Authentication;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Authentication;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using System.Net;
using Xunit;
using Streetcode.BLL.DTO.Authentication.Login;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Entities.Users;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.Authentication;

namespace Streetcode.XIntegrationTest.ControllerTests.Authentication
{
    public class AuthControllerTests : BaseControllerTests<AuthenticationClient>, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private User _testUser;
        private string _testPassword;

        public AuthControllerTests(CustomWebApplicationFactory<Program> factory)
           : base(factory, "/api/Auth")
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
