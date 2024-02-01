using Streetcode.BLL.DTO.Media.Art;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Media.Images.Art;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Streetcode;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.Media.Image;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Media.Images
{
    public class ArtControllerTests : BaseControllerTests, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private Art _testArt;
        private StreetcodeContent _testStreetcodeContent;

        public ArtControllerTests(CustomWebApplicationFactory<Program> factory)
            : base(factory, "/api/Art")
        {
            this._testArt = ArtExtracter.Extract(this.GetHashCode());
            this._testStreetcodeContent = StreetcodeContentExtracter
                .Extract(
                    this.GetHashCode(),
                    this.GetHashCode(),
                    Guid.NewGuid().ToString());
        }

        public override void Dispose()
        {
            StreetcodeContentExtracter.Remove(this._testStreetcodeContent);
            ArtExtracter.Remove(this._testArt);
        }

        [Fact]
        public async Task GetAll_ReturnSuccessStatusCode()
        {
            var response = await this.client.GetAllAsync();
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<ArtDTO>>(response.Content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
        }

        [Fact]
        public async Task GetById_ReturnSuccessStatusCode()
        {
            Art expectedArt = this._testArt;
            var response = await this.client.GetByIdAsync(expectedArt.Id);

            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<ArtDTO>(response.Content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
            Assert.Multiple(
                () => Assert.Equal(expectedArt.Id, returnedValue.Id),
                () => Assert.Equal(expectedArt.ImageId, returnedValue.ImageId),
                () => Assert.Equal(expectedArt.Description, returnedValue.Description));
        }

        [Fact]
        public async Task GetById_Incorrect_ReturnBadRequest()
        {
            int id = -100;
            var response = await this.client.GetByIdAsync(id);

            Assert.Multiple(
                () => Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode),
                () => Assert.False(response.IsSuccessStatusCode));
        }

        [Fact]
        public async Task GetArtsByStreetcodeId_ReturnSuccessStatusCode()
        {
            ArtExtracter.AddStreetcodeArt(this._testStreetcodeContent.Id, this._testArt.Id);
            int streetcodeId = this._testStreetcodeContent.Id;
            var response = await this.client.GetArtsByStreetcodeId(streetcodeId);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<ArtDTO>>(response.Content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
        }

        [Fact]
        public async Task GetArtsByStreetcodeId_Incorrect_BadResult()
        {
            int streetcodeId = -100;

            var response = await this.client.GetArtsByStreetcodeId(streetcodeId);

            Assert.Multiple(
                          () => Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode),
                          () => Assert.False(response.IsSuccessStatusCode));
        }
    }
}
