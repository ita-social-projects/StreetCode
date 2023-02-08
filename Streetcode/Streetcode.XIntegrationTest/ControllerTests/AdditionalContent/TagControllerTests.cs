using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.AdditionalContent
{
    public class TagControllerTests :BaseControllerTests, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        public TagControllerTests(CustomWebApplicationFactory<Program> factory) : base(factory, "/api/Tag")
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
        public async Task GetById_ReturnSuccessStatusCode()
        {
            int id = 1;
            var response = await client.GetByIdAsync(id);

            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<TagDTO>(response.Content);

            Assert.Multiple(
                () => Assert.Equal(id, returnedValue?.Id),
                () => Assert.True(response.IsSuccessStatusCode));
        }

        [Fact]
        public async Task GetByIdIncorrect_ReturnBadRequest()
        {
            int id = -100;
            var response = await client.GetByIdAsync(id);

            Assert.Multiple(
                () => Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode),
                () => Assert.False(response.IsSuccessStatusCode));
        }

        //dont return streetcodeenumerable
        [Theory]
        [InlineData(1)]
        public async Task GetByStreetcodeId_ReturnSuccessStatusCode(int streetcodeId)
        {
            var response = await client.GetByStreetcodeId(streetcodeId);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<TagDTO>>(response.Content);

            Assert.Multiple(
                () => Assert.NotNull(returnedValue),
                () => Assert.True(returnedValue.All(t => t.Streetcodes.Any(s => s.Id == streetcodeId))),
                () => Assert.True(response.IsSuccessStatusCode));
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

        //return only with the equaltitle
        [Theory]
        [InlineData("writer")]
        public async Task GetByTitle_ReturnSuccessStatusCode(string title)
        {
            var response = await client.GetResponse($"/GetTagByTitle/{title}");
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<TagDTO>(response.Content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
            Assert.Equal(returnedValue.Title, title);
        }

        [Fact]
        public async Task GetByTitle_Incorrect_ReturnBadRequest()
        {
            string title = "Some_Incorrect_Title";
            var response = await client.GetResponse($"/GetTagByTitle/{title}");
            Assert.False(response.IsSuccessStatusCode);
        }
    }
}
