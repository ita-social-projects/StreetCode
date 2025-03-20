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
    [Collection("StreetcodeArt")]
    public class StreetcodeArtControllerTests : BaseControllerTests<StreetcodeArtClient>, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly Art testArt;
        private readonly StreetcodeContent testStreetcodeContent;

        public StreetcodeArtControllerTests(CustomWebApplicationFactory<Program> factory)
            : base(factory, "/api/StreetcodeArt")
        {
            int uniqueId = UniqueNumberGenerator.GenerateInt();
            this.testStreetcodeContent = StreetcodeContentExtracter
                .Extract(
                    uniqueId,
                    uniqueId,
                    Guid.NewGuid().ToString());
            this.testArt = ArtExtracter.Extract(uniqueId, uniqueId);
        }

        [Fact]
        public async Task GetByStreetcodeId_ReturnSuccessStatusCode()
        {
            ArtExtracter.AddStreetcodeArt(this.testStreetcodeContent.Id, this.testArt.Id);
            int streetcodeId = this.testStreetcodeContent.Id;
            var response = await this.Client.GetByStreetcodeId(streetcodeId);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<StreetcodeArtDTO>>(response.Content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
        }

        [Fact]
        public async Task GetByStreetcodeId_Incorrect_ReturnBadRequest()
        {
            int streetcodeId = -100;
            var response = await this.Client.GetByStreetcodeId(streetcodeId);

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
            }

            base.Dispose(disposing);
        }
    }
}
