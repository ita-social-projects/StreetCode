using Streetcode.BLL.DTO.Media.Images;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Media.Images
{
    public class ImageControllerTests : BaseControllerTests, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        public ImageControllerTests(CustomWebApplicationFactory<Program> factory) : base(factory,"/api/Image")
        {

        }
        [Fact]
        public async Task GetAllReturn_SuccessStatusCode()
        {
            var response = await this.client.GetAllAsync();
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<ImageDTO>>(response.Content);

            Assert.Multiple(
                () => Assert.True(response.IsSuccessStatusCode),
                () => Assert.NotNull(returnedValue));

        }

        [Fact]
        public async Task GetById_ReturnSuccessStatusCode()
        {
            int id = 1;
            var response = await this.client.GetByIdAsync(id);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<ImageDTO>(response.Content);

            Assert.True(response.IsSuccessStatusCode);
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
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<ImageDTO>>(response.Content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
            Assert.True(returnedValue.All(i => i.Streetcodes.Any(s => s.Id == streetcodeId)));
        }

        public async Task GetByStreetcodeId_Incorrect_ReturnBadRequest()
        {
            int streetcodeId = -100;
            var response = await client.GetByStreetcodeId(streetcodeId);

            Assert.Multiple(
              () => Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode),
              () => Assert.False(response.IsSuccessStatusCode));
        }

    }
}
