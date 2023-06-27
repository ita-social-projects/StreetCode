using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.AdditionalContent.Subtitle;
using System.Net;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.AdditionalContent
{
    public class SubtitleControllerTests : BaseControllerTests, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        public SubtitleControllerTests(CustomWebApplicationFactory<Program> factory)
            : base(factory, "api/Subtitle")
        {
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

        [Theory]
        [InlineData(1)]
        public async Task GetByStreetcodeId_ReturnSuccess(int streetcodeId)
        {
            var response = await this.client.GetByStreetcodeId(streetcodeId);

            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<SubtitleDTO>(response.Content);

            Assert.True(response.IsSuccessful);
            Assert.NotNull(returnedValue);
            Assert.Equal(streetcodeId, returnedValue.StreetcodeId);
        }
    }
}
