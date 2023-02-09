using FluentResults;
using Streetcode.BLL.DTO.AdditionalContent.Coordinates;
using Streetcode.BLL.DTO.AdditionalContent.Coordinates.Types;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.AdditionalContent
{
    public class CoordinateControllerTests: BaseControllerTests, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        public CoordinateControllerTests(CustomWebApplicationFactory<Program> factory):base(factory,"/api/Coordinate")
        {
        }

        [Fact]
        public async Task GetByStreetcodeId_SuccsessStatusCode()
        {
            int streetcodeId = 1;
            var response = await client.GetByStreetcodeId(streetcodeId);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<StreetcodeCoordinateDTO>>(response.Content);

            Assert.NotNull(returnedValue);
            Assert.True(response.IsSuccessStatusCode);
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
