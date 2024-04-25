using Streetcode.BLL.DTO.Media.Art;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.Media.Image;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Media.Images;
using Xunit;
using Streetcode.XIntegrationTest.Base;

namespace Streetcode.XIntegrationTest.ControllerTests.Media.Images
{
    public class StreetcodeArtControllerTests : BaseControllerTests<StreetcodeArtClient>, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private Art _testArt;
        private StreetcodeContent _testStreetcodeContent;

        public StreetcodeArtControllerTests(CustomWebApplicationFactory<Program> factory)
            : base(factory, "/api/StreetcodeArt")
        {
            int uniqueId = UniqueNumberGenerator.GenerateInt();
            this._testArt = ArtExtracter.Extract(uniqueId);
            this._testStreetcodeContent = StreetcodeContentExtracter
                .Extract(
                    uniqueId,
                    uniqueId,
                    Guid.NewGuid().ToString());
        }

        public override void Dispose()
        {
            StreetcodeContentExtracter.Remove(this._testStreetcodeContent);
            ArtExtracter.Remove(this._testArt);
        }

        [Fact]
        public async Task GetByStreetcodeId_ReturnSuccessStatusCode()
        {
            ArtExtracter.AddStreetcodeArt(this._testStreetcodeContent.Id, this._testArt.Id);
            int streetcodeId = this._testStreetcodeContent.Id;
            var response = await this.client.GetByStreetcodeId(streetcodeId);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<StreetcodeArtDTO>>(response.Content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
        }

        [Fact]
        public async Task GetByStreetcodeId_Incorrect_ReturnBadRequest()
        {
            int streetcodeId = -100;
            var response = await this.client.GetByStreetcodeId(streetcodeId);

            Assert.Multiple(
                () => Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode),
                () => Assert.False(response.IsSuccessStatusCode));
        }
    }
}
