using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Team;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Team;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.AdditionalContent;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.MediaExtracter.Image;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Team
{
    [Collection("TickerString")]
    public class TickerStringControllerTests : BaseControllerTests<TickerStringClient>, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly Image _testTeamMemberImage;
        private readonly TeamMember _testTeamMember;
        private readonly Positions _testPosition;

        public TickerStringControllerTests(CustomWebApplicationFactory<Program> factory)
            : base(factory, "api/TickerString")
        {
            int uniqueId = UniqueNumberGenerator.GenerateInt();
            _testTeamMemberImage = ImageExtracter.Extract(uniqueId);
            _testTeamMember = TeamMemberExtracter.Extract(uniqueId, _testTeamMemberImage.Id);
            _testPosition = TeamPositionsExtracter.Extract(uniqueId);
        }

        [Fact]
        public async Task GetNameTickerString_ReturnsSuccessStatusCode()
        {
            // Arrange
            TeamPositionsExtracter.AddTeamMemberPositions(_testTeamMember.Id, _testPosition.Id);

            // Act
            var response = await this.Client.GetNameTickerString();
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<string>(response.Content);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
        }

        
    }
}
