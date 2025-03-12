using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.MediatR.Team.GetAll;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Helpers;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Team;

public class GetAllTeamsTest
{
    private const string _testBase64String = "rVhhWrnh72xHfKGHg6YTV2H4ywe7BorrYUdILaKz0lQ=";

    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IBlobService> _mockBlobService;
    private readonly GetAllTeamHandler _handler;

    public GetAllTeamsTest()
    {
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockMapper = new Mock<IMapper>();
        _mockBlobService = new Mock<IBlobService>();
        _handler = new GetAllTeamHandler(
            _mockRepository.Object,
            _mockMapper.Object,
            _mockBlobService.Object);
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_CorrectType()
    {
        // Arrange
        var (teamMemberList, teamMemberDtoList) = GetTeamMemberObjectsLists();

        SetupPaginatedRepository(teamMemberList);
        SetupBlobService();
        SetupMapper(teamMemberDtoList);

        // Act
        var result = await _handler.Handle(new GetAllTeamQuery(), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.NotNull(result.Value),
            () => Assert.IsType<List<TeamMemberDTO>>(result.ValueOrDefault.TeamMembers));
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_CountMatch()
    {
        // Arrange
        var (teamMemberList, teamMemberDtoList) = GetTeamMemberObjectsLists();

        SetupPaginatedRepository(teamMemberList);
        SetupBlobService();
        SetupMapper(teamMemberDtoList);

        // Act
        var result = await _handler.Handle(new GetAllTeamQuery(), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.NotNull(result.Value),
            () => Assert.Equal(GetTeamMemberObjectsLists().Item2.Count, result.Value.TeamMembers.Count()));
    }

    [Fact]
    public async Task Handler_Returns_Correct_PageSize()
    {
        // Arrange
        const int pageSize = 2;
        var (teamMemberList, teamMemberDtoList) = GetTeamMemberObjectsLists();

        SetupPaginatedRepository(teamMemberList
            .Take(pageSize));
        SetupBlobService();
        SetupMapper(teamMemberDtoList
            .Take(pageSize)
            .ToList());

        // Act
        var result = await _handler.Handle(new GetAllTeamQuery(page: 1, pageSize: pageSize), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.IsType<List<TeamMemberDTO>>(result.Value.TeamMembers),
            () => Assert.Equal(pageSize, result.Value.TeamMembers.Count()));
    }

    private static (List<TeamMember>, List<TeamMemberDTO>) GetTeamMemberObjectsLists()
    {
        var teamMemberList = new List<TeamMember>()
        {
            new TeamMember()
            {
                Id = 1,
            },
            new TeamMember()
            {
                Id = 4,
            },
            new TeamMember()
            {
                Id = 6,
            },
        };
        var teamMemberDtoList = new List<TeamMemberDTO>()
        {
            new TeamMemberDTO()
            {
                Id = 1,
            },
            new TeamMemberDTO()
            {
                Id = 4,
            },
            new TeamMemberDTO()
            {
                Id = 6,
            },
        };

        return (teamMemberList, teamMemberDtoList);
    }

    private void SetupPaginatedRepository(IEnumerable<TeamMember> teamMemberList)
    {
        _mockRepository
            .Setup(repo => repo.TeamRepository.GetAllPaginated(
                It.IsAny<ushort?>(),
                It.IsAny<ushort?>(),
                It.IsAny<Expression<Func<TeamMember, TeamMember>>?>(),
                It.IsAny<Expression<Func<TeamMember, bool>>?>(),
                It.IsAny<Func<IQueryable<TeamMember>, IIncludableQueryable<TeamMember, object>>?>(),
                It.IsAny<Expression<Func<TeamMember, object>>?>(),
                It.IsAny<Expression<Func<TeamMember, object>>?>()))
            .Returns(PaginationResponse<TeamMember>.Create(teamMemberList.AsQueryable()));
    }

    private void SetupBlobService()
    {
        _mockBlobService
            .Setup(x => x.FindFileInStorageAsBase64(It.IsAny<string>()))
            .Returns(_testBase64String);
    }

    private void SetupMapper(IEnumerable<TeamMemberDTO> teamMemberDtoList)
    {
        _mockMapper
            .Setup(x => x.Map<IEnumerable<TeamMemberDTO>>(It.IsAny<IEnumerable<TeamMember>>()))
            .Returns(teamMemberDtoList);
    }
}
