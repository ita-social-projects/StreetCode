using Streetcode.BLL.DTO.Users.Expertise;
using Streetcode.DAL.Entities.Users.Expertise;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Users.Expertises;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.Expertises;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Users.Expertises;

public class ExpertiseControllerTests : BaseControllerTests<ExpertiseClient>
{
    private readonly Expertise _testExpertise;

    public ExpertiseControllerTests(CustomWebApplicationFactory<Program> factory)
        : base(factory, "/api/Expertises")
    {
        int uniqueId = UniqueNumberGenerator.GenerateInt();
        _testExpertise = ExpertiseExtracter.Extract(uniqueId);
    }

    [Fact]
    public async Task GetAll_ReturnSuccessStatusCode()
    {
        // Act
        var response = await Client.GetAll();
        var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<List<ExpertiseDTO>>(response.Content);

        // Assert
        Assert.True(response.IsSuccessStatusCode);
        Assert.NotNull(returnedValue);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            ExpertiseExtracter.Remove(_testExpertise);
        }

        base.Dispose(disposing);
    }
}