using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Team.Position.GetAll;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Team.Position;

public class GetAllWithTeamMembersTest
{
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<ILoggerService> _mockLogger;

    public GetAllWithTeamMembersTest()
    {
        _mockMapper = new Mock<IMapper>();
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockLogger = new Mock<ILoggerService>();
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_WhenTypeIsCorrect()
    {
        //Arrange
        SetupMapMethod(GetListPositionDTO());
        SetupGetAllAsyncMethod(GetPositionsList());

        var handler = new GetAllWithTeamMembersHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);

        //Act
        var result = await handler.Handle(new GetAllWithTeamMembersQuery(), CancellationToken.None);

        //Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.IsType<List<PositionDTO>>(result.ValueOrDefault)
        );
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_WhenCountMatch()
    {
        //Arrange
        SetupMapMethod(GetListPositionDTO());
        SetupGetAllAsyncMethod(GetPositionsList());

        var handler = new GetAllWithTeamMembersHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);

        //Act
        var result = await handler.Handle(new GetAllWithTeamMembersQuery(), CancellationToken.None);

        //Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.Equal(GetPositionsList().Count(), result.Value.Count())
        );
    }

    private void SetupMapMethod(IEnumerable<PositionDTO> positionDTOs)
    {
        _mockMapper.Setup(x => x.Map<IEnumerable<PositionDTO>>(It.IsAny<IEnumerable<Positions>>()))
            .Returns(positionDTOs);
    }

    private void SetupGetAllAsyncMethod(IEnumerable<Positions> positions)
    {
        _mockRepository.Setup(x => x.PositionRepository.GetAllAsync(
                null,
                It.IsAny<Func<IQueryable<Positions>, IIncludableQueryable<Positions, object>>>()))
            .ReturnsAsync(positions);
    }

    private static IEnumerable<Positions> GetPositionsList()
    {
        var partners = new List<Positions>
        {
            new Positions
            {
                Id = 1
            },
            new Positions
            {
                Id = 2
            }
        };
        return partners;
    }

    private static List<Positions> GetPositionsListWithNotExistingId()
    {
        return new List<Positions>(); // Return an empty list instead of null
    }

    private static List<PositionDTO> GetListPositionDTO()
    {
        var PositionDTO = new List<PositionDTO>
        {
            new PositionDTO
            {
                Id = 1
            },
            new PositionDTO
            {
                Id = 2,
            }
        };
        return PositionDTO;
    }
}