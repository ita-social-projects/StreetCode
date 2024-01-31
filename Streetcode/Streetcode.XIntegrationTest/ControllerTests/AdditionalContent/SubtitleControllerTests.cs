using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.AdditionalContent.Subtitle;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Streetcode;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter;
using System.Net;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.AdditionalContent
{
    public class SubtitleControllerTests : BaseControllerTests, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private StreetcodeContent _testStreetcodeContent;

        public SubtitleControllerTests(CustomWebApplicationFactory<Program> factory)
            : base(factory, "api/Subtitle")
        {
            this._testStreetcodeContent = StreetcodeContentExtracter
                .Extract(this.GetHashCode(), this.GetHashCode(), Guid.NewGuid().ToString());
        }

        public override void Dispose()
        {
            StreetcodeContentExtracter.Remove(this._testStreetcodeContent);
        }

        [Fact]
        public async Task GetAll_ReturnSuccess()
        {
            var response = await this.client.GetAllAsync();
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<SubtitleDTO>>(response.Content);
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
        }

        [Fact]
        [ExtractTestSubtitle]
        public async Task GetById_ReturnSuccessContent()
        {
            int streetcodeId = this._testStreetcodeContent.Id;
            Subtitle expectedSubtitle = ExtractTestSubtitle.SubtitleForTest;

            var response = await this.client.GetByIdAsync(expectedSubtitle.Id);

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
            var response = await this.client.GetByIdAsync(incorrectId);

            Assert.Multiple(
                () => Assert.False(response.IsSuccessStatusCode),
                () => Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode));
        }

        [Fact]
        [ExtractTestSubtitle]
        public async Task GetByStreetcodeId_ReturnSuccess()
        {
            int streetcodeId = this._testStreetcodeContent.Id;
            var response = await this.client.GetByStreetcodeId(streetcodeId);

            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<SubtitleDTO>(response.Content);

            Assert.True(response.IsSuccessful);
            Assert.NotNull(returnedValue);
            Assert.Equal(streetcodeId, returnedValue.StreetcodeId);
        }
    }
}
