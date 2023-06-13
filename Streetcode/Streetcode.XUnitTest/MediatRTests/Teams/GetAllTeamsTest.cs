using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.MediatR.Team.GetAll;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Teams;

public class GetAllTeamTest
{
    private Mock<IRepositoryWrapper> _mockRepository;
    private Mock<IMapper> _mockMapper;

    public GetAllTeamTest()
    {
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockMapper = new Mock<IMapper>();
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_CorrectType()
    {
        //Arrange
        _mockRepository.Setup(x => x.TeamRepository.GetAllAsync(
            null,
            It.IsAny<Func<IQueryable<TeamMember>, IIncludableQueryable<TeamMember, object>>>()))
            .ReturnsAsync(GetTeamList());

        _mockMapper
            .Setup(x => x.Map<IEnumerable<TeamMemberDTO>>(It.IsAny<IEnumerable<TeamMember>>()))
            .Returns(GetListTeamDTO());

        var handler = new GetAllTeamHandler(_mockRepository.Object, _mockMapper.Object);

        //Act
        var result = await handler.Handle(new GetAllTeamQuery(), CancellationToken.None);

        //Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.IsType<List<TeamMemberDTO>>(result.ValueOrDefault)
        );
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_CountMatch()
    {
        //Arrange
        _mockRepository.Setup(x => x.TeamRepository.GetAllAsync(
            null,
            It.IsAny<Func<IQueryable<TeamMember>, IIncludableQueryable<TeamMember, object>>>()))
            .ReturnsAsync(GetTeamList());

        _mockMapper
            .Setup(x => x
            .Map<IEnumerable<TeamMemberDTO>>(It.IsAny<IEnumerable<TeamMember>>()))
            .Returns(GetListTeamDTO());

        var handler = new GetAllTeamHandler(_mockRepository.Object, _mockMapper.Object);

        //Act
        var result = await handler.Handle(new GetAllTeamQuery(), CancellationToken.None);

        //Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.Equal(GetTeamList().Count(), result.Value.Count())
        );
    }

    [Fact]
    public async Task ShouldThrowExeption_IdNotExist()
    {
        //Arrange
        var expectedError = "Cannot find any team";

        _mockRepository.Setup(x => x.TeamRepository.GetAllAsync(
            null,
            It.IsAny<Func<IQueryable<TeamMember>, IIncludableQueryable<TeamMember, object>>>()))
            .ReturnsAsync(GetTeamListWithNotExistingId());

        var handler = new GetAllTeamHandler(_mockRepository.Object, _mockMapper.Object);

        //Act
        var result = await handler.Handle(new GetAllTeamQuery(), CancellationToken.None);

        //Assert
        Assert.Equal(expectedError, result.Errors.First().Message);

        _mockMapper.Verify(x => x.Map<IEnumerable<TeamMemberDTO>>(It.IsAny<IEnumerable<TeamMember>>()), Times.Never);
    }

    private static IEnumerable<TeamMember> GetTeamList()
    {
        var team_members = new List<TeamMember>
        {
            new TeamMember
            {
                Id = 1
            },

            new TeamMember
            {
                Id = 2
            }
        };

        return team_members;
    }

    private static List<TeamMember>? GetTeamListWithNotExistingId()
    {
        return null;
    }

    private static List<TeamMemberDTO> GetListTeamDTO()
    {
        var team_membersDTO = new List<TeamMemberDTO>
        {
            new TeamMemberDTO
            {
                Id = 1
            },

            new TeamMemberDTO
            {
                Id = 2,
            }
        };

        return team_membersDTO;
    }
}
