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
            int id = 1;
            var response = await client.GetByIdAsync(id);

            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task GetByStreetcodeId_ReturnExpectedContent()
        {
            int id = 1;
            var response = await client.GetByStreetcodeId(id);
            var content = response.Content;
            if (response.IsSuccessStatusCode)
            {
                var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<StreetcodeCoordinateDTO>>(response.Content);

                Assert.NotNull(returnedValue);

                if (returnedValue.Count() != 0)
                {
                    Assert.Equal(id, returnedValue.FirstOrDefault(c => c.StreetcodeId == id)?.StreetcodeId);
                }
            }
        }

        [Fact]
        public async Task GetByStreetcodeId_DoesntExist_ReturnBadRequest()
        {
            var response = await client.GetByStreetcodeId(-10);
            Assert.False(response.IsSuccessStatusCode);
        }
    }
}
