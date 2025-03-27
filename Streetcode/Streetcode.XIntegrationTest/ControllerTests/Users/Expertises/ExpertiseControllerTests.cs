using Streetcode.BLL.DTO.Users.Expertise;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Users.Expertises;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Users.Expertises;

public class ExpertiseControllerTests : BaseControllerTests<ExpertiseClient>
{
    public ExpertiseControllerTests(CustomWebApplicationFactory<Program> factory)
        : base(factory, "/api/Expertises")
    {
    }

    [Fact]
    public async Task GetAll_Expertises_ReturnsSuccessStatusCode()
    {
        // Act
        var response = await Client.GetAll();
        var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<List<ExpertiseDTO>>(response.Content);

        // Assert
        Assert.True(response.IsSuccessStatusCode);
        Assert.NotNull(returnedValue);
    }
}