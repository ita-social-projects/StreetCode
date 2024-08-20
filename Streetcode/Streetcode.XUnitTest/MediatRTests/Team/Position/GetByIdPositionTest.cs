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
    private const int _id = 1;
    private const string _title = "some title 1";
    private readonly Mock<IRepositoryWrapper> _mockRepo;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILoggerService> _mockLogger;

    private readonly Positions position = new Positions()
    {
        Id = _id,
        Position = _title,
    };

    private readonly PositionDTO positionDto = new PositionDTO
    {
        Id = _id,
        Position = _title,
    };

    public GetByIdPositionTest()
    {
        this._mockRepo = new Mock<IRepositoryWrapper>();
        this._mockMapper = new Mock<IMapper>();
        this._mockLogger = new Mock<ILoggerService>();
    }

    [Fact]
    public async Task Handler_Returns_Matching_Element()
    {
        // Arrange
        this.SetupRepository(this.position);
        this.SetupMapper(this.positionDto);

        var handler = new GetByIdTeamPositionHandler(this._mockMapper.Object, this._mockRepo.Object, this._mockLogger.Object);

        // Act
        var result = await handler.Handle(new GetByIdTeamPositionQuery(_id), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.IsType<PositionDTO>(result.Value),
            () => Assert.True(result.Value.Id.Equals(_id)));
    }

    [Fact]
    public async Task Handler_Returns_NoMatching_Element()
    {
        // Arrange
        this.SetupRepository(new Positions());
        this.SetupMapper(new PositionDTO());

        var handler = new GetByIdTeamPositionHandler(this._mockMapper.Object, this._mockRepo.Object, this._mockLogger.Object);

        // Act
        var result = await handler.Handle(new GetByIdTeamPositionQuery(_id), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.IsType<PositionDTO>(result.Value),
            () => Assert.False(result.Value.Id.Equals(_id)));
    }

    private void SetupRepository(Positions positions)
    {
        this._mockRepo
            .Setup(repo => repo.PositionRepository
                .GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Positions, bool>>>(),
                    It.IsAny<Func<IQueryable<Positions>,
                    IIncludableQueryable<Positions, object>>>()))
            .ReturnsAsync(positions);
    }

    private void SetupMapper(PositionDTO positionsDto)
    {
        this._mockMapper
            .Setup(x => x.Map<PositionDTO>(It.IsAny<Positions>()))
            .Returns(positionsDto);
    }
}
