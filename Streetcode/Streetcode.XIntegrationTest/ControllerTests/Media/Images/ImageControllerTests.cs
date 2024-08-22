using Streetcode.BLL.DTO.Media.Images;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Media.Images;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.MediaExtracter.Image;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Media.Images
{
    public class ImageControllerTests : BaseControllerTests<ImageClient>, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly Image testImage;
        private readonly StreetcodeContent testStreetcodeContent;

        public ImageControllerTests(CustomWebApplicationFactory<Program> factory)
            : base(factory, "/api/Image")
        {
            int uniqueId = UniqueNumberGenerator.GenerateInt();
            this.testImage = ImageExtracter.Extract(uniqueId);
            this.testStreetcodeContent = StreetcodeContentExtracter
                .Extract(
                    uniqueId,
                    uniqueId,
                    Guid.NewGuid().ToString());
        }

        public override void Dispose()
        {
            StreetcodeContentExtracter.Remove(this.testStreetcodeContent);
            ImageExtracter.Remove(this.testImage);
        }

        [Fact]
        public async Task GetAllReturn_SuccessStatusCode()
        {
            var response = await this.Client.GetAllAsync();
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<ImageDTO>>(response.Content);

            Assert.Multiple(
                () => Assert.True(response.IsSuccessStatusCode),
                () => Assert.NotNull(returnedValue));
        }

        [Fact]
        public async Task GetById_ReturnSuccessStatusCode()
        {
            Image expectedImage = this.testImage;
            var response = await this.Client.GetByIdAsync(expectedImage.Id);
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
            var response = await this.Client.GetByIdAsync(id);

            Assert.Multiple(
                () => Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode),
                () => Assert.False(response.IsSuccessStatusCode));
        }

        [Fact]
        public async Task GetByStreetcodeId_ReturnSuccessStatusCode()
        {
            ImageExtracter.AddStreetcodeImage(this.testStreetcodeContent.Id, this.testImage.Id);
            int streetcodeId = this.testStreetcodeContent.Id;
            var response = await this.Client.GetByStreetcodeId(streetcodeId);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<ImageDTO>>(response.Content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
        }

        [Fact]
        public async Task GetByStreetcodeId_Incorrect_ReturnBadRequest()
        {
            int streetcodeId = -100;
            var response = await this.Client.GetByStreetcodeId(streetcodeId);

            Assert.Multiple(
              () => Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode),
              () => Assert.False(response.IsSuccessStatusCode));
        }
    }
}
