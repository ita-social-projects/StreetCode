using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Team.GetAll;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode.TextContent;
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
            this._mockMapper = new Mock<IMapper>();
            this._mockRepo = new Mock<IRepositoryWrapper>();
            this._mockLogger = new Mock<ILoggerService>();
            this._mockStringLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Fact]
        public async Task Handle_ReturnsSuccess()
        {
            // Arrange
            this.SetupMocks(GetTeamList(), GetListTeamDTO());
            var handler = new GetAllMainTeamHandler(this._mockRepo.Object, this._mockMapper.Object, this._mockLogger.Object, this._mockStringLocalizerCannotFind.Object);

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
            this.SetupMocks(new List<TeamMember>(), new List<TeamMemberDTO>());
            var handler = new GetAllMainTeamHandler(this._mockRepo.Object, this._mockMapper.Object, this._mockLogger.Object, this._mockStringLocalizerCannotFind.Object);
            var expectedError = "Cannot find any team";
            this._mockStringLocalizerCannotFind.Setup(x => x["CannotFindAnyTeam"])
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

        private static List<TeamMemberDTO> GetListTeamDTO()
        {
            var teamMembersDTO = new List<TeamMemberDTO>
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
            return teamMembersDTO;
        }

        private void SetupMocks(IEnumerable<TeamMember> teamMembers, IEnumerable<TeamMemberDTO> teamMemberDTOs)
        {
            this._mockRepo
                .Setup(r => r.TeamRepository
                    .GetAllAsync(
                        null,
                        It.IsAny<Func<IQueryable<TeamMember>,
                        IIncludableQueryable<TeamMember, object>>?>()))
                .ReturnsAsync(teamMembers);

            this._mockMapper.Setup(r => r.Map<IEnumerable<TeamMemberDTO>>(
                It.IsAny<IEnumerable<TeamMember>>()))
                .Returns(teamMemberDTOs);
        }
    }
}