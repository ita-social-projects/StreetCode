using Streetcode.BLL.DTO.Media.Images;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Media.Images;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.MediaExtracter.Image;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter;
using Xunit;
using Streetcode.XIntegrationTest.Base;

namespace Streetcode.XIntegrationTest.ControllerTests.Media.Images
{
    public class ImageControllerTests : BaseControllerTests<ImageClient>, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private Image _testImage;
        private StreetcodeContent _testStreetcodeContent;

        public ImageControllerTests(CustomWebApplicationFactory<Program> factory)
            : base(factory, "/api/Image")
        {
            int uniqueId = UniqueNumberGenerator.GenerateInt();
            this._testImage = ImageExtracter.Extract(uniqueId);
            this._testStreetcodeContent = StreetcodeContentExtracter
                .Extract(
                    uniqueId,
                    uniqueId,
                    Guid.NewGuid().ToString());
        }

        public override void Dispose()
        {
            StreetcodeContentExtracter.Remove(this._testStreetcodeContent);
            ImageExtracter.Remove(this._testImage);
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
            Image expectedImage = this._testImage;
            var response = await this.client.GetByIdAsync(expectedImage.Id);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<ImageDTO>(response.Content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
            Assert.Multiple(
                () => Assert.Equal(expectedImage.Id, returnedValue.Id),
                () => Assert.Equal(expectedImage.BlobName, returnedValue.BlobName),
                () => Assert.Equal(expectedImage.ImageDetails?.Id, returnedValue.ImageDetails?.Id));
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
        public async Task GetByStreetcodeId_ReturnSuccessStatusCode()
        {
            ImageExtracter.AddStreetcodeImage(this._testStreetcodeContent.Id, this._testImage.Id);
            int streetcodeId = this._testStreetcodeContent.Id;
            var response = await this.client.GetByStreetcodeId(streetcodeId);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<ImageDTO>>(response.Content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
        }

        public async Task GetByStreetcodeId_Incorrect_ReturnBadRequest()
        {
            int streetcodeId = -100;
            var response = await this.client.GetByStreetcodeId(streetcodeId);

            Assert.Multiple(
              () => Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode),
              () => Assert.False(response.IsSuccessStatusCode));
        }
    }
}
