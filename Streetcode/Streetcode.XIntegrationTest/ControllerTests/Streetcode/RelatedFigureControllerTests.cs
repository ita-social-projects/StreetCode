using System.Net;
using Streetcode.BLL.DTO.Streetcode.RelatedFigure;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Streetcode.RelatedFigure;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.StreetCode;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.AdditionalContent;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter.RelatedFigure;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Streetcode;

[Collection("Authorization")]
public class RelatedFigureControllerTests : BaseAuthorizationControllerTests<RelatedFigureClient>
{
    private readonly StreetcodeContent _testStreetcodeContent1;
    private readonly StreetcodeContent _testStreetcodeContent2;
    private readonly RelatedFigure _testRelatedFigure;
    private readonly Tag _testTag;

    public RelatedFigureControllerTests(
        CustomWebApplicationFactory<Program> factory,
        TokenStorage tokenStorage)
        : base(factory, "/api/RelatedFigure", tokenStorage)
    {
         int observerId = UniqueNumberGenerator.GenerateInt();
         int targetId = UniqueNumberGenerator.GenerateInt();
         _testStreetcodeContent1 = StreetcodeContentExtracter
             .Extract(
                 observerId,
                 observerId,
                 Guid.NewGuid().ToString());
         _testStreetcodeContent2 = StreetcodeContentExtracter
             .Extract(
                 targetId,
                 targetId,
                 Guid.NewGuid().ToString());
         _testRelatedFigure = RelatedFigureExtracter.Extract(_testStreetcodeContent1.Id, _testStreetcodeContent2.Id);
         _testTag = TagExtracter.Extract(targetId, Guid.NewGuid().ToString());
    }

    [Fact]
    public async Task GetByStreetcodeId_ReturnSuccessStatusCode()
    {
        // Act
        var response = await Client.GetByStreetcodeId(_testStreetcodeContent1.Id);
        var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<RelatedFigureDTO>>(response.Content);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
        });
    }

    [Fact]
    public async Task GetByStreetcodeId_Incorrect_ReturnsBadRequest()
    {
        // Arrange
        int incorrectId = -100;

        // Act
        var response = await Client.GetByStreetcodeId(incorrectId);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.False(response.IsSuccessStatusCode);
        });
    }

    [Fact]
    public async Task GetByTagId_ReturnSuccessStatusCode()
    {
        // Arrange
        TagExtracter.AddStreetcodeTagIndex(_testStreetcodeContent1.Id, _testTag.Id);

        // Act
        var response = await Client.GetByTagId(_testTag.Id);
        var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<RelatedFigureDTO>>(response.Content);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
        });
    }

    [Fact]
    public async Task GetByTagId_Incorrect_ReturnEmptyList()
    {
        // Arrange
        int incorrectId = -100;

        // Act
        var response = await Client.GetByTagId(incorrectId);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("[]", response.Content);
        });
    }

    [Fact]
    [ExtractCreateTestRelatedFigure]
    public async Task CreateRelatedFigure_ReturnsSuccessStatusCode()
    {
        // Arrange
        var observerId = ExtractCreateTestRelatedFigureAttribute.StreetcodeContent1.Id;
        var targetId = ExtractCreateTestRelatedFigureAttribute.StreetcodeContent2.Id;

        // Act
        var response = await Client.Create(observerId, targetId, TokenStorage.AdminAccessToken);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("{}", response.Content);
        });
    }

    [Fact]
    public async Task Create_TokenNotPassed_ReturnsUnauthorized()
    {
        // Act
        var response = await Client.Create(_testStreetcodeContent1.Id, _testStreetcodeContent2.Id);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Create_NotAdminTokenPassed_ReturnsForbidden()
    {
        // Act
        var response = await Client.Create(_testStreetcodeContent1.Id, _testStreetcodeContent2.Id, TokenStorage.UserAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    [ExtractDeleteTestRelatedFigure]
    public async Task DeleteRelatedFigure_ReturnsSuccessStatusCode()
    {
        // Arrange
        var observerId = ExtractDeleteTestRelatedFigureAttribute.StreetcodeContent1.Id;
        var targetId = ExtractDeleteTestRelatedFigureAttribute.StreetcodeContent2.Id;

        // Act
        var response = await Client.Delete(observerId, targetId, TokenStorage.AdminAccessToken);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("{}", response.Content);
        });
    }

    [Fact]
    public async Task Delete_TokenNotPassed_ReturnsUnauthorized()
    {
        // Act
        var response = await Client.Delete(_testStreetcodeContent1.Id, _testStreetcodeContent2.Id);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Delete_NotAdminTokenPassed_ReturnsForbidden()
    {
        // Act
        var response = await Client.Delete(_testStreetcodeContent1.Id, _testStreetcodeContent2.Id, TokenStorage.UserAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            TagExtracter.Remove(_testTag);
            RelatedFigureExtracter.Remove(_testRelatedFigure);
            StreetcodeContentExtracter.Remove(_testStreetcodeContent1);
            StreetcodeContentExtracter.Remove(_testStreetcodeContent2);
        }

        base.Dispose(disposing);
    }
}