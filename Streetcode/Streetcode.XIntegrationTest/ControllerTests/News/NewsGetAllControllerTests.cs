using Streetcode.BLL.DTO.News;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.News;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.AdditionalContent;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.News;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.News
{
    public class NewsGetAllControllerTests : BaseControllerTests<NewsClient>, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private DAL.Entities.News.News testNews;

        public NewsGetAllControllerTests(CustomWebApplicationFactory<Program> factory)
            : base(factory, "/api/News")
        {
            int uniqueId = UniqueNumberGenerator.GenerateInt();
            this.testNews = NewsExtracter.Extract(uniqueId);
        }

        [Fact]
        public async Task GetAll_ReturnSuccessStatusCode()
        {
            var response = await this.Client.GetAllAsync(1, 10);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<GetAllNewsResponseDTO>(response.Content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
        }

        [Fact]
        public async Task GetAll_PageNumberTooBig_ReturnsEmptyCollection()
        {
            var response = await this.Client.GetAllAsync(999, 10);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<GetAllNewsResponseDTO>(response.Content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
            Assert.Empty(returnedValue.News);
        }

        [Fact]
        public async Task GetAll_PageSizeIsZero_ReturnsEmptyCollection()
        {
            var response = await this.Client.GetAllAsync(1, 0);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<GetAllNewsResponseDTO>(response.Content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
            Assert.Empty(returnedValue.News);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                NewsExtracter.Remove(this.testNews);
            }

            base.Dispose(disposing);
        }
    }
}
