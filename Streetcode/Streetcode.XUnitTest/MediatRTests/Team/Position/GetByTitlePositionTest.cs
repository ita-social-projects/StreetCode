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
    private const string title = "test_title";
    private readonly Mock<IRepositoryWrapper> mockRepo;
    private readonly Mock<IMapper> mockMapper;
    private readonly Mock<ILoggerService> mockLogger;

    private readonly Positions context = new Positions
    {
        Id = 1,
        Position = title,
    };

    private readonly PositionDTO contextDto = new PositionDTO
    {
        Id = 1,
        Position = title,
    };

    public GetByTitlePositionTest()
    {
        this.mockRepo = new Mock<IRepositoryWrapper>();
        this.mockMapper = new Mock<IMapper>();
        this.mockLogger = new Mock<ILoggerService>();
    }

    [Fact]
    public async Task Handler_Returns_Matching_Element()
    {
        // Arrange
        this.SetupRepository(this.context);
        this.SetupMapper(this.contextDto);

        var handler = new GetByTitleTeamPositionHandler(this.mockMapper.Object, this.mockRepo.Object, this.mockLogger.Object);

        // Act
        var result = await handler.Handle(new GetByTitleTeamPositionQuery(title), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.IsType<PositionDTO>(result.Value),
            () => Assert.Equal(title, result.Value.Position));
    }

    [Fact]
    public async Task Handler_Returns_NoMatching_Element()
    {
        // Arrange
        this.SetupRepository(new Positions());
        this.SetupMapper(new PositionDTO());

        var handler = new GetByTitleTeamPositionHandler(this.mockMapper.Object, this.mockRepo.Object, this.mockLogger.Object);

        // Act
        var result = await handler.Handle(new GetByTitleTeamPositionQuery(title), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.IsType<PositionDTO>(result.Value),
            () => Assert.Null(result.Value.Position));
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
