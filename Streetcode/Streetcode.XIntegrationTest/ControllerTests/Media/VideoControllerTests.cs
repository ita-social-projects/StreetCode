namespace Streetcode.XIntegrationTest.ControllerTests.Media
{
    using Streetcode.BLL.DTO.Media;
    using Streetcode.BLL.DTO.Media.Audio;
    using Streetcode.XIntegrationTest.ControllerTests.Utils;
    using Xunit;

    public class VideoControllerTests : BaseControllerTests, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        public VideoControllerTests(CustomWebApplicationFactory<Program> factory)
            : base(factory, "/api/Video")
        {
        }

        [Fact]
        public async Task GetAll_ReturnSuccessStatusCode()
        {
            var response = await client.GetAllAsync();
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<AudioDTO>>(response.Content);
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
        }

        [Fact]
        public async Task GetById_ReturnSuccessStatusCode()
        {
            int id = 1;
            var response = await client.GetByIdAsync(id);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<AudioDTO>(response.Content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
            Assert.Equal(id, returnedValue?.Id);
        }

        [Fact]
        public async Task GetById_Incorrect_ReturnBadRequest()
        {
            int id = -100;
            var response = await client.GetByIdAsync(id);

            Assert.Multiple(
                () => Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode),
                () => Assert.False(response.IsSuccessStatusCode));
        }

        [Theory]
        [InlineData(1)]
        public async Task GetByStreetcodeId_ReturnSuccessStatusCode(int streetcodeId)
        {
            var response = await client.GetByStreetcodeId(streetcodeId);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<VideoDTO>(response.Content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
            Assert.True(returnedValue.StreetcodeId == streetcodeId);
        }

        [Fact]
        public async Task GetByStreetcodeId_Incorrect_ReturnBadRequest()
        {
            int streetcodeId = -100;
            var response = await client.GetByStreetcodeId(streetcodeId);

            Assert.Multiple(
                () => Assert.False(response.IsSuccessStatusCode),
                () => Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode));
        }
    }
}
