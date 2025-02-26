using System.Net;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Additional;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.AdditionalContent;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.AdditionalContent
{
    [Collection("Authorization")]
    public class SubtitleControllerTests : BaseControllerTests<SubtitleClient>, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly Subtitle testSubtitle;

        public SubtitleControllerTests(CustomWebApplicationFactory<Program> factory)
            : base(factory, "api/Subtitle")
        {
            int uniqueId = UniqueNumberGenerator.GenerateInt();
            this.testSubtitle = SubtitleExtracter.Extract(uniqueId);
        }

        [Fact]
        public async Task GetAll_ReturnSuccess()
        {
            var response = await this.Client.GetAllAsync();
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<SubtitleDTO>>(response.Content);
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
        }

        [Fact]
        public async Task GetById_ReturnSuccessContent()
        {
            Subtitle expectedSubtitle = this.testSubtitle;

            var response = await this.Client.GetByIdAsync(expectedSubtitle.Id);

            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<SubtitleDTO>(response.Content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
            Assert.Multiple(
                () => Assert.Equal(expectedSubtitle.Id, returnedValue?.Id),
                () => Assert.Equal(expectedSubtitle.SubtitleText, returnedValue?.SubtitleText));
        }

        [Fact]
        public async Task GetById_Incorrect_ReturnBadRequest()
        {
            int incorrectId = -100;
            var response = await this.Client.GetByIdAsync(incorrectId);

            Assert.Multiple(
                () => Assert.False(response.IsSuccessStatusCode),
                () => Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode));
        }

        [Fact]
        public async Task GetByStreetcodeId_ReturnSuccess()
        {
            int streetcodeId = this.testSubtitle.StreetcodeId;
            var response = await this.Client.GetByStreetcodeId(streetcodeId);

            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<SubtitleDTO>(response.Content);

            Assert.True(response.IsSuccessful);
            Assert.NotNull(returnedValue);
            Assert.Equal(streetcodeId, returnedValue.StreetcodeId);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                SubtitleExtracter.Remove(this.testSubtitle);
            }

            base.Dispose(disposing);
        }
    }
}
