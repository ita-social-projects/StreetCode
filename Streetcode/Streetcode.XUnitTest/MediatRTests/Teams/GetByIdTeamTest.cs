using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.MediatR.Team.GetById;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Teams;

public class GetTeamByIdTest
{
    private Mock<IRepositoryWrapper> _mockRepository;
    private Mock<IMapper> _mockMapper;

    public GetTeamByIdTest()
    {
        _mockMapper = new Mock<IMapper>();
        _mockRepository = new Mock<IRepositoryWrapper>();
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_ExistingId()
    {
        //Arrange
        var testTeam = GetTeam();

        _mockRepository.Setup(x => x.TeamRepository
            .GetSingleOrDefaultAsync(
               It.IsAny<Expression<Func<TeamMember, bool>>>(),
                It.IsAny<Func<IQueryable<TeamMember>,
                IIncludableQueryable<TeamMember, object>>>()))
            .ReturnsAsync(testTeam);

        _mockMapper
            .Setup(x => x
            .Map<TeamMemberDTO>(It.IsAny<TeamMember>()))
            .Returns(GetTeamDTO());

        var handler = new GetByIdTeamHandler(_mockRepository.Object, _mockMapper.Object);

        //Act
        var result = await handler.Handle(new GetByIdTeamQuery(testTeam.Id), CancellationToken.None);

        //Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.True(result.IsSuccess),
            () => Assert.Equal(result.Value.Id, testTeam.Id)
        );
    }

    [Fact]
    public async Task ShouldReturnErrorResponse_NotExistingId()
    {
        //Arrange
        var testTeam = GetTeam();
        var expectedError = $"Cannot find any team with corresponding id: {testTeam.Id}";

        _mockRepository.Setup(x => x.TeamRepository
            .GetSingleOrDefaultAsync(
               It.IsAny<Expression<Func<TeamMember, bool>>>(),
                It.IsAny<Func<IQueryable<TeamMember>,
                IIncludableQueryable<TeamMember, object>>>()))
            .ReturnsAsync(GetTeamWithNotExistingId());

        _mockMapper
            .Setup(x => x
            .Map<TeamMemberDTO>(It.IsAny<TeamMember>()))
            .Returns(GetTeamDTOWithNotExistingId());

        var handler = new GetByIdTeamHandler(_mockRepository.Object, _mockMapper.Object);

        //Act
        var result = await handler.Handle(new GetByIdTeamQuery(testTeam.Id), CancellationToken.None);

        //Assert
        Assert.Multiple(
            () => Assert.True(result.IsFailed),
            () => Assert.Equal(expectedError, result.Errors.First().Message)
        );
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_CorrectType()
    {
        //Arrange
        var testTeam = GetTeam();

        _mockRepository.Setup(x => x.TeamRepository
            .GetSingleOrDefaultAsync(
               It.IsAny<Expression<Func<TeamMember, bool>>>(),
                It.IsAny<Func<IQueryable<TeamMember>,
                IIncludableQueryable<TeamMember, object>>>()))
            .ReturnsAsync(testTeam);

        _mockMapper
            .Setup(x => x
            .Map<TeamMemberDTO>(It.IsAny<TeamMember>()))
            .Returns(GetTeamDTO());

        var handler = new GetByIdTeamHandler(_mockRepository.Object, _mockMapper.Object);

        //Act
        var result = await handler.Handle(new GetByIdTeamQuery(testTeam.Id), CancellationToken.None);

        //Assert
        Assert.Multiple(
            () => Assert.NotNull(result.ValueOrDefault),
            () => Assert.IsType<TeamMemberDTO>(result.ValueOrDefault)
        );
    }

    private static TeamMember GetTeam()
    {
        return new TeamMember
        {
            Id = 1
        };
    }
    private static TeamMember? GetTeamWithNotExistingId()
    {
        return null;
    }

    private static TeamMemberDTO GetTeamDTO()
    {
        return new TeamMemberDTO
        {
            Id = 1
        };
    }

    private static TeamMemberDTO? GetTeamDTOWithNotExistingId()
    {
        return null;
    }
}
