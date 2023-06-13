using AutoMapper;
using Moq;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.MediatR.Team.Delete;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Teams;

public class DeleteTeamTest
{
    private Mock<IMapper> _mockMapper;
    private Mock<IRepositoryWrapper> _mockRepository;

    public DeleteTeamTest()
    {
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockMapper = new Mock<IMapper>();
    }

    [Fact]
    public async Task ShouldDeleteSuccessfully()
    {
        //Arrange
        var testTeam = GetTeam();

        _mockMapper.Setup(x => x.Map<TeamMemberDTO>(It.IsAny<TeamMember>()))
            .Returns(GetTeamDTO());

        _mockRepository.Setup(x => x.TeamRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<TeamMember, bool>>>(), null))
            .ReturnsAsync(testTeam);

        var handler = new DeleteTeamHandler(_mockRepository.Object, _mockMapper.Object);

        //Act
        var result = await handler.Handle(new DeleteTeamQuery(testTeam.Id), CancellationToken.None);

        //Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.True(result.IsSuccess)
        );

        _mockRepository.Verify(x => x.TeamRepository.Delete(It.Is<TeamMember>(x => x.Id == testTeam.Id)), Times.Once);
        _mockRepository.Verify(x => x.SaveChanges(), Times.Once);
    }

    [Fact]
    public async Task ShouldThrowExeption_IdNotExisting()
    {
        //Arrange
        var testTeam = GetTeam();
        var expectedError = "No team with such id";

        _mockRepository.Setup(x => x.TeamRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<TeamMember, bool>>>(), null))
            .ReturnsAsync(GetTeamWithNotExistingId());

        //Act
        var handler = new DeleteTeamHandler(_mockRepository.Object, _mockMapper.Object);

        var result = await handler.Handle(new DeleteTeamQuery(testTeam.Id), CancellationToken.None);

        //Assert
        Assert.Equal(expectedError, result.Errors.First().Message);

        _mockRepository.Verify(x => x.TeamRepository.Delete(It.IsAny<TeamMember>()), Times.Never);
    }

    [Fact]
    public async Task ShouldThrowExeption_SaveChangesAsyncIsNotSuccessful()
    {
        //Arrange
        var testTeam = GetTeam();
        var expectedError = "The team wasn`t added";

        _mockMapper.Setup(x => x.Map<TeamMemberDTO>(It.IsAny<TeamMember>()))
            .Returns(GetTeamDTO());

        _mockRepository.Setup(x => x.TeamRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<TeamMember, bool>>>(), null))
            .ReturnsAsync(testTeam);
        _mockRepository.Setup(x => x.SaveChanges())
            .Throws(new Exception(expectedError));

        //Act
        var handler = new DeleteTeamHandler(_mockRepository.Object, _mockMapper.Object);

        var result = await handler.Handle(new DeleteTeamQuery(testTeam.Id), CancellationToken.None);

        //Assert
        Assert.Equal(expectedError, result.Errors.First().Message);
    }

    private static TeamMember GetTeam()
    {
        return new TeamMember
        {
            Id = 1
        };
    }

    private static TeamMemberDTO GetTeamDTO()
    {
        return new TeamMemberDTO();
    }

    private static TeamMember? GetTeamWithNotExistingId()
    {
        return null;
    }
}
