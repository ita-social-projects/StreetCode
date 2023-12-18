using global::Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.AdditionalContent.Tag;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Streetcode;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.AdditionalContent
{
    public class TagControllerTests : BaseControllerTests, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        public TagControllerTests(CustomWebApplicationFactory<Program> factory)
            : base(factory, "/api/Tag")
        {

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
        [ExtractTestTag]
        public async Task GetById_ReturnSuccessStatusCode()
        {
            Tag expectedTag = ExtractTestTag.TagForTest;
            var response = await client.GetByIdAsync(expectedTag.Id);

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
        [ExtractTestStreetcode]
        [ExtractTestTag]
        public async Task GetByStreetcodeId_ReturnSuccessStatusCode()
        {
            StreetcodeTagIndexSetup.Setup(ExtractTestStreetcode.StreetcodeForTest, ExtractTestTag.TagForTest);
            int streetcodeId = ExtractTestStreetcode.StreetcodeForTest.Id;
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
        [ExtractTestTag]
        public async Task GetByTitle_ReturnSuccessStatusCode()
        {

            Tag expectedTag = ExtractTestTag.TagForTest;
            var response = await client.GetResponse($"/GetTagByTitle/{expectedTag.Title}");
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<TagDTO>(response.Content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
            Assert.Multiple(
                () => Assert.Equal(expectedTag.Id, returnedValue?.Id),
                () => Assert.Equal(expectedTag.Title, returnedValue?.Title));
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
