using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Team.Position.GetByTitle;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Team.Position;

public class GetByTitlePositionTest
{
    private const string _title = "test_title";
    private readonly Mock<IRepositoryWrapper> _mockRepo;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILoggerService> _mockLogger;

    private readonly Positions context = new Positions
    {
        Id = 1,
        Position = _title,
    };

    private readonly PositionDTO contextDto = new PositionDTO
    {
        Id = 1,
        Position = _title,
    };

    public GetByTitlePositionTest()
    {
        this._mockRepo = new Mock<IRepositoryWrapper>();
        this._mockMapper = new Mock<IMapper>();
        this._mockLogger = new Mock<ILoggerService>();
    }

    [Fact]
    public async Task Handler_Returns_Matching_Element()
    {
        // Arrange
        this.SetupRepository(this.context);
        this.SetupMapper(this.contextDto);

        var handler = new GetByTitleTeamPositionHandler(this._mockMapper.Object, this._mockRepo.Object, this._mockLogger.Object);

        // Act
        var result = await handler.Handle(new GetByTitleTeamPositionQuery(_title), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.IsType<PositionDTO>(result.Value),
            () => Assert.Equal(_title, result.Value.Position));
    }

    [Fact]
    public async Task Handler_Returns_NoMatching_Element()
    {
        // Arrange
        this.SetupRepository(new Positions());
        this.SetupMapper(new PositionDTO());

        var handler = new GetByTitleTeamPositionHandler(this._mockMapper.Object, this._mockRepo.Object, this._mockLogger.Object);

        // Act
        var result = await handler.Handle(new GetByTitleTeamPositionQuery(_title), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.IsType<PositionDTO>(result.Value),
            () => Assert.Null(result.Value.Position));
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
