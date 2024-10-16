using System.Linq.Expressions;
using System.Net;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Team.Position.Delete;
using Streetcode.BLL.MediatR.Team.Position.Update;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.DAL.Repositories.Interfaces.Team;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.AdditionalContent.Positions;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Additional;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.AdditionalContent;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.AdditionalContent;

[Collection("Authorization")]
public class TeamPositionsControllerTests : BaseAuthorizationControllerTests<TeamPositionsClient>,
    IClassFixture<CustomWebApplicationFactory<Program>>
{
    private Positions _testCreatePosition;
    private Positions _testUpdatePosition;
    private StreetcodeContent _testStreetcodeContent;

    public TeamPositionsControllerTests(CustomWebApplicationFactory<Program> factory, TokenStorage tokenStorage)
        : base(factory, "/api/Position", tokenStorage)
    {
        int uniqueId = UniqueNumberGenerator.GenerateInt();
        this._testCreatePosition = TeamPositionsExtracter.Extract(uniqueId);
        this._testUpdatePosition = TeamPositionsExtracter.Extract(uniqueId);
        this._testStreetcodeContent = StreetcodeContentExtracter
            .Extract(
                uniqueId,
                uniqueId,
                Guid.NewGuid().ToString());
    }

    public override void Dispose()
    {
        StreetcodeContentExtracter.Remove(this._testStreetcodeContent);
        TeamPositionsExtracter.Remove(this._testCreatePosition);
    }

    [Fact]
    public async Task GetAll_ReturnSuccessStatusCode()
    {
        var response = await this.client.GetAllAsync();
        var returnedValue =
            CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<PositionDTO>>(response.Content);

        Assert.True(response.IsSuccessStatusCode);
        Assert.NotNull(returnedValue);
    }

    [Fact]
    public async Task GetAllWithTeamMembers_ReturnSuccessStatusCode()
    {
        var response = await this.client.GetAllWithTeamMembers();
        var returnedValue =
            CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<PositionDTO>>(response.Content);

        Assert.True(response.IsSuccessStatusCode);
        Assert.NotNull(returnedValue);
    }

    [Fact]
    public async Task GetById_ReturnSuccessStatusCode()
    {
        Positions expectedPosition = this._testCreatePosition;
        var response = await this.client.GetByIdAsync(expectedPosition.Id);

        var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<PositionDTO>(response.Content);

        Assert.True(response.IsSuccessStatusCode);
        Assert.NotNull(returnedValue);
        Assert.Multiple(
            () => Assert.Equal(expectedPosition.Id, returnedValue?.Id),
            () => Assert.Equal(expectedPosition.Position, returnedValue?.Position));
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
    [ExtractCreateTestPosition]
    public async Task Create_ReturnsSuccessStatusCode()
    {
        // Arrange
        var positionCreateDto = ExtractCreateTestPosition.PositionForTest;

        // Act
        var response = await client.CreateAsync(positionCreateDto, _tokenStorage.AdminAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    [ExtractCreateTestPosition]
    public async Task Create_TokenNotPassed_ReturnsUnauthorized()
    {
        // Arrange
        var positionCreateDto = ExtractCreateTestPosition.PositionForTest;

        // Act
        var response = await client.CreateAsync(positionCreateDto);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    [ExtractCreateTestPosition]
    public async Task Create_NotAdminTokenPassed_ReturnsForbidden()
    {
        // Arrange
        var positionCreateDto = ExtractCreateTestPosition.PositionForTest;

        // Act
        var response = await client.CreateAsync(positionCreateDto, _tokenStorage.UserAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }


    [Fact]
    [ExtractCreateTestPosition]
    public async Task Create_CreatesNewTeamPositionContext()
    {
        // Arrange
        var positionCreateDto = ExtractCreateTestPosition.PositionForTest;

        // Act
        var response = await client.CreateAsync(positionCreateDto, _tokenStorage.AdminAccessToken);
        var getResponse = await client.GetByTitle(positionCreateDto.Position);
        var fetchedStreetcode = CaseIsensitiveJsonDeserializer.Deserialize<Positions>(getResponse.Content);

        // Assert
        Assert.Equal(positionCreateDto.Position, fetchedStreetcode.Position);
    }

    [Fact]
    [ExtractCreateTestPosition]
    public async Task Create_WithInvalidData_ReturnsBadRequest()
    {
        // Arrange
        var positionCreateDto = ExtractCreateTestPosition.PositionForTest;
        positionCreateDto.Position = null; // Invalid data

        // Act
        var response = await client.CreateAsync(positionCreateDto, this._tokenStorage.AdminAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    [ExtractCreateTestPosition]
    public async Task Create_WithExistingTeamPosition_ReturnsConflict()
    {
        // Arrange
        var positionCreateDto = ExtractCreateTestPosition.PositionForTest;
        positionCreateDto.Position = _testCreatePosition.Position;

        // Act
        var response = await client.CreateAsync(positionCreateDto, this._tokenStorage.AdminAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    [ExtractUpdateTestPosition]
    public async Task Update_ReturnSuccessStatusCode()
    {
        // Arrange
        var positionCreateDto = ExtractUpdateTestPosition.PositionForTest;
        positionCreateDto.Id = this._testCreatePosition.Id;

        // Act
        var response = await client.UpdateAsync(positionCreateDto, this._tokenStorage.AdminAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    [ExtractUpdateTestPosition]
    public async Task Update_Incorect_ReturnBadRequest()
    {
        // Arrange
        var positionCreateDto = ExtractUpdateTestPosition.PositionForTest;

        // Act
        var response = await client.UpdateAsync(positionCreateDto, this._tokenStorage.AdminAccessToken);

        // Assert
        Assert.Multiple(
            () => Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode),
            () => Assert.False(response.IsSuccessStatusCode));
    }

    [Fact]
    [ExtractUpdateTestPosition]
    public async Task Update_TokenNotPassed_ReturnsUnauthorized()
    {
        // Arrange
        var positionCreateDto = ExtractUpdateTestPosition.PositionForTest;

        // Act
        var response = await client.UpdateAsync(positionCreateDto);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    [ExtractUpdateTestPosition]
    public async Task Update_NotAdminTokenPassed_ReturnsForbidden()
    {
        // Arrange
        var positionCreateDto = ExtractUpdateTestPosition.PositionForTest;

        // Act
        var response = await client.UpdateAsync(positionCreateDto, this._tokenStorage.UserAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    [ExtractUpdateTestPosition]
    public async Task Update_WithInvalidData_ReturnsBadRequest()
    {
        // Arrange
        var positionCreateDto = ExtractUpdateTestPosition.PositionForTest;
        positionCreateDto.Position = null; // Invalid data

        // Act
        var response = await client.UpdateAsync(positionCreateDto, this._tokenStorage.AdminAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    [ExtractUpdateTestPosition]
    public async Task Update_WithExistingTitle_ReturnsBadRequest()
    {
        // Arrange
        var positionCreateDto = ExtractUpdateTestPosition.PositionForTest;
        positionCreateDto.Id = this._testCreatePosition.Id - 1;
        positionCreateDto.Position = this._testUpdatePosition.Position;

        // Act
        var response = await client.UpdateAsync(positionCreateDto, this._tokenStorage.AdminAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    [ExtractUpdateTestPosition]
    public async Task Update_ChangesNotSaved_ReturnsBadRequest()
    {
        // Arrange
        Expression<Func<Positions, bool>> testExpression = x => x.Id == this._testUpdatePosition.Id;

        var positionCreateDto = ExtractUpdateTestPosition.PositionForTest;
        positionCreateDto.Id = this._testUpdatePosition.Id;

        var repositoryMock = new Mock<IPositionRepository>();
        var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
        repositoryMock.Setup(r => r.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Positions, bool>>>(), default))
            .ReturnsAsync((Expression<Func<Positions, bool>> expr, IIncludableQueryable<Positions, bool> include) =>
            {
                var compiledExpr = expr.Compile();
                return compiledExpr(this._testUpdatePosition) ? this._testUpdatePosition : null;
            });

        repositoryWrapperMock.SetupGet(wrapper => wrapper.PositionRepository).Returns(repositoryMock.Object);
        repositoryWrapperMock.Setup(wrapper => wrapper.SaveChangesAsync()).ReturnsAsync(null);
        repositoryWrapperMock.Setup(wrapper => wrapper.SaveChangesAsync()).Throws(default(Exception));

        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(m => m.Map<Positions>(default)).Returns(this._testUpdatePosition);
        var loggerMock = new Mock<ILoggerService>();

        var handler = new UpdateTeamPositionHandler(repositoryWrapperMock.Object, mapperMock.Object, loggerMock.Object);

        var query = new UpdateTeamPositionCommand(positionCreateDto);
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
        int id = this._testCreatePosition.Id;

        // Act
        var response = await this.client.Delete(id, this._tokenStorage.AdminAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Delete_TokenNotPassed_ReturnsUnathorized()
    {
        // Arrange
        int id = this._testCreatePosition.Id;

        // Act
        var response = await this.client.Delete(id);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Delete_NotAdminTokenPassed_ReturnsForbidden()
    {
        // Arrange
        int id = this._testCreatePosition.Id;

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
        int id = this._testUpdatePosition.Id;

        var repositoryMock = new Mock<IPositionRepository>();
        var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
        repositoryMock.Setup(r => r.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Positions, bool>>>(), default))
            .ReturnsAsync(this._testUpdatePosition);
        repositoryMock.Setup(r => r.Delete(default));

        repositoryWrapperMock.SetupGet(wrapper => wrapper.PositionRepository).Returns(repositoryMock.Object);
        repositoryWrapperMock.Setup(wrapper => wrapper.SaveChangesAsync()).ReturnsAsync(null);
        repositoryWrapperMock.Setup(wrapper => wrapper.SaveChangesAsync()).Throws(default(Exception));

        var loggerMock = new Mock<ILoggerService>();

        var handler = new DeleteTeamPositionHandler(repositoryWrapperMock.Object, loggerMock.Object);

        var query = new DeleteTeamPositionCommand(id);
        var cancellationToken = CancellationToken.None;
        // Act
        var result = await handler.Handle(query, cancellationToken);

        // Assert
        Assert.False(result.IsSuccess);
    }
}
