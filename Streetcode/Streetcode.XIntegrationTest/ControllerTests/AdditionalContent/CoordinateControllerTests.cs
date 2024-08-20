using Streetcode.BLL.DTO.AdditionalContent.Coordinates.Types;
using Streetcode.DAL.Entities.AdditionalContent.Coordinates.Types;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Additional;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.AdditionalContent;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.AdditionalContent
{
    public class CoordinateControllerTests : BaseControllerTests<CoordinateClient>, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly StreetcodeContent _testStreetcodeContent;
        private readonly StreetcodeCoordinate _testStreetcodeCoordinate;

        public CoordinateControllerTests(CustomWebApplicationFactory<Program> factory)
            : base(factory, "/api/Coordinate")
        {
            int uniqueId = UniqueNumberGenerator.GenerateInt();
            this._testStreetcodeContent = StreetcodeContentExtracter
                .Extract(
                uniqueId,
                uniqueId,
                Guid.NewGuid().ToString());
            this._testStreetcodeCoordinate = CoordinateExtracter.Extract(uniqueId, this._testStreetcodeContent.Id);
        }

        public override void Dispose()
        {
            StreetcodeContentExtracter.Remove(this._testStreetcodeContent);
            CoordinateExtracter.Remove(this._testStreetcodeCoordinate);
        }

        [Fact]
        public async Task GetByStreetcodeId_SuccsessStatusCode()
        {
            int streetcodeId = this._testStreetcodeContent.Id;
            var response = await this.Client.GetByStreetcodeId(streetcodeId);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<StreetcodeCoordinateDTO>>(response.Content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
            Assert.True(returnedValue.All(c => c.StreetcodeId == streetcodeId));
        }

        [Fact]
        public async Task GetByStreetcodeId_DoesntExist_ReturnBadRequest()
        {
            int invalidStreetcodeId = -10;
            var response = await this.Client.GetByStreetcodeId(invalidStreetcodeId);
            Assert.False(response.IsSuccessStatusCode);
        }
    }
}
