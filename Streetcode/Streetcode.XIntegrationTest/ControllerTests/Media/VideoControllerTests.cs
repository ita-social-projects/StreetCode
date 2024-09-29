using Streetcode.BLL.DTO.Media;
using Streetcode.DAL.Entities.Media;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Media;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.AdditionalContent;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.Media.Video;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Media
{
    public class VideoControllerTests : BaseControllerTests<VideoClient>, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly Video testVideo;

        public VideoControllerTests(CustomWebApplicationFactory<Program> factory)
            : base(factory, "/api/Video")
        {
            int uniqueId = UniqueNumberGenerator.GenerateInt();
            this.testVideo = VideoExtracter.Extract(uniqueId);
        }

        [Fact]
        public async Task GetAll_ReturnSuccessStatusCode()
        {
            var response = await this.Client.GetAllAsync();
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<VideoDTO>>(response.Content);
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
        }

        [Fact]
        public async Task GetById_ReturnSuccessStatusCode()
        {
            int id = this.testVideo.Id;
            var response = await this.Client.GetByIdAsync(id);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<VideoDTO>(response.Content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
            Assert.Equal(id, returnedValue.Id);
        }

        [Fact]
        public async Task GetById_Incorrect_ReturnBadRequest()
        {
            int id = -100;
            var response = await this.Client.GetByIdAsync(id);

            Assert.Multiple(
                () => Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode),
                () => Assert.False(response.IsSuccessStatusCode));
        }

        [Fact]
        public async Task GetByStreetcodeId_ReturnSuccessStatusCode()
        {
            int streetcodeId = this.testVideo.StreetcodeId;
            int videoId = this.testVideo.Id;
            var response = await this.Client.GetByStreetcodeId(streetcodeId);
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
            var response = await this.Client.GetByStreetcodeId(streetcodeId);

            Assert.Multiple(
                () => Assert.False(response.IsSuccessStatusCode),
                () => Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                VideoExtracter.Remove(this.testVideo);
            }

            base.Dispose(disposing);
        }
    }
}
