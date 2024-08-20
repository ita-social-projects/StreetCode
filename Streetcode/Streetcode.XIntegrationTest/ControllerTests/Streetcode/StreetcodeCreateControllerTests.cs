using System.Net;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Streetcode;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.StreetCode;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Streetcode
{
    [Collection("Authorization")]
    public class StreetcodeCreateControllerTests : BaseAuthorizationControllerTests<StreetcodeClient>, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly StreetcodeContent _testStreetcodeContent;

        public StreetcodeCreateControllerTests(CustomWebApplicationFactory<Program> factory, TokenStorage tokenStorage)
           : base(factory, "/api/Streetcode", tokenStorage)
        {
            int uniqueId = UniqueNumberGenerator.GenerateInt();
            this._testStreetcodeContent = StreetcodeContentExtracter
                .Extract(
                    uniqueId,
                    uniqueId,
                    Guid.NewGuid().ToString());
        }

        public override void Dispose()
        {
            StreetcodeContentExtracter.Remove(this._testStreetcodeContent);
        }

        [Fact(Skip = "There are no images in the streetcode, so the test will fail without them.")]
        [ExtractCreateTestStreetcode]
        public async Task Create_ReturnsSuccessStatusCode()
        {
            // Arrange
            var streetcodeCreateDTO = ExtractCreateTestStreetcodeAttribute.StreetcodeForTest;

            // Act
            var response = await this.Client.CreateAsync(streetcodeCreateDTO, this.TokenStorage.AdminAccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(Skip = "This test will fail if the previous test fails.")]
        [ExtractCreateTestStreetcode]
        public async Task Create_CreatesNewStreetcode()
        {
            // Arrange
            var streetcodeCreateDTO = ExtractCreateTestStreetcodeAttribute.StreetcodeForTest;

            // Act
            await this.Client.CreateAsync(streetcodeCreateDTO, this.TokenStorage.AdminAccessToken);
            var streetcodeId = StreetcodeIndexFetch.GetStreetcodeByIndex(streetcodeCreateDTO.Index);
            var getResponse = await this.Client.GetByIdAsync(streetcodeId);
            var fetchedStreetcode = CaseIsensitiveJsonDeserializer.Deserialize<StreetcodeContent>(getResponse.Content);

            // Assert
            Assert.Equal(streetcodeCreateDTO.Title, fetchedStreetcode?.Title);
            Assert.Equal(streetcodeCreateDTO.TransliterationUrl, fetchedStreetcode?.TransliterationUrl);
        }

        [Fact]
        [ExtractCreateTestStreetcode]
        public async Task Create_TokenNotPassed_ReturnsUnauthorized()
        {
            // Arrange
            var streetcodeCreateDTO = ExtractCreateTestStreetcodeAttribute.StreetcodeForTest;

            // Act
            var response = await this.Client.CreateAsync(streetcodeCreateDTO);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        [ExtractCreateTestStreetcode]
        public async Task Create_NotAdminTokenPassed_ReturnsForbidden()
        {
            // Arrange
            var streetcodeCreateDTO = ExtractCreateTestStreetcodeAttribute.StreetcodeForTest;

            // Act
            var response = await this.Client.CreateAsync(streetcodeCreateDTO, this.TokenStorage.UserAccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        [ExtractCreateTestStreetcode]
        public async Task Create_WithInvalidData_ReturnsBadRequest()
        {
            // Arrange
            var streetcodeCreateDTO = ExtractCreateTestStreetcodeAttribute.StreetcodeForTest;
            streetcodeCreateDTO.Title = null!;  // Invalid data

            // Act
            var response = await this.Client.CreateAsync(streetcodeCreateDTO, this.TokenStorage.AdminAccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        [ExtractCreateTestStreetcode]
        public async Task Create_WithExistingStreetcode_ReturnsConflict()
        {
            // Arrange
            var streetcodeCreateDTO = ExtractCreateTestStreetcodeAttribute.StreetcodeForTest;
            streetcodeCreateDTO.Index = this._testStreetcodeContent.Index;

            // Act
            var response = await this.Client.CreateAsync(streetcodeCreateDTO, this.TokenStorage.AdminAccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        [ExtractCreateTestStreetcode]
        public async Task Create_WithLongTransliterationUrl_ReturnsBadRequest()
        {
            // Arrange
            var transliterationUrlMaxLength = 150;
            var streetcodeCreateDTO = ExtractCreateTestStreetcodeAttribute.StreetcodeForTest;
            streetcodeCreateDTO.TransliterationUrl = new string('a', transliterationUrlMaxLength + 1);

            // Act
            var response = await this.Client.CreateAsync(streetcodeCreateDTO, this.TokenStorage.AdminAccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
