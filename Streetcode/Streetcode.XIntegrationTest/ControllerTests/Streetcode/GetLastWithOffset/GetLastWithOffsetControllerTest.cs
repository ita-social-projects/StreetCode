using Streetcode.BLL.DTO.Streetcode;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Streetcode.GetLastWithOffset;

public class GetLastWithOffsetControllerTest : BaseControllerTests, IClassFixture<CustomWebApplicationFactory<Program>>
{
    private StreetcodeContent _testStreetcodeContent;
    
    public GetLastWithOffsetControllerTest(CustomWebApplicationFactory<Program> factory) 
        : base(factory,"/api/Streetcode")
    {
        this._testStreetcodeContent = StreetcodeContentExtracter
            .Extract(
                this.GetHashCode(),
                this.GetHashCode(),
                Guid.NewGuid().ToString());
    }
    
    public override void Dispose()
    {
        StreetcodeContentExtracter.Remove(this._testStreetcodeContent);
    }

    [Fact]
    public async Task GetLastWithOffsetReturnSuccessStatusCode()
    {
        StreetcodeContent expectedStreetcode = this._testStreetcodeContent;
        int expectedOffset = 0;
        var response = await this.client.GetResponse($"/GetLastWithOffset/{0}");

        var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<StreetcodeMainPageDTO>(response.Content);

        Assert.True(response.IsSuccessStatusCode);
        Assert.NotNull(returnedValue);
        Assert.Multiple(() => Assert.Equal(expectedStreetcode.Id, returnedValue.Id));
    }

    [Fact]
    public async Task GetLastWithOffsetIncorrectReturnBadRequest()
    {
        int expectedOffset = Int32.MaxValue;
        var response = await this.client.GetResponse($"/GetLastWithOffset/{expectedOffset}");
    
        Assert.Multiple(
            () => Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode),
            () => Assert.False(response.IsSuccessStatusCode));
    }
}