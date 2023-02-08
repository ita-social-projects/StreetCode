using Streetcode.BLL.DTO.Media.Images;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Media.Images
{
    public class ArtControllerTests : BaseControllerTests, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        public ArtControllerTests(CustomWebApplicationFactory<Program> factory) : base(factory,"/api/Art")
        {
        }

        [Fact]
        public async Task GetAll_ReturnSuccessStatusCode()
        {
            var response = await client.GetAllAsync();
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<ArtDTO>>(response.Content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
        }

        [Fact]
        public async Task GetById_ReturnSuccessStatusCode()
        {
            int id = 1;
            var response = await this.client.GetByIdAsync(id);

            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<ArtDTO>(response.Content);

            Assert.Equal(id, returnedValue?.Id);
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task GetById_Incorrect_ReturnBadRequest()
        {
            int id = -100;
            var response = await client.GetByIdAsync(-1);

            Assert.Multiple(
                () => Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode),
                () => Assert.False(response.IsSuccessStatusCode));
        }

        [Theory]
        [InlineData(1)]
        public async Task GetByStreetcodeId_ReturnSuccessStatusCode(int streetcodeId)
        {
            var response = await client.GetByStreetcodeId(streetcodeId);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable< ArtDTO>>(response.Content);
            Assert.Multiple(
              () => Assert.True(response.IsSuccessStatusCode),
              () => Assert.NotNull(returnedValue),
              () => Assert.True(returnedValue.All(t => t.Streetcodes.Any(s => s.Id == streetcodeId))));
        }

        [Fact]
        public async Task GetByStreetcodeId_Incorrect_BadResult()
        {
            int streetcodeId = -100;

            var response = await this.client.GetByStreetcodeId(streetcodeId);

            Assert.False(response.IsSuccessStatusCode);
        }
    }
}
