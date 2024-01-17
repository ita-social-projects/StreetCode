using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Streetcode;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using System.Net;
using Xunit;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Authentication;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Authentication;

namespace Streetcode.XIntegrationTest.ControllerTests.Authentication
{
    public class AuthControllerTests : BaseControllerTests<AuthenticationClient>, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        public AuthControllerTests(CustomWebApplicationFactory<Program> factory)
           : base(factory, "/api/Auth")
        {
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
    }
}
