using Streetcode.BLL.DTO.Media;
using Streetcode.BLL.DTO.Media.Audio;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Media.Video;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Media
{
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
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<VideoDTO>>(response.Content);
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
        }

        [Fact]
        [ExtractTestVideo]
        public async Task GetById_ReturnSuccessStatusCode()
        {
            int id = ExtractTestVideo.VideoForTest.Id;
            var response = await client.GetByIdAsync(id);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<VideoDTO>(response.Content);

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

        [Fact]
        [ExtractTestVideo]
        public async Task GetByStreetcodeId_ReturnSuccessStatusCode()
        {
            int streetcodeId = ExtractTestVideo.StreetcodeWithVideo.Id;
            int videoId = ExtractTestVideo.VideoForTest.Id;
            var response = await client.GetByStreetcodeId(streetcodeId);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<VideoDTO>(response.Content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
            Assert.Multiple(
                () => Assert.True(returnedValue.StreetcodeId == streetcodeId),
                () => Assert.True(returnedValue.Id == videoId));
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
