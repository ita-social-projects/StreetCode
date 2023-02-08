using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.AdditionalContent
{
    public class SubtitleControllerTests: BaseControllerTests, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        public SubtitleControllerTests(CustomWebApplicationFactory<Program> factory)
            : base(factory, "api/Subtitle")
        {
        }

        [Fact]
        public async Task GetAll_ReturnSuccessStatusCode()
        {
            var response = await this.client.GetAllAsync();
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<SubtitleDTO>>(response.Content);

            Assert.Multiple(
                () => Assert.True(response.IsSuccessStatusCode),
                () => Assert.NotNull(returnedValue),
                () => Assert.Equal(10, returnedValue.Count()));
        }

        [Fact]
        public async Task GetAll_ReturnCorrectContent()
        {
            var response = await this.client.GetAllAsync();

            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<SubtitleDTO>>(response.Content);

            Assert.Multiple(
                () => Assert.NotNull(returnedValue),
                () => Assert.Equal(10, returnedValue.Count()));
        }

        [Theory]
        [InlineData(1)]
        public async Task GetById_ReturnSuccessStatusCode(int id)
        {
            var response = await this.client.GetByIdAsync(id);

            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<SubtitleDTO>(response.Content);

            Assert.Multiple(
                () => Assert.Equal(id, returnedValue?.Id),
                () => Assert.True(response.IsSuccessStatusCode));
        }

        [Fact]
        public async Task GetById_Incorrect_ReturnBadRequest()
        {
            var response = await this.client.GetByIdAsync(-100);
            Assert.False(response.IsSuccessStatusCode);
        }

        [Theory]
        [InlineData(1)]
        public async Task GetByStreetcodeId_ReturnSuccessStatusCode(int streetcodeId)
        {
            var response = await this.client.GetByStreetcodeId(streetcodeId);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<SubtitleDTO>>(response.Content);

            Assert.Multiple(
                () => Assert.NotNull(returnedValue),
                () => Assert.True(returnedValue.All(s => s.StreetcodeId == streetcodeId)),
                () => Assert.True(response.IsSuccessful));
        }
    }
}
