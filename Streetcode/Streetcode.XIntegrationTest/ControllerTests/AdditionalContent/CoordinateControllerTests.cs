namespace Streetcode.XIntegrationTest.ControllerTests.AdditionalContent
{
    using Streetcode.BLL.DTO.AdditionalContent.Coordinates.Types;
    using Streetcode.XIntegrationTest.ControllerTests.Utils;
    using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Streetcode;
    using Xunit;

    public class CoordinateControllerTests : BaseControllerTests, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        public CoordinateControllerTests(CustomWebApplicationFactory<Program> factory)
            : base(factory, "/api/Coordinate")
        {
        }

        [Fact]
        [ExtractTestStreetcode]
        public async Task GetByStreetcodeId_SuccsessStatusCode()
        {
            int streetcodeId = ExtractTestStreetcode.StreetcodeForTest.Id;
            var response = await client.GetByStreetcodeId(streetcodeId);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<StreetcodeCoordinateDTO>>(response.Content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
            Assert.True(returnedValue.All(c => c.StreetcodeId == streetcodeId));
        }

        [Fact]
        public async Task GetByStreetcodeId_DoesntExist_ReturnBadRequest()
        {
            int invalidStreetcodeId = -10;
            var response = await this.client.GetByStreetcodeId(invalidStreetcodeId);
            Assert.False(response.IsSuccessStatusCode);
        }
    }
}
