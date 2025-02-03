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
    private readonly Mock<IMapper> mockMapper;
    private readonly Mock<IRepositoryWrapper> mockRepository;
    private readonly Mock<ILoggerService> mockLogger;
    private readonly Mock<IStringLocalizer<CannotFindSharedResource>> mockLocalizer;

    public GetAllWithTeamMembersTest()
    {
        this.mockMapper = new Mock<IMapper>();
        this.mockRepository = new Mock<IRepositoryWrapper>();
        this.mockLogger = new Mock<ILoggerService>();
        this.mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_WhenTypeIsCorrect()
    {
        // Arrange
        this.SetupMapMethod(GetListPositionDTO());
        this.SetupGetAllAsyncMethod(GetPositionsList());

        var handler = new GetAllWithTeamMembersHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizer.Object);

        // Act
        var result = await handler.Handle(new GetAllWithTeamMembersQuery(), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.IsType<List<PositionDto>>(result.ValueOrDefault));
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_WhenCountMatch()
    {
        // Arrange
        this.SetupMapMethod(GetListPositionDTO());
        this.SetupGetAllAsyncMethod(GetPositionsList());

        var handler = new GetAllWithTeamMembersHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizer.Object);

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

    private static List<PositionDto> GetListPositionDTO()
    {
        var positionDTO = new List<PositionDto>
        {
            new PositionDto
            {
                Id = 1,
            },
            new PositionDto
            {
                Id = 2,
            },
        };
        return positionDTO;
    }

    private void SetupMapMethod(IEnumerable<PositionDto> positionDTOs)
    {
        this.mockMapper.Setup(x => x.Map<IEnumerable<PositionDto>>(It.IsAny<IEnumerable<Positions>>()))
            .Returns(positionDTOs);
    }

    private void SetupGetAllAsyncMethod(IEnumerable<Positions> positions)
    {
        this.mockRepository
            .Setup(x => x.PositionRepository
                .GetAllAsync(
                    null,
                    It.IsAny<Func<IQueryable<Positions>,
                    IIncludableQueryable<Positions, object>>>()))
            .ReturnsAsync(positions);
    }
}
