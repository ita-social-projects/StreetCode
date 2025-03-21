using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Team.GetAll;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Teams
{
    public class GetAllMainTeamHandlerTest
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IRepositoryWrapper> _mockRepo;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockStringLocalizerCannotFind;

        public GetAllMainTeamHandlerTest()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepo = new Mock<IRepositoryWrapper>();
            _mockLogger = new Mock<ILoggerService>();
            _mockStringLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Fact]
        public async Task Handle_ReturnsSuccess()
        {
            // Arrange
            SetupMocks(GetTeamList(), GetListTeamDto());
            var handler = new GetAllMainTeamHandler(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object, _mockStringLocalizerCannotFind.Object);

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
            SetupMocks(null, new List<TeamMemberDTO>());
            var handler = new GetAllMainTeamHandler(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object, _mockStringLocalizerCannotFind.Object);
            var expectedError = "Cannot find any team";
            _mockStringLocalizerCannotFind.Setup(x => x["CannotFindAnyTeam"])
                .Returns(new LocalizedString("CannotFindAnyTeam", expectedError));

            // Act
            var result = await handler.Handle(new GetAllMainTeamQuery(), CancellationToken.None);

            // Assert
            Assert.Contains(expectedError, result.Errors[0].Message);
        }

        private static IEnumerable<TeamMember> GetTeamList()
        {
            var teamMembers = new List<TeamMember>
        {
            new TeamMember
            {
                Id = 1,
                IsMain = true,
                Positions = { },
                TeamMemberLinks = { },
            },
            new TeamMember
            {
                Id = 2,
                IsMain = true,
                Positions = { },
                TeamMemberLinks = { },
            },
        };

            return teamMembers;
        }

        private static List<TeamMemberDTO> GetListTeamDto()
        {
            var teamMembersDto = new List<TeamMemberDTO>
            {
                new TeamMemberDTO
                {
                    Id = 1,
                    IsMain = true,
                    Positions = { },
                    TeamMemberLinks = { },
                },
                new TeamMemberDTO
                {
                    Id = 2,
                    IsMain = true,
                    Positions = { },
                    TeamMemberLinks = { },
                },
            };
            return teamMembersDto;
        }

        private void SetupMocks(IEnumerable<TeamMember> teamMembers, IEnumerable<TeamMemberDTO> teamMemberDtOs)
        {
            _mockRepo
                .Setup(r => r.TeamRepository
                    .GetAllAsync(
                        null,
                        It.IsAny<Func<IQueryable<TeamMember>,
                        IIncludableQueryable<TeamMember, object>>?>()))
                .ReturnsAsync(teamMembers);

            _mockMapper.Setup(r => r.Map<IEnumerable<TeamMemberDTO>>(
                It.IsAny<IEnumerable<TeamMember>>()))
                .Returns(teamMemberDtOs);
        }
    }
}