using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Team.Position.GetAll;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Team.Position;

public class GetAllWithTeamMembersTest
{
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizer;

    public GetAllWithTeamMembersTest()
    {
        _mockMapper = new Mock<IMapper>();
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockLogger = new Mock<ILoggerService>();
        _mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_WhenTypeIsCorrect()
    {
        // Arrange
        SetupMapMethod(GetListPositionDto());
        SetupGetAllAsyncMethod(GetPositionsList());

        var handler = new GetAllWithTeamMembersHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizer.Object);

        // Act
        var result = await handler.Handle(new GetAllWithTeamMembersQuery(), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.IsType<List<PositionDTO>>(result.ValueOrDefault));
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_WhenCountMatch()
    {
        // Arrange
        SetupMapMethod(GetListPositionDto());
        SetupGetAllAsyncMethod(GetPositionsList());

        var handler = new GetAllWithTeamMembersHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizer.Object);

        // Act
        var result = await handler.Handle(new GetAllWithTeamMembersQuery(), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.Equal(GetPositionsList().Count(), result.Value.Count()));
    }

    private static IEnumerable<Positions> GetPositionsList()
    {
        var partners = new List<Positions>
        {
            new Positions
            {
                Id = 1,
                TeamMembers = new List<TeamMember>
                {
                    new TeamMember { Id = 2 },
                },
            },
            new Positions
            {
                Id = 2,
            },
        };
        return partners;
    }

    private static List<PositionDTO> GetListPositionDto()
    {
        var positionDto = new List<PositionDTO>
        {
            new PositionDTO
            {
                Id = 1,
            },
            new PositionDTO
            {
                Id = 2,
            },
        };
        return positionDto;
    }

    private void SetupMapMethod(IEnumerable<PositionDTO> positionDtOs)
    {
        _mockMapper.Setup(x => x.Map<IEnumerable<PositionDTO>>(It.IsAny<IEnumerable<Positions>>()))
            .Returns(positionDtOs);
    }

    private void SetupGetAllAsyncMethod(IEnumerable<Positions> positions)
    {
        _mockRepository
            .Setup(x => x.PositionRepository
                .GetAllAsync(
                    null,
                    It.IsAny<Func<IQueryable<Positions>,
                    IIncludableQueryable<Positions, object>>>()))
            .ReturnsAsync(positions);
    }
}
