using System.Net;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.AuthorizationFixture;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Streetcode.TextContent.RelatedTerms;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.StreetCode.TextContent;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter.TextContent;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Streetcode.TextContent;

[Collection("Authorization")]
public class RelatedTermControllerTests : BaseAuthorizationControllerTests<RelatedTermClient>
{
    private readonly RelatedTerm _testRelatedTerm;

    public RelatedTermControllerTests(CustomWebApplicationFactory<Program> factory, TokenStorage tokenStorage)
        : base(factory, "/api/RelatedTerm", tokenStorage)
    {
        var relatedTermId = UniqueNumberGenerator.GenerateInt();
        _testRelatedTerm = RelatedTermExtracter.Extract(relatedTermId);
    }

    [Fact]
    public async Task GetByTermId_ShouldReturnSuccessStatusCode_WhenRelatedTermsReceived()
    {
        // Arrange
        var relatedTermTermId = _testRelatedTerm.TermId;

        // Act
        var response = await this.Client.GetByTermId(relatedTermTermId);
        var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<RelatedTermDTO>>(response.Content);
        var returnedValueList = returnedValue!.ToList();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(returnedValue);
        Assert.Multiple(
            () => Assert.Equal(_testRelatedTerm.Id, returnedValueList[0].Id),
            () => Assert.Equal(_testRelatedTerm.Word, returnedValueList[0].Word),
            () => Assert.Equal(_testRelatedTerm.TermId, returnedValueList[0].TermId));
    }

    [Fact]
    public async Task GetByTermId_ShouldReturnSuccessStatusCode_WhenIdIsNotValid()
    {
        // Arrange
        const int relatedTermTermId = int.MinValue;

        // Act
        var response = await this.Client.GetByTermId(relatedTermTermId);
        var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<RelatedTermDTO>>(response.Content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(returnedValue);
        Assert.Empty(returnedValue);
    }

    [Fact]
    [ExtractCreateTestRelatedTerm]
    public async Task Create_ShouldReturnSuccessStatusCode_WhenRelatedTermAdded()
    {
        // Arrange
        var relatedTermCreateDto = ExtractCreateTestRelatedTermAttribute.RelatedTermCreateDtoForTest;

        // Act
        var response = await this.Client.Create(relatedTermCreateDto, this.TokenStorage.AdminAccessToken);
        var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<RelatedTermDTO>(response.Content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(returnedValue);
        Assert.Multiple(
            () => Assert.Equal(relatedTermCreateDto.Word, returnedValue.Word),
            () => Assert.Equal(relatedTermCreateDto.TermId, returnedValue.TermId));
    }

    [Fact]
    [ExtractCreateTestRelatedTerm]
    public async Task Create_ShouldReturnFail_WhenRelatedTermIsInvalid()
    {
        // Arrange
        var relatedTermCreateDto = ExtractCreateTestRelatedTermAttribute.RelatedTermCreateDtoForTest;
        relatedTermCreateDto.Word = null!;

        // Act
        var response = await this.Client.Create(relatedTermCreateDto, this.TokenStorage.AdminAccessToken);
        var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<RelatedTermDTO>(response.Content);

        // Assert
        Assert.Equal(0, (int)response.StatusCode);
        Assert.Null(returnedValue);
    }

    [Fact]
    [ExtractCreateTestRelatedTerm]
    public async Task Create_ShouldReturnBadRequest_WhenRelatedTermWithWordAlreadyExists()
    {
        // Arrange
        var relatedTermCreateDto = ExtractCreateTestRelatedTermAttribute.RelatedTermCreateDtoForTest;
        relatedTermCreateDto.Word = _testRelatedTerm.Word!;
        relatedTermCreateDto.TermId = _testRelatedTerm.TermId;

        // Act
        var response = await this.Client.Create(relatedTermCreateDto, this.TokenStorage.AdminAccessToken);
        var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<RelatedTermDTO>(response.Content);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Null(returnedValue);
    }

    [Fact]
    [ExtractCreateTestRelatedTerm]
    public async Task Create_ShouldReturnUnauthorized_WhenTokenIsAbsent()
    {
        // Arrange
        var relatedTermCreateDto = ExtractCreateTestRelatedTermAttribute.RelatedTermCreateDtoForTest;

        // Act
        var response = await this.Client.Create(relatedTermCreateDto);
        var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<RelatedTermDTO>(response.Content);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.Null(returnedValue);
    }

    [Fact]
    [ExtractCreateTestRelatedTerm]
    public async Task Create_ShouldReturnForbidden_WhenUserIsNotAdmin()
    {
        // Arrange
        var relatedTermCreateDto = ExtractCreateTestRelatedTermAttribute.RelatedTermCreateDtoForTest;

        // Act
        var response = await this.Client.Create(relatedTermCreateDto, this.TokenStorage.UserAccessToken);
        var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<RelatedTermDTO>(response.Content);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        Assert.Null(returnedValue);
    }

    [Fact]
    [ExtractUpdateTestRelatedTerm]
    public async Task Update_ShouldReturnUnauthorized_WhenTokenIsAbsent()
    {
        // Arrange
        var relatedTermUpdateDto = ExtractUpdateTestRelatedTermAttribute.RelatedTermUpdateDtoForTest;
        relatedTermUpdateDto.Id = _testRelatedTerm.Id;

        // Act
        var response = await this.Client.Update(relatedTermUpdateDto.Id, relatedTermUpdateDto);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    [ExtractUpdateTestRelatedTerm]
    public async Task Update_ShouldReturnForbidden_WhenUserIsNotAdmin()
    {
        // Arrange
        var relatedTermUpdateDto = ExtractUpdateTestRelatedTermAttribute.RelatedTermUpdateDtoForTest;
        relatedTermUpdateDto.Id = _testRelatedTerm.Id;

        // Act
        var response = await this.Client.Update(relatedTermUpdateDto.Id, relatedTermUpdateDto, this.TokenStorage.UserAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    [ExtractDeleteTestRelatedTerm]
    public async Task Delete_ShouldReturnSuccessStatusCode_WhenRelatedTermExists()
    {
        // Arrange
        var relatedTerm = ExtractDeleteTestRelatedTermAttribute.RelatedTermForTest;

        // Act
        var response = await this.Client.Delete(relatedTerm.Word!, this.TokenStorage.AdminAccessToken);
        var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<RelatedTermDTO>(response.Content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(returnedValue);
        Assert.Multiple(
            () => Assert.Equal(relatedTerm.Id, returnedValue.Id),
            () => Assert.Equal(relatedTerm.Word, returnedValue.Word),
            () => Assert.Equal(relatedTerm.TermId, returnedValue.TermId));
    }

    [Fact]
    public async Task Delete_ShouldReturnBadRequest_WhenWordNotExists()
    {
        // Arrange
        const string relatedTermWord = "qwerty";

        // Act
        var response = await this.Client.Delete(relatedTermWord, this.TokenStorage.AdminAccessToken);
        var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<RelatedTermDTO>(response.Content);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Null(returnedValue);
    }

    [Fact]
    [ExtractDeleteTestRelatedTerm]
    public async Task Delete_ShouldReturnUnauthorized_WhenTokenIsAbsent()
    {
        // Arrange
        var relatedTermWord = ExtractDeleteTestRelatedTermAttribute.RelatedTermForTest.Word;

        // Act
        var response = await this.Client.Delete(relatedTermWord!);
        var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<RelatedTermDTO>(response.Content);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.Null(returnedValue);
    }

    [Fact]
    [ExtractDeleteTestRelatedTerm]
    public async Task Delete_ShouldReturnForbidden_WhenUserIsNotAdmin()
    {
        // Arrange
        var relatedTermWord = ExtractDeleteTestRelatedTermAttribute.RelatedTermForTest.Word;

        // Act
        var response = await this.Client.Delete(relatedTermWord!, this.TokenStorage.UserAccessToken);
        var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<RelatedTermDTO>(response.Content);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        Assert.Null(returnedValue);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            RelatedTermExtracter.Remove(_testRelatedTerm);
        }

        base.Dispose(disposing);
    }
}
