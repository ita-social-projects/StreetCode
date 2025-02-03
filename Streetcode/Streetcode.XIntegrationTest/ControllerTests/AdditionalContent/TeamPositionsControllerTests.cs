using System.Linq.Expressions;
using System.Net;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Team.Position.Delete;
using Streetcode.BLL.MediatR.Team.Position.Update;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Media.Images;
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
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.MediaExtracter.Image;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.AdditionalContent;

[Collection("Authorization")]
public class TeamPositionsControllerTests : BaseAuthorizationControllerTests<TeamPositionsClient>,
    IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly Image testTeamMemberImage;
    private readonly TeamMember testTeamMember;
    private readonly Positions testCreatePosition;
    private readonly Positions testUpdatePosition;
    private readonly StreetcodeContent testStreetcodeContent;

    public TeamPositionsControllerTests(CustomWebApplicationFactory<Program> factory, TokenStorage tokenStorage)
        : base(factory, "/api/Position", tokenStorage)
    {
        int uniqueId = UniqueNumberGenerator.GenerateInt();
        this.testCreatePosition = TeamPositionsExtracter.Extract(uniqueId);
        this.testUpdatePosition = TeamPositionsExtracter.Extract(uniqueId);
        this.testTeamMemberImage = ImageExtracter.Extract(1);
        this.testTeamMember = TeamMemberExtracter.Extract(uniqueId);
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
            CaseIsensitiveJsonDeserializer.Deserialize<GetAllPositionsDto>(response.Content);

        Assert.True(response.IsSuccessStatusCode);
        Assert.NotNull(returnedValue);
    }

    // [Fact(Skip = "There is no Position that has TeamMembers, so it will fail without them.")]
    [Fact]
    public async Task GetAllWithTeamMembers_ReturnSuccessStatusCode()
    {
        TeamPositionsExtracter.AddTeamMemberPositions(this.testTeamMember.Id, this.testCreatePosition.Id);
        var response = await this.Client.GetAllWithTeamMembers();
        var returnedValue =
            CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<PositionDto>>(response.Content);

        Assert.True(response.IsSuccessStatusCode);
        Assert.NotNull(returnedValue);
    }

    [Fact]
    public async Task GetById_ReturnSuccessStatusCode()
    {
        Positions expectedPosition = this.testCreatePosition;
        var response = await this.Client.GetByIdAsync(expectedPosition.Id);

        var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<PositionDto>(response.Content);

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
        var response = await this.Client.GetByIdAsync(incorrectId);

        Assert.Multiple(
            () => Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode),
            () => Assert.False(response.IsSuccessStatusCode));
    }

    [Fact]
    [ExtractCreateTestPositionAttribute]
    public async Task Create_ReturnsSuccessStatusCode()
    {
        // Arrange
        var positionCreateDto = ExtractCreateTestPositionAttribute.PositionForTest;

        // Act
        var response = await this.Client.CreateAsync(positionCreateDto, this.TokenStorage.AdminAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    [ExtractCreateTestPositionAttribute]
    public async Task Create_TokenNotPassed_ReturnsUnauthorized()
    {
        // Arrange
        var positionCreateDto = ExtractCreateTestPositionAttribute.PositionForTest;

        // Act
        var response = await this.Client.CreateAsync(positionCreateDto);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    [ExtractCreateTestPositionAttribute]
    public async Task Create_NotAdminTokenPassed_ReturnsForbidden()
    {
        // Arrange
        var positionCreateDto = ExtractCreateTestPositionAttribute.PositionForTest;

        // Act
        var response = await this.Client.CreateAsync(positionCreateDto, this.TokenStorage.UserAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    [ExtractCreateTestPositionAttribute]
    public async Task Create_CreatesNewTeamPositionContext()
    {
        // Arrange
        var positionCreateDto = ExtractCreateTestPositionAttribute.PositionForTest;

        // Act
        await this.Client.CreateAsync(positionCreateDto, this.TokenStorage.AdminAccessToken);
        var getResponse = await this.Client.GetByTitle(positionCreateDto.Position);
        var fetchedStreetcode = CaseIsensitiveJsonDeserializer.Deserialize<Positions>(getResponse.Content);

        // Assert
        Assert.Equal(positionCreateDto.Position, fetchedStreetcode?.Position);
    }

    [Fact]
    [ExtractCreateTestPositionAttribute]
    public async Task Create_WithInvalidData_ReturnsBadRequest()
    {
        // Arrange
        var positionCreateDto = ExtractCreateTestPositionAttribute.PositionForTest;
        positionCreateDto.Position = null!; // Invalid data

        // Act
        var response = await this.Client.CreateAsync(positionCreateDto, this.TokenStorage.AdminAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    [ExtractCreateTestPositionAttribute]
    public async Task Create_WithExistingTeamPosition_ReturnsConflict()
    {
        // Arrange
        var positionCreateDto = ExtractCreateTestPositionAttribute.PositionForTest;
        positionCreateDto.Position = this.testCreatePosition.Position!;

        // Act
        var response = await this.Client.CreateAsync(positionCreateDto, this.TokenStorage.AdminAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    [ExtractUpdateTestPosition]
    public async Task Update_ReturnSuccessStatusCode()
    {
        // Arrange
        var positionCreateDto = ExtractUpdateTestPositionAttribute.PositionForTest;
        positionCreateDto.Id = this.testCreatePosition.Id;

        // Act
        var response = await this.Client.UpdateAsync(positionCreateDto, this.TokenStorage.AdminAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    [ExtractUpdateTestPosition]
    public async Task Update_Incorect_ReturnBadRequest()
    {
        // Arrange
        var positionCreateDto = ExtractUpdateTestPositionAttribute.PositionForTest;
        var incorrectPositionId = -10;
        positionCreateDto.Id = incorrectPositionId;

        // Act
        var response = await this.Client.UpdateAsync(positionCreateDto, this.TokenStorage.AdminAccessToken);

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
        var positionCreateDto = ExtractUpdateTestPositionAttribute.PositionForTest;

        // Act
        var response = await this.Client.UpdateAsync(positionCreateDto);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    [ExtractUpdateTestPosition]
    public async Task Update_NotAdminTokenPassed_ReturnsForbidden()
    {
        // Arrange
        var positionCreateDto = ExtractUpdateTestPositionAttribute.PositionForTest;

        // Act
        var response = await this.Client.UpdateAsync(positionCreateDto, this.TokenStorage.UserAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    [ExtractUpdateTestPosition]
    public async Task Update_WithInvalidData_ReturnsBadRequest()
    {
        // Arrange
        var positionCreateDto = ExtractUpdateTestPositionAttribute.PositionForTest;
        positionCreateDto.Position = null!; // Invalid data

        // Act
        var response = await this.Client.UpdateAsync(positionCreateDto, this.TokenStorage.AdminAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    [ExtractUpdateTestPosition]
    public async Task Update_WithExistingTitle_ReturnsBadRequest()
    {
        // Arrange
        var positionCreateDto = ExtractUpdateTestPositionAttribute.PositionForTest;
        positionCreateDto.Id = this.testCreatePosition.Id - 1;
        positionCreateDto.Position = this.testUpdatePosition.Position!;

        // Act
        var response = await this.Client.UpdateAsync(positionCreateDto, this.TokenStorage.AdminAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    [ExtractUpdateTestPosition]
    public async Task Update_ChangesNotSaved_ReturnsBadRequest()
    {
        // Arrange
        var positionCreateDto = ExtractUpdateTestPositionAttribute.PositionForTest;
        positionCreateDto.Id = this.testUpdatePosition.Id;

        var repositoryMock = new Mock<IPositionRepository>();
        var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
        var mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        repositoryMock.Setup(r => r.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Positions, bool>>>(), default))
            .ReturnsAsync((Expression<Func<Positions, bool>> expr, IIncludableQueryable<Positions, bool> include) =>
            {
                var compiledExpr = expr.Compile();
                return compiledExpr(this.testUpdatePosition) ? this.testUpdatePosition : null;
            });

        repositoryWrapperMock.SetupGet(wrapper => wrapper.PositionRepository).Returns(repositoryMock.Object);
        repositoryWrapperMock.Setup(wrapper => wrapper.SaveChangesAsync()).ReturnsAsync(null);
        repositoryWrapperMock.Setup(wrapper => wrapper.SaveChangesAsync()).Throws(default(Exception));

        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(m => m.Map<Positions>(default)).Returns(this.testUpdatePosition);
        var loggerMock = new Mock<ILoggerService>();

        var handler = new UpdateTeamPositionHandler(repositoryWrapperMock.Object, mapperMock.Object, loggerMock.Object, mockLocalizer.Object);

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
        int id = this.testCreatePosition.Id;

        // Act
        var response = await this.Client.Delete(id, this.TokenStorage.AdminAccessToken);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Delete_TokenNotPassed_ReturnsUnathorized()
    {
        // Arrange
        int id = this.testCreatePosition.Id;

        // Act
        var response = await this.Client.Delete(id);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Delete_NotAdminTokenPassed_ReturnsForbidden()
    {
        // Arrange
        int id = this.testCreatePosition.Id;

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
        int id = this.testUpdatePosition.Id;

        var repositoryMock = new Mock<IPositionRepository>();
        var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
        var mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        repositoryMock.Setup(r => r.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Positions, bool>>>(), default))
            .ReturnsAsync(this.testUpdatePosition);
        repositoryMock.Setup(r => r.Delete(default!));

        repositoryWrapperMock.SetupGet(wrapper => wrapper.PositionRepository).Returns(repositoryMock.Object);
        repositoryWrapperMock.Setup(wrapper => wrapper.SaveChangesAsync()).ReturnsAsync(null);
        repositoryWrapperMock.Setup(wrapper => wrapper.SaveChangesAsync()).Throws(default(Exception));

        var loggerMock = new Mock<ILoggerService>();

        var handler = new DeleteTeamPositionHandler(repositoryWrapperMock.Object, loggerMock.Object, mockLocalizer.Object);

        var query = new DeleteTeamPositionCommand(id);
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
            TeamPositionsExtracter.Remove(this.testCreatePosition);
            TeamPositionsExtracter.Remove(this.testUpdatePosition);
            ImageExtracter.Remove(this.testTeamMemberImage);
            TeamMemberExtracter.Remove(this.testTeamMember);
        }

        base.Dispose(disposing);
    }
}
