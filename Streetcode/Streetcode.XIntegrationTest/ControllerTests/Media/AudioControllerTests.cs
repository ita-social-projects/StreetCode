﻿using Streetcode.BLL.DTO.Media.Audio;
using Streetcode.DAL.Entities.Media;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Media.Audio;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Streetcode;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Media
{
    public class AudioControllerTests : BaseControllerTests, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        public AudioControllerTests(CustomWebApplicationFactory<Program> factory)
            : base(factory, "/api/Audio")
        {
        }

        [Fact]
        public async Task GetAll_ReturnSuccessStatusCode()
        {
            var response = await this.client.GetAllAsync();
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<AudioDTO>>(response.Content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
        }

        [Fact]
        [ExtractTestAudio]
        public async Task GetById_ReturnSuccessStatusCode()
        {
            Audio expected = ExtractTestAudio.AudioForTest;
            var response = await this.client.GetByIdAsync(expected.Id);

            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<AudioDTO>(response.Content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
            Assert.Equal(expected.Id, returnedValue?.Id);
        }

        [Fact]
        public async Task GetById_Incorrect_ReturnBadRequest()
        {
            int id = -100;
            var response = await this.client.GetByIdAsync(id);

            Assert.Multiple(
                () => Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode),
                () => Assert.False(response.IsSuccessStatusCode));
        }

        [Fact]
        [ExtractTestAudio]
        public async Task GetByStreetcodeId_ReturnSuccessStatusCode()
        {
            int streetcodeId = ExtractTestAudio.StreetcodeWithAudio.Id;
            int audioId = ExtractTestAudio.AudioForTest.Id;
            var response = await this.client.GetByStreetcodeId(streetcodeId);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<AudioDTO>(response.Content);
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
            Assert.True(returnedValue.Id == audioId);
        }

        [Fact]
        public async Task GetByStreetcodeId_Incorrect_ReturnBadRequest()
        {
            int streetcodeId = -100;
            var response = await this.client.GetByStreetcodeId(streetcodeId);

            Assert.Multiple(
                () => Assert.False(response.IsSuccessStatusCode),
                () => Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode));
        }
    }
}
