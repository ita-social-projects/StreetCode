using Streetcode.BLL.DTO.AdditionalContent.Coordinates.Types;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.AdditionalContent
{
   public class CoordinateControllerTests : BaseControllerTests, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private StreetcodeContent _testStreetcodeContent;

        public CoordinateControllerTests(CustomWebApplicationFactory<Program> factory)
            : base(factory, "/api/Coordinate")
        {
            this._testStreetcodeContent = StreetcodeContentExtracter
                .Extract(this.GetHashCode(), Guid.NewGuid().ToString());
        }

        public override void Dispose()
        {
            StreetcodeContentExtracter.Remove(this._testStreetcodeContent);
        }

        [Fact]
        public async Task GetByStreetcodeId_SuccsessStatusCode()
        {
            int streetcodeId = this._testStreetcodeContent.Id;
            var response = await this.client.GetByStreetcodeId(streetcodeId);
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
