using System.Net;
using Streetcode.BLL.DTO.Media.Audio;
using Streetcode.DAL.Entities.Media;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.AuthorizationFixture;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Media;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Media;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.Media.Audio;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Media
{
    [Collection("Authorization")]
    public class AudioControllerTests : BaseAuthorizationControllerTests<AudioClient>, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly Audio testAudio;
        private readonly StreetcodeContent testStreetcodeContent;

        public AudioControllerTests(CustomWebApplicationFactory<Program> factory, TokenStorage tokenStorage)
            : base(factory, "/api/Audio", tokenStorage)
        {
            int uniqueId = UniqueNumberGenerator.GenerateInt();
            this.testAudio = AudioExtracter.Extract(uniqueId);
            this.testStreetcodeContent = StreetcodeContentExtracter
                .Extract(
                    uniqueId,
                    uniqueId,
                    Guid.NewGuid().ToString(),
                    audio: this.testAudio);
        }

        [Fact]
        public async Task GetAll_ReturnSuccessStatusCode()
        {
            var response = await this.Client.GetAllAsync();
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<AudioDTO>>(response.Content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
        }

        [Fact]
        public async Task GetById_ReturnSuccessStatusCode()
        {
            Audio expected = this.testAudio;
            var response = await this.Client.GetByIdAsync(expected.Id);

            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<AudioDTO>(response.Content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
            Assert.Equal(expected.Id, returnedValue.Id);
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
            int streetcodeId = this.testStreetcodeContent.Id;
            int audioId = this.testAudio.Id;
            var response = await this.Client.GetByStreetcodeId(streetcodeId);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<AudioDTO>(response.Content);
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
            Assert.True(returnedValue.Id == audioId);
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

        [Fact]
        public async Task GetBaseAudio_ReturnSuccessStatusCode()
        {
            int existentId = testAudio.Id;

            var response = await Client.GetBaseAudio(existentId);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetBaseAudio_Incorrect_ReturnBadRequest()
        {
            int nonExistentId = -100;

            var response = await Client.GetBaseAudio(nonExistentId);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        [ExtractTestAudio]
        public async Task Create_ReturnSuccessStatusCode()
        {
            var audioCreateDto = ExtractTestAudioAttribute.AudioCreateDtoForTest;

            var response = await Client.CreateAudio(audioCreateDto, TokenStorage.AdminAccessToken);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        [ExtractTestAudio]
        public async Task Update_ReturnSuccessStatusCode()
        {
            var audioUpdateDto = ExtractTestAudioAttribute.AudioUpdateDtoForTest;
            audioUpdateDto.Id = this.testAudio.Id;

            var response = await Client.UpdateAudio(audioUpdateDto, TokenStorage.AdminAccessToken);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        [ExtractTestAudio]
        public async Task Update_IncorrectId_ReturnBadRequest()
        {
            var audioUpdateDto = ExtractTestAudioAttribute.AudioUpdateDtoForTest;
            audioUpdateDto.Id = -1;

            var response = await Client.UpdateAudio(audioUpdateDto, TokenStorage.AdminAccessToken);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Delete_ReturnSuccessStatusCode()
        {
            int existentId = testAudio.Id;

            var response = await Client.DeleteAudio(existentId, TokenStorage.AdminAccessToken);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Delete_IncorrectId_ReturnBadRequest()
        {
            int nonExistentId = -1;

            var response = await Client.DeleteAudio(nonExistentId, TokenStorage.AdminAccessToken);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                StreetcodeContentExtracter.Remove(this.testStreetcodeContent);
                AudioExtracter.Remove(this.testAudio);
            }

            base.Dispose(disposing);
        }
    }
}
