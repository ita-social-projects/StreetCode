using System.Net;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.AuthorizationFixture;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Streetcode;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.StreetCode;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Streetcode
{
    [Collection("Authorization")]
    public class StreetcodeCreateControllerTests : BaseAuthorizationControllerTests<StreetcodeClient>
    {
        private readonly StreetcodeContent _testStreetcodeContent;

        public StreetcodeCreateControllerTests(CustomWebApplicationFactory<Program> factory, TokenStorage tokenStorage)
           : base(factory, "/api/Streetcode", tokenStorage)
        {
            var uniqueId = UniqueNumberGenerator.GenerateInt();
            _testStreetcodeContent = StreetcodeContentExtracter
                .Extract(
                    uniqueId,
                    uniqueId,
                    Guid.NewGuid().ToString());
        }

        [Fact]
        [ExtractCreateTestStreetcode]
        public async Task Create_TokenNotPassed_ReturnsUnauthorized()
        {
            // Arrange
            var streetcodeCreateDto = ExtractCreateTestStreetcodeAttribute.StreetcodeForTest;

            // Act
            var response = await Client.CreateAsync(streetcodeCreateDto);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        [ExtractCreateTestStreetcode]
        public async Task Create_NotAdminTokenPassed_ReturnsForbidden()
        {
            // Arrange
            var streetcodeCreateDto = ExtractCreateTestStreetcodeAttribute.StreetcodeForTest;

            // Act
            var response = await Client.CreateAsync(streetcodeCreateDto, TokenStorage.UserAccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        [ExtractCreateTestStreetcode]
        public async Task Create_WithInvalidData_ReturnsBadRequest()
        {
            // Arrange
            var streetcodeCreateDto = ExtractCreateTestStreetcodeAttribute.StreetcodeForTest;
            streetcodeCreateDto.Title = null!;  // Invalid data

            // Act
            var response = await Client.CreateAsync(streetcodeCreateDto, TokenStorage.AdminAccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        [ExtractCreateTestStreetcode]
        public async Task Create_WithExistingStreetcode_ReturnsConflict()
        {
            // Arrange
            var streetcodeCreateDto = ExtractCreateTestStreetcodeAttribute.StreetcodeForTest;
            streetcodeCreateDto.Index = _testStreetcodeContent.Index;

            // Act
            var response = await Client.CreateAsync(streetcodeCreateDto, TokenStorage.AdminAccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        [ExtractCreateTestStreetcode]
        public async Task Create_WithLongTransliterationUrl_ReturnsBadRequest()
        {
            // Arrange
            const int transliterationUrlMaxLength = 150;
            var streetcodeCreateDto = ExtractCreateTestStreetcodeAttribute.StreetcodeForTest;
            streetcodeCreateDto.TransliterationUrl = new string('a', transliterationUrlMaxLength + 1);

            // Act
            var response = await Client.CreateAsync(streetcodeCreateDto, TokenStorage.AdminAccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                StreetcodeContentExtracter.Remove(_testStreetcodeContent);
            }

            base.Dispose(disposing);
        }
    }
}
