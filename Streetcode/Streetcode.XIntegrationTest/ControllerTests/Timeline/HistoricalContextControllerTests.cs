using System.Net;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.AdditionalContent.Timeline;
using Streetcode.XIntegrationTest.ControllerTests.Utils.AuthorizationFixture;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Timeline;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.Timeline;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Timeline;

[Collection("Authorization")]
public class HistoricalContextControllerTests : BaseAuthorizationControllerTests<HistoricalContextClient>,
    IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HistoricalContext _testCreateContext;
    private readonly HistoricalContext _testUpdateContext;
    private readonly StreetcodeContent _testStreetcodeContent;

    public HistoricalContextControllerTests(CustomWebApplicationFactory<Program> factory, TokenStorage tokenStorage)
        : base(factory, "/api/HistoricalContext", tokenStorage)
    {
        int uniqueId = UniqueNumberGenerator.GenerateInt();
        _testCreateContext = HistoricalContextExtracter.Extract(uniqueId);
        _testUpdateContext = HistoricalContextExtracter.Extract(uniqueId);
        _testStreetcodeContent = StreetcodeContentExtracter
            .Extract(
                uniqueId,
                uniqueId,
                Guid.NewGuid().ToString());
    }

    [Fact]
    public async Task GetAll_ReturnSuccessStatusCode()
    {
        // Act
        var response = await this.Client.GetAllAsync();
        var returnedValue =
            CaseIsensitiveJsonDeserializer.Deserialize<GetAllHistoricalContextDTO>(response.Content);

        // Assert
        Assert.Multiple(
            () => Assert.True(response.IsSuccessStatusCode),
            () => Assert.NotNull(returnedValue));
    }

    [Fact]
    public async Task GetById_ReturnSuccessStatusCode()
    {
        // Arrange
        HistoricalContext expectedContext = _testCreateContext;

        // Act
        var response = await this.Client.GetByIdAsync(expectedContext.Id);

        var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<HistoricalContextDTO>(response.Content);

        // Assert
        Assert.Multiple(
            () => Assert.True(response.IsSuccessStatusCode),
            () => Assert.NotNull(returnedValue),
            () => Assert.Equal(expectedContext.Id, returnedValue?.Id),
            () => Assert.Equal(expectedContext.Title, returnedValue?.Title));
    }

    [Fact]
    public async Task GetByIdIncorrect_ReturnBadRequest()
    {
        // Arrange
        const int incorrectId = -100;

        // Act
        var response = await this.Client.GetByIdAsync(incorrectId);

        // Assert
        Assert.Multiple(
            () => Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode),
            () => Assert.False(response.IsSuccessStatusCode));
    }

    [Fact]
    [ExtractCreateTestHistoricalContext]
    public async Task Create_ReturnsSuccessStatusCode()
    {
        // Arrange
        var historicalContextCreateDto = ExtractCreateTestHistoricalContextAttribute.HistoricalContextForTest;

        // Act
        var response = await this.Client.CreateAsync(historicalContextCreateDto, this.TokenStorage.AdminAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    [ExtractCreateTestHistoricalContext]
    public async Task Create_TokenNotPassed_ReturnsUnauthorized()
    {
        // Arrange
        var historicalContextCreateDto = ExtractCreateTestHistoricalContextAttribute.HistoricalContextForTest;

        // Act
        var response = await this.Client.CreateAsync(historicalContextCreateDto);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    [ExtractCreateTestHistoricalContext]
    public async Task Create_NotAdminTokenPassed_ReturnsForbidden()
    {
        // Arrange
        var historicalContextCreateDto = ExtractCreateTestHistoricalContextAttribute.HistoricalContextForTest;

        // Act
        var response = await this.Client.CreateAsync(historicalContextCreateDto, this.TokenStorage.UserAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    [ExtractCreateTestHistoricalContext]
    public async Task Create_CreatesNewHistoricalContext()
    {
        // Arrange
        var historicalContextCreateDto = ExtractCreateTestHistoricalContextAttribute.HistoricalContextForTest;

        // Act
        await this.Client.CreateAsync(historicalContextCreateDto, this.TokenStorage.AdminAccessToken);
        var getResponse = await this.Client.GetByTitle(historicalContextCreateDto.Title);
        var fetchedStreetcode = CaseIsensitiveJsonDeserializer.Deserialize<HistoricalContext>(getResponse.Content);

        // Assert
        Assert.Equal(historicalContextCreateDto.Title, fetchedStreetcode?.Title);
    }

    [Fact]
    [ExtractCreateTestHistoricalContext]
    public async Task Create_WithInvalidData_ReturnsBadRequest()
    {
        // Arrange
        var historicalContextCreateDto = ExtractCreateTestHistoricalContextAttribute.HistoricalContextForTest;
        historicalContextCreateDto.Title = null!; // Invalid data

        // Act
        var response = await this.Client.CreateAsync(historicalContextCreateDto, this.TokenStorage.AdminAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    [ExtractCreateTestHistoricalContext]
    public async Task Create_WithExistingTag_ReturnsConflict()
    {
        // Arrange
        var historicalContextCreateDto = ExtractCreateTestHistoricalContextAttribute.HistoricalContextForTest;
        historicalContextCreateDto.Title = _testCreateContext.Title!;

        // Act
        var response = await this.Client.CreateAsync(historicalContextCreateDto, this.TokenStorage.AdminAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    [ExtractUpdateTestHistoricalContext]
    public async Task Update_ReturnSuccessStatusCode()
    {
        // Arrange
        var historicalContextCreateDto = ExtractUpdateTestHistoricalContextAttribute.HistoricalContextForTest;
        historicalContextCreateDto.Id = _testCreateContext.Id;

        // Act
        var response = await this.Client.UpdateAsync(historicalContextCreateDto, this.TokenStorage.AdminAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    [ExtractUpdateTestHistoricalContext]
    public async Task Update_Incorect_ReturnBadRequest()
    {
        // Arrange
        var historicalContextCreateDto = ExtractUpdateTestHistoricalContextAttribute.HistoricalContextForTest;
        const int invalidHistoricalContextId = -10;
        historicalContextCreateDto.Id = invalidHistoricalContextId;

        // Act
        var response = await this.Client.UpdateAsync(historicalContextCreateDto, this.TokenStorage.AdminAccessToken);

        // Assert
        Assert.Multiple(
            () => Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode),
            () => Assert.False(response.IsSuccessStatusCode));
    }

    [Fact]
    [ExtractUpdateTestHistoricalContext]
    public async Task Update_TokenNotPassed_ReturnsUnauthorized()
    {
        // Arrange
        var historicalContextCreateDto = ExtractUpdateTestHistoricalContextAttribute.HistoricalContextForTest;

        // Act
        var response = await this.Client.UpdateAsync(historicalContextCreateDto);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    [ExtractUpdateTestHistoricalContext]
    public async Task Update_NotAdminTokenPassed_ReturnsForbidden()
    {
        // Arrange
        var historicalContextCreateDto = ExtractUpdateTestHistoricalContextAttribute.HistoricalContextForTest;

        // Act
        var response = await this.Client.UpdateAsync(historicalContextCreateDto, this.TokenStorage.UserAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    [ExtractUpdateTestHistoricalContext]
    public async Task Update_WithInvalidData_ReturnsBadRequest()
    {
        // Arrange
        var historicalContextCreateDto = ExtractUpdateTestHistoricalContextAttribute.HistoricalContextForTest;
        historicalContextCreateDto.Title = null!; // Invalid data

        // Act
        var response = await this.Client.UpdateAsync(historicalContextCreateDto, this.TokenStorage.AdminAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    [ExtractUpdateTestHistoricalContext]
    public async Task Update_WithExistingTitle_ReturnsBadRequest()
    {
        // Arrange
        var historicalContextCreateDto = ExtractUpdateTestHistoricalContextAttribute.HistoricalContextForTest;
        historicalContextCreateDto.Id = _testCreateContext.Id;
        historicalContextCreateDto.Title = _testUpdateContext.Title!;

        // Act
        var response = await this.Client.UpdateAsync(historicalContextCreateDto, this.TokenStorage.AdminAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ReturnsSuccessStatusCode()
    {
        // Arrange
        int id = _testCreateContext.Id;

        // Act
        var response = await this.Client.Delete(id, this.TokenStorage.AdminAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Delete_TokenNotPassed_ReturnsUnathorized()
    {
        // Arrange
        int id = _testCreateContext.Id;

        // Act
        var response = await this.Client.Delete(id);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Delete_NotAdminTokenPassed_ReturnsForbidden()
    {
        // Arrange
        int id = _testCreateContext.Id;

        // Act
        var response = await this.Client.Delete(id, this.TokenStorage.UserAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task Delete_WithInvalidData_ReturnsBadRequest()
    {
        // Arrange
        const int id = -100;

        // Act
        var response = await this.Client.Delete(id, this.TokenStorage.AdminAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ChangesNotSaved_ReturnsBadRequest()
    {
        int id = this.testUpdateContext.Id;

        var repositoryMock = new Mock<IHistoricalContextRepository>();
        var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
        var mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        repositoryMock.Setup(r => r.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<HistoricalContext, bool>>>(), default))
            .ReturnsAsync(this.testUpdateContext);
        repositoryMock.Setup(r => r.Delete(default!));

        repositoryWrapperMock.SetupGet(wrapper => wrapper.HistoricalContextRepository).Returns(repositoryMock.Object);
        repositoryWrapperMock.Setup(wrapper => wrapper.SaveChangesAsync()).ReturnsAsync(null);
        repositoryWrapperMock.Setup(wrapper => wrapper.SaveChangesAsync()).Throws(default(Exception));

        var loggerMock = new Mock<ILoggerService>();

        var handler = new DeleteHistoricalContextHandler(repositoryWrapperMock.Object, loggerMock.Object, mockLocalizerCannotFind.Object);

        var query = new DeleteHistoricalContextCommand(id);
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await handler.Handle(query, cancellationToken);

        // Assert
        Assert.False(result.IsSuccess);
    }
}