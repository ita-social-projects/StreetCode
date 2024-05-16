using System.Linq.Expressions;
using System.Net;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Timeline.HistoricalContext.Create;
using Streetcode.BLL.MediatR.Timeline.HistoricalContext.Delete;
using Streetcode.BLL.MediatR.Timeline.HistoricalContext.Update;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.DAL.Repositories.Interfaces.AdditionalContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.DAL.Repositories.Interfaces.Timeline;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.AdditionalContent.Timeline;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Timeline;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.Timeline;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Timeline;

[Collection("Authorization")]
public class HistoricalContextControllerTests : BaseAuthorizationControllerTests<HistoricalContextClient>,
    IClassFixture<CustomWebApplicationFactory<Program>>
{
    private HistoricalContext _testCreateContext;
    private HistoricalContext _testUpdateContext;
    private StreetcodeContent _testStreetcodeContent;

    public HistoricalContextControllerTests(CustomWebApplicationFactory<Program> factory, TokenStorage tokenStorage)
        : base(factory, "/api/HistoricalContext", tokenStorage)
    {
        int uniqueId = UniqueNumberGenerator.GenerateInt();
        this._testCreateContext = HistoricalContextExtracter.Extract(uniqueId);
        this._testUpdateContext = HistoricalContextExtracter.Extract(uniqueId);
        this._testStreetcodeContent = StreetcodeContentExtracter
            .Extract(
                uniqueId,
                uniqueId,
                Guid.NewGuid().ToString());
    }

    public override void Dispose()
    {
        StreetcodeContentExtracter.Remove(this._testStreetcodeContent);
        HistoricalContextExtracter.Remove(this._testCreateContext);
    }

    [Fact]
    public async Task GetAll_ReturnSuccessStatusCode()
    {
        var response = await this.client.GetAllAsync();
        var returnedValue =
            CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<HistoricalContextDTO>>(response.Content);

        Assert.True(response.IsSuccessStatusCode);
        Assert.NotNull(returnedValue);
    }

    [Fact]
    public async Task GetById_ReturnSuccessStatusCode()
    {
        HistoricalContext expectedContext = this._testCreateContext;
        var response = await this.client.GetByIdAsync(expectedContext.Id);

        var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<HistoricalContextDTO>(response.Content);

        Assert.True(response.IsSuccessStatusCode);
        Assert.NotNull(returnedValue);
        Assert.Multiple(
            () => Assert.Equal(expectedContext.Id, returnedValue?.Id),
            () => Assert.Equal(expectedContext.Title, returnedValue?.Title));
    }

    [Fact]
    public async Task GetByIdIncorrect_ReturnBadRequest()
    {
        int incorrectId = -100;
        var response = await this.client.GetByIdAsync(incorrectId);

        Assert.Multiple(
            () => Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode),
            () => Assert.False(response.IsSuccessStatusCode));
    }

    [Fact]
    [ExtractCreateTestHistoricalContext]
    public async Task Create_ReturnsSuccessStatusCode()
    {
        // Arrange
        var historicalContextCreateDto = ExtractCreateTestHistoricalContext.HistoricalContextForTest;

        // Act
        var response = await client.CreateAsync(historicalContextCreateDto, _tokenStorage.AdminAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    [ExtractCreateTestHistoricalContext]
    public async Task Create_TokenNotPassed_ReturnsUnauthorized()
    {
        // Arrange
        var historicalContextCreateDto = ExtractCreateTestHistoricalContext.HistoricalContextForTest;

        // Act
        var response = await client.CreateAsync(historicalContextCreateDto);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    [ExtractCreateTestHistoricalContext]
    public async Task Create_NotAdminTokenPassed_ReturnsForbidden()
    {
        // Arrange
        var historicalContextCreateDto = ExtractCreateTestHistoricalContext.HistoricalContextForTest;

        // Act
        var response = await client.CreateAsync(historicalContextCreateDto, _tokenStorage.UserAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }


    [Fact]
    [ExtractCreateTestHistoricalContext]
    public async Task Create_CreatesNewHistoricalContext()
    {
        // Arrange
        var historicalContextCreateDto = ExtractCreateTestHistoricalContext.HistoricalContextForTest;

        // Act
        var response = await client.CreateAsync(historicalContextCreateDto, _tokenStorage.AdminAccessToken);
        var getResponse = await client.GetByTitle(historicalContextCreateDto.Title);
        var fetchedStreetcode = CaseIsensitiveJsonDeserializer.Deserialize<HistoricalContext>(getResponse.Content);

        // Assert
        Assert.Equal(historicalContextCreateDto.Title, fetchedStreetcode.Title);
    }

    [Fact]
    [ExtractCreateTestHistoricalContext]
    public async Task Create_WithInvalidData_ReturnsBadRequest()
    {
        // Arrange
        var historicalContextCreateDto = ExtractCreateTestHistoricalContext.HistoricalContextForTest;
        historicalContextCreateDto.Title = null; // Invalid data

        // Act
        var response = await client.CreateAsync(historicalContextCreateDto, this._tokenStorage.AdminAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    [ExtractCreateTestHistoricalContext]
    public async Task Create_WithExistingTag_ReturnsConflict()
    {
        // Arrange
        var historicalContextCreateDto = ExtractCreateTestHistoricalContext.HistoricalContextForTest;
        historicalContextCreateDto.Title = _testCreateContext.Title;

        // Act
        var response = await client.CreateAsync(historicalContextCreateDto, this._tokenStorage.AdminAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    [ExtractUpdateTestHistoricalContext]
    public async Task Update_ReturnSuccessStatusCode()
    {
        // Arrange
        var historicalContextCreateDto = ExtractUpdateTestHistoricalContext.HistoricalContextForTest;
        historicalContextCreateDto.Id = this._testCreateContext.Id;

        // Act
        var response = await client.UpdateAsync(historicalContextCreateDto, this._tokenStorage.AdminAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    [ExtractUpdateTestHistoricalContext]
    public async Task Update_Incorect_ReturnBadRequest()
    {
        // Arrange
        var historicalContextCreateDto = ExtractUpdateTestHistoricalContext.HistoricalContextForTest;

        // Act
        var response = await client.UpdateAsync(historicalContextCreateDto, this._tokenStorage.AdminAccessToken);

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
        var historicalContextCreateDto = ExtractUpdateTestHistoricalContext.HistoricalContextForTest;

        // Act
        var response = await client.UpdateAsync(historicalContextCreateDto);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    [ExtractUpdateTestHistoricalContext]
    public async Task Update_NotAdminTokenPassed_ReturnsForbidden()
    {
        // Arrange
        var historicalContextCreateDto = ExtractUpdateTestHistoricalContext.HistoricalContextForTest;

        // Act
        var response = await client.UpdateAsync(historicalContextCreateDto, this._tokenStorage.UserAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    [ExtractUpdateTestHistoricalContext]
    public async Task Update_WithInvalidData_ReturnsBadRequest()
    {
        // Arrange
        var historicalContextCreateDto = ExtractUpdateTestHistoricalContext.HistoricalContextForTest;
        historicalContextCreateDto.Title = null; // Invalid data

        // Act
        var response = await client.UpdateAsync(historicalContextCreateDto, this._tokenStorage.AdminAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    [ExtractUpdateTestHistoricalContext]
    public async Task Update_WithExistingTitle_ReturnsBadRequest()
    {
        // Arrange
        var historicalContextCreateDto = ExtractUpdateTestHistoricalContext.HistoricalContextForTest;
        historicalContextCreateDto.Id = this._testCreateContext.Id;
        historicalContextCreateDto.Title = this._testUpdateContext.Title;

        // Act
        var response = await client.UpdateAsync(historicalContextCreateDto, this._tokenStorage.AdminAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    [ExtractUpdateTestHistoricalContext]
    public async Task Update_ChangesNotSaved_ReturnsBadRequest()
    {
        // Arrange
        Expression<Func<HistoricalContext, bool>> testExpression = x => x.Id == this._testUpdateContext.Id;

        var historicalContextCreateDto = ExtractUpdateTestHistoricalContext.HistoricalContextForTest;
        historicalContextCreateDto.Id = this._testUpdateContext.Id;

        var repositoryMock = new Mock<IHistoricalContextRepository>();
        var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
        repositoryMock.Setup(r => r.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<HistoricalContext, bool>>>(), default))
            .ReturnsAsync((Expression<Func<HistoricalContext, bool>> expr, IIncludableQueryable<HistoricalContext, bool> include) =>
            {
                var compiledExpr = expr.Compile();
                return compiledExpr(this._testUpdateContext) ? this._testUpdateContext : null;
            });

        repositoryWrapperMock.SetupGet(wrapper => wrapper.HistoricalContextRepository).Returns(repositoryMock.Object);
        repositoryWrapperMock.Setup(wrapper => wrapper.SaveChangesAsync()).ReturnsAsync(null);
        repositoryWrapperMock.Setup(wrapper => wrapper.SaveChangesAsync()).Throws(default(Exception));

        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(m => m.Map<HistoricalContext>(default)).Returns(this._testUpdateContext);
        var loggerMock = new Mock<ILoggerService>();

        var handler = new UpdateHistoricalContextHandler(repositoryWrapperMock.Object, mapperMock.Object, loggerMock.Object);

        var query = new UpdateHistoricalContextCommand(historicalContextCreateDto);
        var cancellationToken = CancellationToken.None;
        // Act
        var result = await handler.Handle(query, cancellationToken);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task Delete_ReturnsSuccessStatusCode()
    {
        // Arrange
        int id = this._testCreateContext.Id;

        // Act
        var response = await this.client.Delete(id, this._tokenStorage.AdminAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Delete_TokenNotPassed_ReturnsUnathorized()
    {
        // Arrange
        int id = this._testCreateContext.Id;

        // Act
        var response = await this.client.Delete(id);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Delete_NotAdminTokenPassed_ReturnsForbidden()
    {
        // Arrange
        int id = this._testCreateContext.Id;

        // Act
        var response = await this.client.Delete(id, this._tokenStorage.UserAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task Delete_WithInvalidData_ReturnsBadRequest()
    {
        // Arrange
        int id = -100;

        // Act
        var response = await this.client.Delete(id, this._tokenStorage.AdminAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ChangesNotSaved_ReturnsBadRequest()
    {
        int id = this._testUpdateContext.Id;

        var repositoryMock = new Mock<IHistoricalContextRepository>();
        var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
        repositoryMock.Setup(r => r.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<HistoricalContext, bool>>>(), default))
            .ReturnsAsync(this._testUpdateContext);
        repositoryMock.Setup(r => r.Delete(default));

        repositoryWrapperMock.SetupGet(wrapper => wrapper.HistoricalContextRepository).Returns(repositoryMock.Object);
        repositoryWrapperMock.Setup(wrapper => wrapper.SaveChangesAsync()).ReturnsAsync(null);
        repositoryWrapperMock.Setup(wrapper => wrapper.SaveChangesAsync()).Throws(default(Exception));

        var loggerMock = new Mock<ILoggerService>();

        var handler = new DeleteHistoricalContextHandler(repositoryWrapperMock.Object, loggerMock.Object);

        var query = new DeleteHistoricalContextCommand(id);
        var cancellationToken = CancellationToken.None;
        // Act
        var result = await handler.Handle(query, cancellationToken);

        // Assert
        Assert.False(result.IsSuccess);
    }
}