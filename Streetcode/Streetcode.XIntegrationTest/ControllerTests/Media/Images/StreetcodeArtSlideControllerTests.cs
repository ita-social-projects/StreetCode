using System.Net;
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
    public class StreetcodeArtSlideControllerTests : BaseControllerTests<StreetcodeArtSlideClient>,
        IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly Art testArt;
        private readonly StreetcodeContent testStreetcodeContent;

        public StreetcodeArtSlideControllerTests(CustomWebApplicationFactory<Program> factory)
            : base(factory, "/api/StreetcodeArt")
        {
            int uniqueArtId = UniqueNumberGenerator.GenerateInt();
            this.testStreetcodeContent = StreetcodeContentExtracter
                .Extract(
                    uniqueArtId,
                    uniqueArtId,
                    Guid.NewGuid().ToString());

            int uniqeStreetcodeId = testStreetcodeContent.Id;
            this.testArt = ArtExtracter.Extract(uniqueArtId, uniqeStreetcodeId);
        }

        [Fact]
        public async Task GetPageByStreetcodeId_ReturnSuccessStatusCode()
        {
            int StreetCodeId = (int)testStreetcodeContent.Id;

            ushort fromSlide = 1;
            ushort AmountOfSlides = 2;

            var response = await Client.GetPageByStreetcodeId(StreetCodeId, fromSlide, AmountOfSlides);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetPageByStreetcodeId_WithInvalidData_ReturnsBadRequest()
        {
            int StreetCodeId = -1;
            ushort fromSlide = (ushort?)ushort.MaxValue ?? 0;
            ushort AmountOfSlides = (ushort?)ushort.MaxValue ?? 0;

            var response = await Client.GetPageByStreetcodeId(StreetCodeId, fromSlide, AmountOfSlides);

            Assert.Multiple(
                () => Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode),
                () => Assert.False(response.IsSuccessStatusCode));
        }

        [Fact]
        public async Task GetAllCountByStreetcodeId_ReturnSuccessStatusCode()
        {
            uint streetCodeId = (uint)testStreetcodeContent.Id;

            var response = await Client.GetAllCountByStreetcodeId(streetCodeId);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetAllCountByStreetcodeId_ReturnBadRequestStatusCode()
        {
            uint streetCodeId = 0;

            var response = await Client.GetAllCountByStreetcodeId(streetCodeId);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
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