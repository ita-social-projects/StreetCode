using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.AdditionalContent;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.AdditionalContent
{
    public class TagControllerTests : BaseControllerTests, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private Tag _testTag;
        private StreetcodeContent _testStreetcodeContent;

        public TagControllerTests(CustomWebApplicationFactory<Program> factory)
            : base(factory, "/api/Tag")
        {
            this._testTag = TagExtracter.Extract(this.GetHashCode());
            this._testStreetcodeContent = StreetcodeContentExtracter
                .Extract(
                    this.GetHashCode(),
                    this.GetHashCode(),
                    Guid.NewGuid().ToString());
        }

        public override void Dispose()
        {
            StreetcodeContentExtracter.Remove(this._testStreetcodeContent);
            TagExtracter.Remove(this._testTag);
        }

        [Fact]
        public async Task GetAll_ReturnSuccessStatusCode()
        {
            var response = await client.GetAllAsync();
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<TagDTO>>(response.Content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
        }

        [Fact]
        public async Task GetById_ReturnSuccessStatusCode()
        {
            Tag expectedTag = this._testTag;
            var response = await this.client.GetByIdAsync(expectedTag.Id);

            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<TagDTO>(response.Content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
            Assert.Multiple(
                () => Assert.Equal(expectedTag.Id, returnedValue?.Id),
                () => Assert.Equal(expectedTag.Title, returnedValue?.Title));
        }

        [Fact]
        public async Task GetByIdIncorrect_ReturnBadRequest()
        {
            int incorrectId = -100;
            var response = await client.GetByIdAsync(incorrectId);

            Assert.Multiple(
                () => Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode),
                () => Assert.False(response.IsSuccessStatusCode));
        }

        [Fact]
        public async Task GetByStreetcodeId_ReturnSuccessStatusCode()
        {
            TagExtracter.AddStreetcodeTagIndex(this._testStreetcodeContent.Id, this._testTag.Id);
            int streetcodeId = this._testStreetcodeContent.Id;
            var response = await client.GetByStreetcodeId(streetcodeId);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<StreetcodeTagDTO>>(response.Content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
        }

        [Fact]
        public async Task GetByStreetcodeId_Incorrect_ReturnBadRequest()
        {
            int streetcodeId = -100;
            var response = await client.GetByStreetcodeId(streetcodeId);

            Assert.Multiple(
                () => Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode),
                () => Assert.False(response.IsSuccessStatusCode));
        }

        [Fact]
        public async Task GetByTitle_ReturnSuccessStatusCode()
        {

            Tag expectedTag = this._testTag;
            var response = await this.client.GetResponse($"/GetTagByTitle/{expectedTag.Title}");
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<TagDTO>(response.Content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
            Assert.Equal(expectedTag.Title, returnedValue?.Title);
        }

        [Fact]
        public async Task GetByTitle_Incorrect_ReturnBadRequest()
        {
            string title = "Some_Incorrect_Title";
            var response = await client.GetResponse($"/GetTagByTitle/{title}");

            Assert.Multiple(
              () => Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode),
              () => Assert.False(response.IsSuccessStatusCode));
        }
    }
}
