using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.MediatR.Team.GetAll;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Teams
{
    public class GetAllMainTeamHandlerTests
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IRepositoryWrapper> _mockRepo;

        public GetAllMainTeamHandlerTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepo = new Mock<IRepositoryWrapper>();
        }

        [Fact]
        public async Task Handle_ReturnsSuccess()
        {
            // Arrange

            _mockRepo.Setup(r => r.TeamRepository.GetAllAsync(null,
                It.IsAny<Func<IQueryable<TeamMember>, IIncludableQueryable<TeamMember, object>>?>()))
            .ReturnsAsync(GetTeamList);

            _mockMapper.Setup(r => r.Map<IEnumerable<TeamMemberDTO>>(
                It.IsAny<IEnumerable<TeamMember>>()))
           .Returns(GetListTeamDTO());

            var handler = new GetAllMainTeamHandler(_mockRepo.Object, _mockMapper.Object);

            // Act
            var result = await handler.Handle(new GetAllMainTeamQuery(), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Value.Count());
        }

        [Fact]
        public async Task Handle_ReturnsError()
        {
            // Arrange
            _mockRepo.Setup(r => r.TeamRepository.GetAllAsync(
                null,
                It.IsAny<Func<IQueryable<TeamMember>, IIncludableQueryable<TeamMember, object>>?>()
            )).ReturnsAsync((IEnumerable<TeamMember>)null);

            var handler = new GetAllMainTeamHandler(_mockRepo.Object, _mockMapper.Object);

            // Act
            var result = await handler.Handle(new GetAllMainTeamQuery(), CancellationToken.None);

            // Assert
            Assert.True(result.IsFailed);
            Assert.Contains("Cannot find any team", result.Errors.First().Message);
        }

        private static IEnumerable<TeamMember> GetTeamList()
        {
            var teamMembers = new List<TeamMember>
        {
            new TeamMember
            {
                Id=1,
                IsMain = true,
                Positions = new List<Positions>(),
                TeamMemberLinks = new List<TeamMemberLink>()
            },
            new TeamMember
            {
                Id=2,
                IsMain = true,
                Positions = new List<Positions>(),
                TeamMemberLinks = new List<TeamMemberLink>()
            }
        };

            return teamMembers;
        }


        private static List<TeamMemberDTO> GetListTeamDTO()
        {
            var teamMembersDTO = new List<TeamMemberDTO>
        {
             new TeamMemberDTO
            {
                Id=1,
                IsMain = true,
                Positions = new List<PositionDTO>(),
                TeamMemberLinks = new List<TeamMemberLinkDTO>()
            },
            new TeamMemberDTO
            {
                Id=2,
                IsMain = true,
                Positions = new List<PositionDTO>(),
                TeamMemberLinks = new List<TeamMemberLinkDTO>()
            },
        };

            return teamMembersDTO;
        }
    }


}

