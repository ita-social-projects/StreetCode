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
    private readonly Mock<IRepositoryWrapper> _mockRepo;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILoggerService> _mockLogger;

    public GetByTitlePositionTest()
    {
        _mockRepo = new Mock<IRepositoryWrapper>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILoggerService>();
    }

    private static string _title = "test_title";

    private readonly Positions context = new Positions
    {
        Id = 1,
        Position = _title
    };

    private readonly PositionDTO contextDto = new PositionDTO
    {
        Id = 1,
        Position = _title
    };

    async Task SetupRepository(Positions positions)
    {
        _mockRepo.Setup(repo => repo.PositionRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Positions, bool>>>(),
                It.IsAny<Func<IQueryable<Positions>,
                    IIncludableQueryable<Positions, object>>>()))
            .ReturnsAsync(positions);
    }

    async Task SetupMapper(PositionDTO positionsDto)
    {
        _mockMapper.Setup(x => x.Map<PositionDTO>(It.IsAny<Positions>()))
            .Returns(positionsDto);
    }

    [Fact]
    public async Task Handler_Returns_Matching_Element()
    {
        //Arrange
        await SetupRepository(context);
        await SetupMapper(contextDto);

        var handler = new GetByTitleTeamPositionHandler(_mockMapper.Object, _mockRepo.Object, _mockLogger.Object);

        //Act
        var result = await handler.Handle(new GetByTitleTeamPositionQuery(_title), CancellationToken.None);

        //Assert
        Assert.Multiple(
            () => Assert.IsType<PositionDTO>(result.Value),
            () => Assert.Equal(result.Value.Position, _title));
    }

    [Fact]
    public async Task Handler_Returns_NoMatching_Element()
    {
        //Arrange
        await SetupRepository(new Positions());
        await SetupMapper(new PositionDTO());

        var handler = new GetByTitleTeamPositionHandler(_mockMapper.Object, _mockRepo.Object, _mockLogger.Object);

        //Act
        var result = await handler.Handle(new GetByTitleTeamPositionQuery(_title), CancellationToken.None);

        //Assert
        Assert.Multiple(
            () => Assert.IsType<PositionDTO>(result.Value),
            () => Assert.Null(result.Value.Position));
    }
}