using Streetcode.BLL.DTO.Media.Art;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Media.Images;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.AdditionalContent;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.Media.Image;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Media.Images
{
    [Collection("Art")]
    public class ArtControllerTests : BaseControllerTests<ArtClient>, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly Art testArt;
        private readonly StreetcodeArtSlide testStreetcodeArtSlide;
        private readonly StreetcodeContent testStreetcodeContent;

        public ArtControllerTests(CustomWebApplicationFactory<Program> factory)
            : base(factory, "/api/Art")
        {
            int uniqueId = UniqueNumberGenerator.GenerateInt();
            this.testArt = ArtExtracter.Extract(1);
            this.testStreetcodeContent = StreetcodeContentExtracter
                .Extract(
                    54,
                    54,
                    Guid.NewGuid().ToString());
            this.testStreetcodeArtSlide = StreetcodeArtSlideExtracter.Extract(uniqueId);
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
            Art expectedArt = this.testArt;
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
            ArtExtracter.AddStreetcodeArtWithStreetcodeArtSlide(this.testStreetcodeContent.Id, this.testArt.Id, this.testStreetcodeArtSlide.Id);
            int streetcodeId = this.testStreetcodeContent.Id;
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                StreetcodeContentExtracter.Remove(this.testStreetcodeContent);
                ArtExtracter.Remove(this.testArt);
                StreetcodeArtSlideExtracter.Remove(this.testStreetcodeArtSlide);
            }

            base.Dispose(disposing);
        }
    }
}
