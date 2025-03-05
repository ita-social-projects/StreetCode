using System.Net;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.AuthorizationFixture;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Streetcode.TextContent.Facts;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.StreetCode.TextContent;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter.TextContent;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Streetcode.TextContent;

[Collection("Authorization")]
public class FactControllerTests : BaseAuthorizationControllerTests<FactClient>
{
    private readonly Fact _testFact;

    public FactControllerTests(CustomWebApplicationFactory<Program> factory, TokenStorage tokenStorage)
        : base(factory, "/api/Fact", tokenStorage)
    {
        var factId = UniqueNumberGenerator.GenerateInt();
        _testFact = FactExtracter.Extract(factId);
    }

    [Fact]
    public async Task GetAll_ShouldReturnSuccessStatusCode_WhenFactsReceived()
    {
        // Act
        var response = await this.Client.GetAllAsync();
        var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<FactDto>>(response.Content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(returnedValue);
    }

    [Fact]
    public async Task GetById_ShouldReturnSuccessStatusCode_WhenIdIsValid()
    {
        // Arrange
        var factId = _testFact.Id;

        // Act
        var response = await this.Client.GetByIdAsync(factId);
        var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<FactDto>(response.Content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(returnedValue);
        Assert.Multiple(
            () => Assert.Equal(_testFact.Id, returnedValue.Id),
            () => Assert.Equal(_testFact.Title, returnedValue.Title),
            () => Assert.Equal(_testFact.ImageId, returnedValue.ImageId),
            () => Assert.Equal(_testFact.FactContent, returnedValue.FactContent),
            () => Assert.Equal(_testFact.Index, returnedValue.Index));
    }

    [Fact]
    public async Task GetById_ShouldReturnBadRequest_WhenIdIsNotValid()
    {
        // Arrange
        const int factId = int.MinValue;

        // Act
        var response = await this.Client.GetByIdAsync(factId);
        var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<FactDto>(response.Content);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Null(returnedValue);
    }

    [Fact]
    public async Task GetByStreetcodeId_ShouldReturnSuccessStatusCode_WhenIdIsValid()
    {
        // Arrange
        var factStreetcodeId = _testFact.StreetcodeId;

        // Act
        var response = await this.Client.GetByStreetcodeId(factStreetcodeId);
        var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<FactDto>>(response.Content);
        var returnedValueList = returnedValue!.ToList();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(returnedValue);
        Assert.Multiple(
            () => Assert.Equal(_testFact.Id, returnedValueList[0].Id),
            () => Assert.Equal(_testFact.Title, returnedValueList[0].Title),
            () => Assert.Equal(_testFact.ImageId, returnedValueList[0].ImageId),
            () => Assert.Equal(_testFact.FactContent, returnedValueList[0].FactContent),
            () => Assert.Equal(_testFact.Index, returnedValueList[0].Index));
    }

    [Fact]
    public async Task GetByStreetcodeId_ShouldReturnBadRequest_WhenIdIsNotValid()
    {
        // Arrange
        const int factStreetcodeId = int.MinValue;

        // Act
        var response = await this.Client.GetByStreetcodeId(factStreetcodeId);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    [ExtractCreateTestFact]
    public async Task Create_ShouldReturnSuccessStatusCode_WhenFactAdded()
    {
        // Arrange
        var factCreateDto = ExtractCreateTestFactAttribute.FactCreateDtoForTest;

        // Act
        var response = await this.Client.Create(factCreateDto, this.TokenStorage.AdminAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("{}", response.Content);
    }

    [Fact]
    [ExtractCreateTestFact]
    public async Task Create_ShouldReturnFail_WhenFactIsInvalid()
    {
        // Arrange
        var factCreateDto = ExtractCreateTestFactAttribute.FactCreateDtoForTest;
        factCreateDto.Title = null!;

        // Act
        var response = await this.Client.Create(factCreateDto, this.TokenStorage.AdminAccessToken);

        // Assert
        Assert.Equal(0, (int)response.StatusCode);
    }

    [Fact]
    [ExtractCreateTestFact]
    public async Task Create_ShouldReturnUnauthorized_WhenTokenIsAbsent()
    {
        // Arrange
        var factCreateDto = ExtractCreateTestFactAttribute.FactCreateDtoForTest;

        // Act
        var response = await this.Client.Create(factCreateDto);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    [ExtractCreateTestFact]
    public async Task Create_ShouldReturnForbidden_WhenUserIsNotAdmin()
    {
        // Arrange
        var factCreateDto = ExtractCreateTestFactAttribute.FactCreateDtoForTest;

        // Act
        var response = await this.Client.Create(factCreateDto, this.TokenStorage.UserAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    [ExtractUpdateTestFact]
    public async Task Update_ShouldReturnSuccessStatusCode_WhenFactUpdated()
    {
        // Arrange
        var factUpdateDto = ExtractUpdateTestFactAttribute.FactUpdateDtoForTest;
        factUpdateDto.Id = _testFact.Id;

        // Act
        var response = await this.Client.Update(_testFact.Id, factUpdateDto, this.TokenStorage.AdminAccessToken);

        // Assert
        Assert.Equal(0, (int)response.StatusCode);
    }

    [Fact]
    [ExtractUpdateTestFact]
    public async Task Update_ShouldReturnFail_WhenIdNotExists()
    {
        // Arrange
        var factUpdateDto = ExtractUpdateTestFactAttribute.FactUpdateDtoForTest;
        factUpdateDto.Id = int.MinValue;

        // Act
        var response = await this.Client.Update(int.MinValue, factUpdateDto, this.TokenStorage.AdminAccessToken);

        // Assert
        Assert.Equal(0, (int)response.StatusCode);
    }

    [Fact]
    [ExtractUpdateTestFact]
    public async Task Update_ShouldReturnFail_WhenFactIsInvalid()
    {
        // Arrange
        var factUpdateDto = ExtractUpdateTestFactAttribute.FactUpdateDtoForTest;
        factUpdateDto.Id = _testFact.Id;
        factUpdateDto.Title = null!;

        // Act
        var response = await this.Client.Update(_testFact.Id, factUpdateDto, this.TokenStorage.AdminAccessToken);

        // Assert
        Assert.Equal(0, (int)response.StatusCode);
    }

    [Fact]
    [ExtractUpdateTestFact]
    public async Task Update_ShouldReturnUnauthorized_WhenTokenIsAbsent()
    {
        // Arrange
        var factUpdateDto = ExtractUpdateTestFactAttribute.FactUpdateDtoForTest;
        factUpdateDto.Id = _testFact.Id;

        // Act
        var response = await this.Client.Update(_testFact.Id, factUpdateDto);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    [ExtractUpdateTestFact]
    public async Task Update_ShouldReturnForbidden_WhenUserIsNotAdmin()
    {
        // Arrange
        var factUpdateDto = ExtractUpdateTestFactAttribute.FactUpdateDtoForTest;

        // Act
        var response = await this.Client.Update(_testFact.Id, factUpdateDto, this.TokenStorage.UserAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    [ExtractDeleteTestFact]
    public async Task Delete_ShouldReturnSuccessStatusCode_WhenFactExists()
    {
        // Arrange
        var factId = ExtractDeleteTestFactAttribute.FactForTest.Id;

        // Act
        var response = await this.Client.Delete(factId, this.TokenStorage.AdminAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("{}", response.Content);
    }

    [Fact]
    public async Task Delete_ShouldReturnBadRequest_WhenIdNotExists()
    {
        // Arrange
        const int factId = int.MinValue;

        // Act
        var response = await this.Client.Delete(factId, this.TokenStorage.AdminAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    [ExtractDeleteTestFact]
    public async Task Delete_ShouldReturnUnauthorized_WhenTokenIsAbsent()
    {
        // Arrange
        var factId = ExtractDeleteTestFactAttribute.FactForTest.Id;

        // Act
        var response = await this.Client.Delete(factId);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    [ExtractDeleteTestFact]
    public async Task Delete_ShouldReturnForbidden_WhenUserIsNotAdmin()
    {
        // Arrange
        var factId = ExtractDeleteTestFactAttribute.FactForTest.Id;

        // Act
        var response = await this.Client.Delete(factId, this.TokenStorage.UserAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            FactExtracter.Remove(_testFact);
        }

        base.Dispose(disposing);
    }
}
