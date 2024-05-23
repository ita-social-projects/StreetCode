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
    private readonly Mock<IRepositoryWrapper> _mockRepo;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILoggerService> _mockLogger;

    public GetByIdPositionTest()
    {
        _mockRepo = new Mock<IRepositoryWrapper>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILoggerService>();
    }

    private const int _id = 1;
    private const string _title = "some title 1";

    private readonly Positions position = new Positions()
    {
        Id = _id,
        Position = _title
    };

    private readonly PositionDTO positionDto = new PositionDTO
    {
        Id = _id,
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
        await SetupRepository(position);
        await SetupMapper(positionDto);

        var handler = new GetByIdTeamPositionHandler(_mockMapper.Object, _mockRepo.Object, _mockLogger.Object);

        //Act
        var result = await handler.Handle(new GetByIdTeamPositionQuery(_id), CancellationToken.None);

        //Assert
        Assert.Multiple(
            () => Assert.IsType<PositionDTO>(result.Value),
            () => Assert.True(result.Value.Id.Equals(_id)));
    }

    [Fact]
    public async Task Handler_Returns_NoMatching_Element()
    {
        //Arrange
        await SetupRepository(new Positions());
        await SetupMapper(new PositionDTO());

        var handler = new GetByIdTeamPositionHandler(_mockMapper.Object, _mockRepo.Object, _mockLogger.Object);

        //Act
        var result = await handler.Handle(new GetByIdTeamPositionQuery(_id), CancellationToken.None);

        //Assert
        Assert.Multiple(
            () => Assert.IsType<PositionDTO>(result.Value),
            () => Assert.False(result.Value.Id.Equals(_id)));
    }
}