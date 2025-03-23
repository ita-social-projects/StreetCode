using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Team.Position.GetById;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Team.Position;

public class GetByIdPositionTest
{
    private const int Id = 1;
    private const string Title = "some title 1";
    private readonly Mock<IRepositoryWrapper> _mockRepo;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizer;

    private readonly Positions _position = new Positions()
    {
        Id = Id,
        Position = Title,
    };

    private readonly PositionDTO _positionDto = new PositionDTO
    {
        Id = Id,
        Position = Title,
    };

    public GetByIdPositionTest()
    {
        _mockRepo = new Mock<IRepositoryWrapper>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILoggerService>();
        _mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
    }

    [Fact]
    public async Task Handler_Returns_Matching_Element()
    {
        // Arrange
        SetupRepository(_position);
        SetupMapper(_positionDto);

        var handler = new GetByIdTeamPositionHandler(_mockMapper.Object, _mockRepo.Object, _mockLogger.Object, _mockLocalizer.Object);

        // Act
        var result = await handler.Handle(new GetByIdTeamPositionQuery(Id), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.IsType<PositionDTO>(result.Value),
            () => Assert.True(result.Value.Id.Equals(Id)));
    }

    [Fact]
    public async Task Handler_Returns_NoMatching_Element()
    {
        // Arrange
        SetupRepository(new Positions());
        SetupMapper(new PositionDTO());

        var handler = new GetByIdTeamPositionHandler(_mockMapper.Object, _mockRepo.Object, _mockLogger.Object, _mockLocalizer.Object);

        // Act
        var result = await handler.Handle(new GetByIdTeamPositionQuery(Id), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.IsType<PositionDTO>(result.Value),
            () => Assert.False(result.Value.Id.Equals(Id)));
    }

    private void SetupRepository(Positions positions)
    {
        _mockRepo
            .Setup(repo => repo.PositionRepository
                .GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Positions, bool>>>(),
                    It.IsAny<Func<IQueryable<Positions>,
                    IIncludableQueryable<Positions, object>>>()))
            .ReturnsAsync(positions);
    }

    private void SetupMapper(PositionDTO positionsDto)
    {
        _mockMapper
            .Setup(x => x.Map<PositionDTO>(It.IsAny<Positions>()))
            .Returns(positionsDto);
    }
}
