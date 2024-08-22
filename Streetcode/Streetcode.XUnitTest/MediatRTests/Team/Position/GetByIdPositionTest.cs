using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Team.Position.GetById;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Team.Position;

public class GetByIdPositionTest
{
    private const int id = 1;
    private const string title = "some title 1";
    private readonly Mock<IRepositoryWrapper> mockRepo;
    private readonly Mock<IMapper> mockMapper;
    private readonly Mock<ILoggerService> mockLogger;

    private readonly Positions position = new Positions()
    {
        Id = id,
        Position = title,
    };

    private readonly PositionDTO positionDto = new PositionDTO
    {
        Id = id,
        Position = title,
    };

    public GetByIdPositionTest()
    {
        this.mockRepo = new Mock<IRepositoryWrapper>();
        this.mockMapper = new Mock<IMapper>();
        this.mockLogger = new Mock<ILoggerService>();
    }

    [Fact]
    public async Task Handler_Returns_Matching_Element()
    {
        // Arrange
        this.SetupRepository(this.position);
        this.SetupMapper(this.positionDto);

        var handler = new GetByIdTeamPositionHandler(this.mockMapper.Object, this.mockRepo.Object, this.mockLogger.Object);

        // Act
        var result = await handler.Handle(new GetByIdTeamPositionQuery(id), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.IsType<PositionDTO>(result.Value),
            () => Assert.True(result.Value.Id.Equals(id)));
    }

    [Fact]
    public async Task Handler_Returns_NoMatching_Element()
    {
        // Arrange
        this.SetupRepository(new Positions());
        this.SetupMapper(new PositionDTO());

        var handler = new GetByIdTeamPositionHandler(this.mockMapper.Object, this.mockRepo.Object, this.mockLogger.Object);

        // Act
        var result = await handler.Handle(new GetByIdTeamPositionQuery(id), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.IsType<PositionDTO>(result.Value),
            () => Assert.False(result.Value.Id.Equals(id)));
    }

    private void SetupRepository(Positions positions)
    {
        this.mockRepo
            .Setup(repo => repo.PositionRepository
                .GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Positions, bool>>>(),
                    It.IsAny<Func<IQueryable<Positions>,
                    IIncludableQueryable<Positions, object>>>()))
            .ReturnsAsync(positions);
    }

    private void SetupMapper(PositionDTO positionsDto)
    {
        this.mockMapper
            .Setup(x => x.Map<PositionDTO>(It.IsAny<Positions>()))
            .Returns(positionsDto);
    }
}
