using Streetcode.BLL.DTO.Media.Art;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Media.Images;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.Media.Image;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Media.Images
{
    public class ArtControllerTests : BaseControllerTests<ArtClient>, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly Art _testArt;
        private readonly StreetcodeArtSlide _testStreetcodeArtSlide;
        private readonly StreetcodeContent _testStreetcodeContent;

        public ArtControllerTests(CustomWebApplicationFactory<Program> factory)
            : base(factory, "/api/Art")
        {
            int uniqueId = UniqueNumberGenerator.GenerateInt();
            this._testArt = ArtExtracter.Extract(1);
            this._testStreetcodeContent = StreetcodeContentExtracter
                .Extract(
                    54,
                    54,
                    Guid.NewGuid().ToString());
            this._testStreetcodeArtSlide = StreetcodeArtSlideExtracter.Extract(uniqueId);
        }

        public override void Dispose()
        {
            StreetcodeContentExtracter.Remove(this._testStreetcodeContent);
            ArtExtracter.Remove(this._testArt);
            StreetcodeArtSlideExtracter.Remove(this._testStreetcodeArtSlide);
        }

        [Fact]
        public async Task GetAll_ReturnSuccessStatusCode()
        {
            var response = await this.Client.GetAllAsync();
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<ArtDTO>>(response.Content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
        }

        [Fact]
        public async Task GetById_ReturnSuccessStatusCode()
        {
            Art expectedArt = this._testArt;
            var response = await this.Client.GetByIdAsync(expectedArt.Id);

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
            var response = await this.Client.GetByIdAsync(id);

            Assert.Multiple(
                () => Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode),
                () => Assert.False(response.IsSuccessStatusCode));
        }

        // [Fact(Skip = "There is no Art that has StreetcodeArts, so it will fail without them.")]
        [Fact]
        public async Task GetArtsByStreetcodeId_ReturnSuccessStatusCode()
        {
            ArtExtracter.AddStreetcodeArtWithStreetcodeArtSlide(this._testStreetcodeContent.Id, this._testArt.Id, this._testStreetcodeArtSlide.Id);
            int streetcodeId = this._testStreetcodeContent.Id;
            var response = await this.Client.GetArtsByStreetcodeId(streetcodeId);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<ArtDTO>>(response.Content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
        }

        [Fact]
        public async Task GetArtsByStreetcodeId_Incorrect_BadResult()
        {
            int streetcodeId = -100;

            var response = await this.Client.GetArtsByStreetcodeId(streetcodeId);

            Assert.Multiple(
                          () => Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode),
                          () => Assert.False(response.IsSuccessStatusCode));
        }
    }
}
