using System.Linq.Expressions;
using System.Net;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Timeline.HistoricalContext.Delete;
using Streetcode.BLL.MediatR.Timeline.HistoricalContext.Update;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.DAL.Repositories.Interfaces.Timeline;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.AdditionalContent.Timeline;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Timeline;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.Job;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.Timeline;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Timeline;

[Collection("Authorization")]
public class HistoricalContextControllerTests : BaseAuthorizationControllerTests<HistoricalContextClient>,
    IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HistoricalContext testCreateContext;
    private readonly HistoricalContext testUpdateContext;
    private readonly StreetcodeContent testStreetcodeContent;

    public HistoricalContextControllerTests(CustomWebApplicationFactory<Program> factory, TokenStorage tokenStorage)
        : base(factory, "/api/HistoricalContext", tokenStorage)
    {
        int uniqueId = UniqueNumberGenerator.GenerateInt();
        this.testCreateContext = HistoricalContextExtracter.Extract(uniqueId);
        this.testUpdateContext = HistoricalContextExtracter.Extract(uniqueId);
        this.testStreetcodeContent = StreetcodeContentExtracter
            .Extract(
                uniqueId,
                uniqueId,
                Guid.NewGuid().ToString());
    }

    [Fact]
    public async Task GetAll_ReturnSuccessStatusCode()
    {
        var response = await this.Client.GetAllAsync();
        var returnedValue =
            CaseIsensitiveJsonDeserializer.Deserialize<GetAllHistoricalContextDTO>(response.Content);

        Assert.True(response.IsSuccessStatusCode);
        Assert.NotNull(returnedValue);
    }

    [Fact]
    public async Task GetById_ReturnSuccessStatusCode()
    {
        HistoricalContext expectedContext = this.testCreateContext;
        var response = await this.Client.GetByIdAsync(expectedContext.Id);

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
        var response = await this.Client.GetByIdAsync(incorrectId);

        Assert.Multiple(
            () => Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode),
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
        historicalContextCreateDto.Title = this.testCreateContext.Title!;

        // Act
        var response = await this.Client.CreateAsync(historicalContextCreateDto, this.TokenStorage.AdminAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    [ExtractUpdateTestHistoricalContextAttribute]
    public async Task Update_ReturnSuccessStatusCode()
    {
        // Arrange
        var historicalContextCreateDto = ExtractUpdateTestHistoricalContextAttribute.HistoricalContextForTest;
        historicalContextCreateDto.Id = this.testCreateContext.Id;

        // Act
        var response = await this.Client.UpdateAsync(historicalContextCreateDto, this.TokenStorage.AdminAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    [ExtractUpdateTestHistoricalContextAttribute]
    public async Task Update_Incorect_ReturnBadRequest()
    {
        // Arrange
        var historicalContextCreateDto = ExtractUpdateTestHistoricalContextAttribute.HistoricalContextForTest;

        // Act
        var response = await this.Client.UpdateAsync(historicalContextCreateDto, this.TokenStorage.AdminAccessToken);

        // Assert
        Assert.Multiple(
            () => Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode),
            () => Assert.False(response.IsSuccessStatusCode));
    }

    [Fact]
    [ExtractUpdateTestHistoricalContextAttribute]
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
    [ExtractUpdateTestHistoricalContextAttribute]
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
    [ExtractUpdateTestHistoricalContextAttribute]
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
    [ExtractUpdateTestHistoricalContextAttribute]
    public async Task Update_WithExistingTitle_ReturnsBadRequest()
    {
        // Arrange
        var historicalContextCreateDto = ExtractUpdateTestHistoricalContextAttribute.HistoricalContextForTest;
        historicalContextCreateDto.Id = this.testCreateContext.Id;
        historicalContextCreateDto.Title = this.testUpdateContext.Title!;

        // Act
        var response = await this.Client.UpdateAsync(historicalContextCreateDto, this.TokenStorage.AdminAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    [ExtractUpdateTestHistoricalContextAttribute]
    public async Task Update_ChangesNotSaved_ReturnsBadRequest()
    {
        // Arrange
        var historicalContextCreateDto = ExtractUpdateTestHistoricalContextAttribute.HistoricalContextForTest;
        historicalContextCreateDto.Id = this.testUpdateContext.Id;

        var repositoryMock = new Mock<IHistoricalContextRepository>();
        var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
        var mockLocalizerValidation = new Mock<IStringLocalizer<FailedToValidateSharedResource>>();
        var mockLocalizerFieldNames = new Mock<IStringLocalizer<FieldNamesSharedResource>>();
        var mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        repositoryMock.Setup(r => r.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<HistoricalContext, bool>>>(), default))
            .ReturnsAsync((Expression<Func<HistoricalContext, bool>> expr, IIncludableQueryable<HistoricalContext, bool> include) =>
            {
                var compiledExpr = expr.Compile();
                return compiledExpr(this.testUpdateContext) ? this.testUpdateContext : null;
            });

        repositoryWrapperMock.SetupGet(wrapper => wrapper.HistoricalContextRepository).Returns(repositoryMock.Object);
        repositoryWrapperMock.Setup(wrapper => wrapper.SaveChangesAsync()).ReturnsAsync(null);
        repositoryWrapperMock.Setup(wrapper => wrapper.SaveChangesAsync()).Throws(default(Exception));

        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(m => m.Map<HistoricalContext>(default)).Returns(this.testUpdateContext);
        var loggerMock = new Mock<ILoggerService>();

        var handler = new UpdateHistoricalContextHandler(repositoryWrapperMock.Object, mapperMock.Object, loggerMock.Object, mockLocalizerCannotFind.Object, mockLocalizerValidation.Object, mockLocalizerFieldNames.Object);

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
        int id = this.testCreateContext.Id;

        // Act
        var response = await this.Client.Delete(id, this.TokenStorage.AdminAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Delete_TokenNotPassed_ReturnsUnathorized()
    {
        // Arrange
        int id = this.testCreateContext.Id;

        // Act
        var response = await this.Client.Delete(id);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Delete_NotAdminTokenPassed_ReturnsForbidden()
    {
        // Arrange
        int id = this.testCreateContext.Id;

        // Act
        var response = await this.Client.Delete(id, this.TokenStorage.UserAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task Delete_WithInvalidData_ReturnsBadRequest()
    {
        // Arrange
        int id = -100;

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

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            StreetcodeContentExtracter.Remove(this.testStreetcodeContent);
            HistoricalContextExtracter.Remove(this.testCreateContext);
        }

        base.Dispose(disposing);
    }
}