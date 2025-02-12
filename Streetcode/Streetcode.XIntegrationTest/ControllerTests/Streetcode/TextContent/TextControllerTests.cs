using System.Net;
using Streetcode.BLL.DTO.Streetcode.TextContent.Text;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.AdditionalContent.Streetcode.TextContent.Texts;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.StreetCode.TextContent;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter.TextContent;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Streetcode.TextContent;

[Collection("Authorization")]
public class TextControllerTests : BaseAuthorizationControllerTests<TextClient>
{
    private readonly Text _testText;

    public TextControllerTests(CustomWebApplicationFactory<Program> factory, TokenStorage tokenStorage)
        : base(factory, "/api/Text", tokenStorage)
    {
        var textId = UniqueNumberGenerator.GenerateInt();
        _testText = TextExtracter.Extract(textId);
    }

    [Fact]
    public async Task GetAll_ShouldReturnSuccessStatusCode_WhenTextsReceived()
    {
        // Act
        var response = await this.Client.GetAllAsync();
        var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<TextDTO>>(response.Content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(returnedValue);
    }

    [Fact]
    public async Task GetById_ShouldReturnSuccessStatusCode_WhenIdIsValid()
    {
        // Arrange
        var textId = _testText.Id;

        // Act
        var response = await this.Client.GetByIdAsync(textId);
        var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<TextDTO>(response.Content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(returnedValue);
        Assert.Multiple(
            () => Assert.Equal(_testText.Id, returnedValue.Id),
            () => Assert.Equal(_testText.Title, returnedValue.Title),
            () => Assert.Equal(_testText.TextContent, returnedValue.TextContent),
            () => Assert.Equal(_testText.AdditionalText, returnedValue.AdditionalText),
            () => Assert.Equal(_testText.StreetcodeId, returnedValue.StreetcodeId));
    }

    [Fact]
    public async Task GetById_ShouldReturnBadRequest_WhenIdIsNotValid()
    {
        // Arrange
        const int textId = int.MinValue;

        // Act
        var response = await this.Client.GetByIdAsync(textId);
        var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<TextDTO>(response.Content);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Null(returnedValue);
    }

    [Fact]
    public async Task GetByStreetcodeId_ShouldReturnSuccessStatusCode_WhenIdIsValid()
    {
        // Arrange
        var textStreetcodeId = _testText.StreetcodeId;

        // Act
        var response = await this.Client.GetByStreetcodeId(textStreetcodeId);
        var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<TextDTO>(response.Content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(returnedValue);
        Assert.Multiple(
            () => Assert.Equal(_testText.Id, returnedValue.Id),
            () => Assert.Equal(_testText.Title, returnedValue.Title),
            () => Assert.Equal(_testText.TextContent, returnedValue.TextContent),
            () => Assert.Equal(_testText.AdditionalText, returnedValue.AdditionalText),
            () => Assert.Equal(_testText.StreetcodeId, returnedValue.StreetcodeId));
    }

    [Fact]
    public async Task GetByStreetcodeId_ShouldReturnBadRequest_WhenIdIsNotValid()
    {
        // Arrange
        const int textStreetcodeId = int.MinValue;

        // Act
        var response = await this.Client.GetByStreetcodeId(textStreetcodeId);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    [ExtractUpdateParsedTextTestText]
    public async Task UpdateParsedText_ShouldReturnSuccessStatusCode_WhenTextsParsed()
    {
        // Arrange
        var textPreview = ExtractUpdateParsedTextTestTextAttribute.TextPreviewUpdateDtoForTest;

        // Act
        var response = await this.Client.UpdateParsedText(textPreview, this.TokenStorage.AdminAccessToken);
        var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<string>(response.Content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(returnedValue);
    }

    [Fact]
    [ExtractUpdateParsedTextTestText]
    public async Task UpdateParsedText_ShouldReturnUnauthorized_WhenTokenIsAbsent()
    {
        // Arrange
        var textPreview = ExtractUpdateParsedTextTestTextAttribute.TextPreviewUpdateDtoForTest;

        // Act
        var response = await this.Client.UpdateParsedText(textPreview);
        var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<string>(response.Content);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.Null(returnedValue);
    }

    [Fact]
    [ExtractUpdateParsedTextTestText]
    public async Task UpdateParsedText_ShouldReturnForbidden_WhenUserIsNotAdmin()
    {
        // Arrange
        var textPreview = ExtractUpdateParsedTextTestTextAttribute.TextPreviewUpdateDtoForTest;

        // Act
        var response = await this.Client.UpdateParsedText(textPreview, this.TokenStorage.UserAccessToken);
        var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<string>(response.Content);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        Assert.Null(returnedValue);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            TextExtracter.Remove(_testText);
        }

        base.Dispose(disposing);
    }
}
