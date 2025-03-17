using System.Net;
using Streetcode.BLL.DTO.Streetcode.TextContent.Term;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.AuthorizationFixture;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Streetcode.TextContent.Terms;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.StreetCode.TextContent;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter.TextContent;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Streetcode.TextContent;

[Collection("Authorization")]
public class TermControllerTests : BaseAuthorizationControllerTests<TermClient>
{
    private readonly Term _testTerm;

    public TermControllerTests(CustomWebApplicationFactory<Program> factory, TokenStorage tokenStorage)
        : base(factory, "/api/Term", tokenStorage)
    {
        var termId = UniqueNumberGenerator.GenerateInt();
        _testTerm = TermExtracter.Extract(termId);
    }

    [Fact]
    public async Task GetAll_ShouldReturnSuccessStatusCode_WhenTermsReceived()
    {
        // Act
        var response = await this.Client.GetAllAsync();
        var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<GetAllTermsDto>(response.Content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(returnedValue);
        Assert.NotEmpty(returnedValue.Terms);
    }

    [Fact]
    public async Task GetById_ShouldReturnSuccessStatusCode_WhenIdIsValid()
    {
        // Arrange
        var termId = _testTerm.Id;

        // Act
        var response = await this.Client.GetByIdAsync(termId);
        var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<TermDto>(response.Content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(returnedValue);
        Assert.Multiple(
            () => Assert.Equal(_testTerm.Id, returnedValue.Id),
            () => Assert.Equal(_testTerm.Title, returnedValue.Title),
            () => Assert.Equal(_testTerm.Description, returnedValue.Description));
    }

    [Fact]
    public async Task GetById_ShouldReturnBadRequest_WhenIdIsNotValid()
    {
        // Arrange
        const int termId = int.MinValue;

        // Act
        var response = await this.Client.GetByIdAsync(termId);
        var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<TermDto>(response.Content);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Null(returnedValue);
    }

    [Fact]
    [ExtractCreateTestTerm]
    public async Task Create_ShouldReturnSuccessStatusCode_WhenTermAdded()
    {
        // Arrange
        var termCreateDto = ExtractCreateTestTermAttribute.TermCreateDtoForTest;

        // Act
        var response = await this.Client.Create(termCreateDto, this.TokenStorage.AdminAccessToken);
        var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<TermDto>(response.Content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(returnedValue);
        Assert.Multiple(
            () => Assert.Equal(termCreateDto.Title, returnedValue.Title),
            () => Assert.Equal(termCreateDto.Description, returnedValue.Description));
    }

    [Fact]
    [ExtractCreateTestTerm]
    public async Task Create_ShouldReturnFail_WhenTermIsInvalid()
    {
        // Arrange
        var termCreateDto = ExtractCreateTestTermAttribute.TermCreateDtoForTest;
        termCreateDto.Title = null!;

        // Act
        var response = await this.Client.Create(termCreateDto, this.TokenStorage.AdminAccessToken);
        var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<TermDto>(response.Content);

        // Assert
        Assert.Equal(0, (int)response.StatusCode);
        Assert.Null(returnedValue);
    }

    [Fact]
    [ExtractCreateTestTerm]
    public async Task Create_ShouldReturnUnauthorized_WhenTokenIsAbsent()
    {
        // Arrange
        var termCreateDto = ExtractCreateTestTermAttribute.TermCreateDtoForTest;

        // Act
        var response = await this.Client.Create(termCreateDto);
        var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<TermDto>(response.Content);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.Null(returnedValue);
    }

    [Fact]
    [ExtractCreateTestTerm]
    public async Task Create_ShouldReturnForbidden_WhenUserIsNotAdmin()
    {
        // Arrange
        var termCreateDto = ExtractCreateTestTermAttribute.TermCreateDtoForTest;

        // Act
        var response = await this.Client.Create(termCreateDto, this.TokenStorage.UserAccessToken);
        var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<TermDto>(response.Content);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        Assert.Null(returnedValue);
    }

    [Fact]
    [ExtractUpdateTestTerm]
    public async Task Update_ShouldReturnSuccessStatusCode_WhenFactUpdated()
    {
        // Arrange
        var factUpdateDto = ExtractUpdateTestTermAttribute.TermUpdateDtoForTest;
        factUpdateDto.Id = _testTerm.Id;

        // Act
        var response = await this.Client.Update(factUpdateDto, this.TokenStorage.AdminAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("{}", response.Content);
    }

    [Fact]
    [ExtractUpdateTestTerm]
    public async Task Update_ShouldReturnFail_WhenIdNotExists()
    {
        // Arrange
        var factUpdateDto = ExtractUpdateTestTermAttribute.TermUpdateDtoForTest;
        factUpdateDto.Id = int.MinValue;

        // Act
        var response = await this.Client.Update(factUpdateDto, this.TokenStorage.AdminAccessToken);

        // Assert
        Assert.Equal(0, (int)response.StatusCode);
    }

    [Fact]
    [ExtractUpdateTestTerm]
    public async Task Update_ShouldReturnFail_WhenFactIsInvalid()
    {
        // Arrange
        var factUpdateDto = ExtractUpdateTestTermAttribute.TermUpdateDtoForTest;
        factUpdateDto.Id = _testTerm.Id;
        factUpdateDto.Title = null!;

        // Act
        var response = await this.Client.Update(factUpdateDto, this.TokenStorage.AdminAccessToken);

        // Assert
        Assert.Equal(0, (int)response.StatusCode);
    }

    [Fact]
    [ExtractUpdateTestTerm]
    public async Task Update_ShouldReturnUnauthorized_WhenTokenIsAbsent()
    {
        // Arrange
        var factUpdateDto = ExtractUpdateTestTermAttribute.TermUpdateDtoForTest;
        factUpdateDto.Id = _testTerm.Id;

        // Act
        var response = await this.Client.Update(factUpdateDto);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    [ExtractUpdateTestTerm]
    public async Task Update_ShouldReturnForbidden_WhenUserIsNotAdmin()
    {
        // Arrange
        var factUpdateDto = ExtractUpdateTestTermAttribute.TermUpdateDtoForTest;
        factUpdateDto.Id = _testTerm.Id;

        // Act
        var response = await this.Client.Update(factUpdateDto, this.TokenStorage.UserAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    [ExtractDeleteTestTerm]
    public async Task Delete_ShouldReturnSuccessStatusCode_WhenTermExists()
    {
        // Arrange
        var termId = ExtractDeleteTestTermAttribute.TermForTest.Id;

        // Act
        var response = await this.Client.Delete(termId, this.TokenStorage.AdminAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("{}", response.Content);
    }

    [Fact]
    [ExtractDeleteTestTerm]
    public async Task Delete_ShouldReturnBadRequest_WhenIdNotExists()
    {
        // Arrange
        const int termId = int.MinValue;

        // Act
        var response = await this.Client.Delete(termId, this.TokenStorage.AdminAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    [ExtractDeleteTestTerm]
    public async Task Delete_ShouldReturnUnauthorized_WhenTokenIsAbsent()
    {
        // Arrange
        var termId = ExtractDeleteTestTermAttribute.TermForTest.Id;

        // Act
        var response = await this.Client.Delete(termId);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    [ExtractDeleteTestTerm]
    public async Task Delete_ShouldReturnForbidden_WhenUserIsNotAdmin()
    {
        // Arrange
        var termId = ExtractDeleteTestTermAttribute.TermForTest.Id;

        // Act
        var response = await this.Client.Delete(termId, this.TokenStorage.UserAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            TermExtracter.Remove(_testTerm);
        }

        base.Dispose(disposing);
    }
}
