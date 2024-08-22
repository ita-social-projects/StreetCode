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
    public class StreetcodeArtControllerTests : BaseControllerTests<StreetcodeArtClient>, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly Art testArt;
        private readonly StreetcodeContent testStreetcodeContent;

        public StreetcodeArtControllerTests(CustomWebApplicationFactory<Program> factory)
            : base(factory, "/api/StreetcodeArt")
        {
            int uniqueId = UniqueNumberGenerator.GenerateInt();
            this.testArt = ArtExtracter.Extract(uniqueId);
            this.testStreetcodeContent = StreetcodeContentExtracter
                .Extract(
                    uniqueId,
                    uniqueId,
                    Guid.NewGuid().ToString());
        }

        public override void Dispose()
        {
            StreetcodeContentExtracter.Remove(this.testStreetcodeContent);
            ArtExtracter.Remove(this.testArt);
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
    }
}
