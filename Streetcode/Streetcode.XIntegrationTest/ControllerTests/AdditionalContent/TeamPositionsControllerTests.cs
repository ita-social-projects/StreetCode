using Streetcode.BLL.DTO.Team;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Team;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Additional;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.AdditionalContent;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.AdditionalContent;

[Collection("Authorization")]
public class TeamPositionsControllerTests : BaseAuthorizationControllerTests<TeamPositionsClient>,
    IClassFixture<CustomWebApplicationFactory<Program>>
{
    private Positions _testCreatePosition;
    private Positions _testUpdatePosition;
    private StreetcodeContent _testStreetcodeContent;

    public TeamPositionsControllerTests(CustomWebApplicationFactory<Program> factory, TokenStorage tokenStorage)
        : base(factory, "/api/Position", tokenStorage)
    {
        int uniqueId = UniqueNumberGenerator.GenerateInt();
        this._testCreatePosition = TeamPositionsExtracter.Extract(uniqueId);
        this._testUpdatePosition = TeamPositionsExtracter.Extract(uniqueId);
        this._testStreetcodeContent = StreetcodeContentExtracter
            .Extract(
                uniqueId,
                uniqueId,
                Guid.NewGuid().ToString());
    }

    public override void Dispose()
    {
        StreetcodeContentExtracter.Remove(this._testStreetcodeContent);
        TeamPositionsExtracter.Remove(this._testCreatePosition);
    }

    [Fact]
    public async Task GetAll_ReturnSuccessStatusCode()
    {
        var response = await this.client.GetAllAsync();
        var returnedValue =
            CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<PositionDTO>>(response.Content);

        Assert.True(response.IsSuccessStatusCode);
        Assert.NotNull(returnedValue);
    }
}