using Streetcode.BLL.DTO.Media.Art;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Media.Images;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.Media.Image;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter;
using Xunit;
using static Streetcode.DAL.Entities.Streetcode.StreetcodeArtSlide;

namespace Streetcode.XIntegrationTest.ControllerTests.StreetcodeArtSlide.GetPageByStreetcodeId;

public class StreetcodeArtSlideControllerTests : BaseControllerTests<StreetcodeArtSlideClient>, IClassFixture<CustomWebApplicationFactory<Program>>
{
    private DAL.Entities.Streetcode.StreetcodeArtSlide _testArtSlide;
    private StreetcodeContent _testStreetcodeContent;

    public StreetcodeArtSlideControllerTests(CustomWebApplicationFactory<Program> factory) : base(factory, "/api/StreetcodeArtSlide")
    {
        int uniqueId = UniqueNumberGenerator.Generate();
        this._testArtSlide = StreetcodeArtSlideExtracter.Extract(uniqueId);
        this._testStreetcodeContent = StreetcodeContentExtracter
            .Extract(
                uniqueId,
                uniqueId,
                Guid.NewGuid().ToString());
    }

    public override void Dispose()
    {
        StreetcodeContentExtracter.Remove(this._testStreetcodeContent);
        StreetcodeArtSlideExtracter.Remove(this._testArtSlide);
    }

    [Fact]
    public async Task GetPageByStreetcodeId_Returns_Success()
    {
        StreetcodeArtSlideExtracter.AddStreetcodeArtSlide(this._testStreetcodeContent.Id, this._testArtSlide.Id, this._testArtSlide.Index);
        int streetcodeId = this._testStreetcodeContent.Id;
        int fromSlideN = 1;
        int amountOfSlides = 1;
        var response = await this.client.GetPageByStreetcodeId(streetcodeId, fromSlideN, amountOfSlides);
        var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<StreetcodeArtDTO>>(response.Content);

        Assert.True(response.IsSuccessStatusCode);
        Assert.NotNull(returnedValue);
    }
}