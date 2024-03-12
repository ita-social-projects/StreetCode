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
                Guid.NewGuid().ToString(), createdAt: DateTime.MaxValue);

    }
    
    [Fact]
    public async Task GetLastWithOffsetReturnSuccessStatusCode()
    {
        int expectedOffset = 0;
        var result = await this.client.GetResponse($"/GetLastWithOffset/{expectedOffset}");

        var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<StreetcodeMainPageDTO>(result.Content);
        Assert.True(result.IsSuccessStatusCode);
        Assert.NotNull(returnedValue);
        Assert.Equal(_testStreetcodeContent.Id, returnedValue.Id);
    }

    [Fact]
    public async Task GetLastWithOffsetIncorrectReturnBadRequest()
    {
        long expectedOffset = Int64.MaxValue;
        var response = await this.client.GetResponse($"/GetLastWithOffset/{expectedOffset}");

        Assert.Multiple(
            () => Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode),
            () => Assert.False(response.IsSuccessStatusCode));
    }

    public override void Dispose()
    {
        StreetcodeContentExtracter.Remove(this._testStreetcodeContent);
    }
}