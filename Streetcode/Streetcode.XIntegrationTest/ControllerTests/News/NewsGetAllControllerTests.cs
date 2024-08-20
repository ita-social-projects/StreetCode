using Streetcode.BLL.DTO.News;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.News;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.News;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.News
{
    public class NewsGetAllControllerTests : BaseControllerTests<NewsClient>, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private DAL.Entities.News.News _testNews;

        public NewsGetAllControllerTests(CustomWebApplicationFactory<Program> factory)
            : base(factory, "/api/News")
        {
            int uniqueId = UniqueNumberGenerator.GenerateInt();
            this._testNews = NewsExtracter.Extract(uniqueId);
        }

        public override void Dispose()
        {
            NewsExtracter.Remove(this._testNews);
        }

        [Fact]
        public async Task GetAll_ReturnSuccessStatusCode()
        {
            var response = await this.Client.GetAllAsync(1, 10);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<NewsDTO>>(response.Content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
        }

        [Fact]
        public async Task GetAll_PageNumberTooBig_ReturnsEmptyCollection()
        {
            var response = await this.Client.GetAllAsync(999, 10);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<NewsDTO>>(response.Content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
            Assert.Empty(returnedValue);
        }

        [Fact]
        public async Task GetAll_PageSizeIsZero_ReturnsEmptyCollection()
        {
            var response = await this.Client.GetAllAsync(1, 0);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<NewsDTO>>(response.Content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
            Assert.Empty(returnedValue);
        }
    }
}
