using Streetcode.DAL.Entities.Streetcode;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Streetcode;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.StreetCode;
using System.Net;
using Xunit;
using Streetcode.XIntegrationTest.Base;

namespace Streetcode.XIntegrationTest.ControllerTests.Streetcode
{
    [Collection("Authorization")]
    public class StreetcodeCreateControllerTests : BaseAuthorizationControllerTests<StreetcodeClient>, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private StreetcodeContent _testStreetcodeContent;

        public StreetcodeCreateControllerTests(CustomWebApplicationFactory<Program> factory, TokenStorage tokenStorage)
           : base(factory, "/api/Streetcode", tokenStorage)
        {
            int uniqueId = UniqueNumberGenerator.GenerateInt();
            _testStreetcodeContent = StreetcodeContentExtracter
                .Extract(
                    uniqueId,
                    uniqueId,
                    Guid.NewGuid().ToString());
        }

        public override void Dispose()
        {
            StreetcodeContentExtracter.Remove(_testStreetcodeContent);
        }

        //[Fact]
        //[ExtractCreateTestStreetcode]
        //public async Task Create_ReturnsSuccessStatusCode()
        //{
        //    // Arrange
        //    var streetcodeCreateDTO = ExtractCreateTestStreetcode.StreetcodeForTest;


        //    // Act
        //    var response = await this.client.CreateAsync(streetcodeCreateDTO, this._tokenStorage.AdminToken);

        //    // Assert
        //    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        //}

        [Fact]
        [ExtractCreateTestStreetcode]
        public async Task Create_TokenNotPassed_ReturnsUnauthorized()
        {
            // Arrange
            var streetcodeCreateDTO = ExtractCreateTestStreetcode.StreetcodeForTest;

            // Act
            var response = await client.CreateAsync(streetcodeCreateDTO);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        [ExtractCreateTestStreetcode]
        public async Task Create_NotAdminTokenPassed_ReturnsForbidden()
        {
            // Arrange
            var streetcodeCreateDTO = ExtractCreateTestStreetcode.StreetcodeForTest;

            // Act
            var response = await client.CreateAsync(streetcodeCreateDTO, _tokenStorage.UserAccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }


        //[Fact]
        //[ExtractCreateTestStreetcode]
        //public async Task Create_CreatesNewStreetcode()
        //{
        //    // Arrange
        //    var streetcodeCreateDTO = ExtractCreateTestStreetcode.StreetcodeForTest;


        //    // Act
        //    var response = await this.client.CreateAsync(streetcodeCreateDTO, this._tokenStorage.AdminToken);
        //    var streetcodeId = StreetcodeIndexFetch.GetStreetcodeByIndex(streetcodeCreateDTO.Index);
        //    var getResponse = await this.client.GetByIdAsync(streetcodeId);
        //    var fetchedStreetcode = CaseIsensitiveJsonDeserializer.Deserialize<StreetcodeContent>(getResponse.Content);


        //    // Assert
        //    Assert.Equal(streetcodeCreateDTO.Title, fetchedStreetcode.Title);
        //    Assert.Equal(streetcodeCreateDTO.TransliterationUrl, fetchedStreetcode.TransliterationUrl);
        //}

        [Fact]
        [ExtractCreateTestStreetcode]
        public async Task Create_WithInvalidData_ReturnsBadRequest()
        {
            // Arrange
            var streetcodeCreateDTO = ExtractCreateTestStreetcode.StreetcodeForTest;
            streetcodeCreateDTO.Title = null;  // Invalid data

            // Act
            var response = await client.CreateAsync(streetcodeCreateDTO, _tokenStorage.AdminAccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        [ExtractCreateTestStreetcode]
        public async Task Create_WithExistingStreetcode_ReturnsConflict()
        {
            // Arrange
            var streetcodeCreateDTO = ExtractCreateTestStreetcode.StreetcodeForTest;
            streetcodeCreateDTO.Index = _testStreetcodeContent.Index;

            // Act
            var response = await client.CreateAsync(streetcodeCreateDTO, _tokenStorage.AdminAccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        [ExtractCreateTestStreetcode]
        public async Task Create_WithLongTransliterationUrl_ReturnsBadRequest()
        {
            // Arrange
            var transliterationUrlMaxLength = 150;
            var streetcodeCreateDTO = ExtractCreateTestStreetcode.StreetcodeForTest;
            streetcodeCreateDTO.TransliterationUrl = new string('a', transliterationUrlMaxLength + 1);

            // Act
            var response = await client.CreateAsync(streetcodeCreateDTO, _tokenStorage.AdminAccessToken);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
