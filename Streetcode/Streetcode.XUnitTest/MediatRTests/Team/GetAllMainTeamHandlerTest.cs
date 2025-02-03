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
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IRepositoryWrapper> mockRepo;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> mockStringLocalizerCannotFind;

        public GetAllMainTeamHandlerTest()
        {
            this.mockMapper = new Mock<IMapper>();
            this.mockRepo = new Mock<IRepositoryWrapper>();
            this.mockLogger = new Mock<ILoggerService>();
            this.mockStringLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Fact]
        public async Task Handle_ReturnsSuccess()
        {
            // Arrange
            this.SetupMocks(GetTeamList(), GetListTeamDTO());
            var handler = new GetAllMainTeamHandler(this.mockRepo.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockStringLocalizerCannotFind.Object);

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
            this.SetupMocks(null, new List<TeamMemberDto>());
            var handler = new GetAllMainTeamHandler(this.mockRepo.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockStringLocalizerCannotFind.Object);
            var expectedError = "Cannot find any team";
            this.mockStringLocalizerCannotFind.Setup(x => x["CannotFindAnyTeam"])
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

        private static List<TeamMemberDto> GetListTeamDTO()
        {
            var teamMembersDTO = new List<TeamMemberDto>
            {
                new TeamMemberDto
                {
                    Id = 1,
                    IsMain = true,
                    Positions = { },
                    TeamMemberLinks = { },
                },
                new TeamMemberDto
                {
                    Id = 2,
                    IsMain = true,
                    Positions = { },
                    TeamMemberLinks = { },
                },
            };
            return teamMembersDTO;
        }

        private void SetupMocks(IEnumerable<TeamMember> teamMembers, IEnumerable<TeamMemberDto> teamMemberDTOs)
        {
            this.mockRepo
                .Setup(r => r.TeamRepository
                    .GetAllAsync(
                        null,
                        It.IsAny<Func<IQueryable<TeamMember>,
                        IIncludableQueryable<TeamMember, object>>?>()))
                .ReturnsAsync(teamMembers);

            this.mockMapper.Setup(r => r.Map<IEnumerable<TeamMemberDto>>(
                It.IsAny<IEnumerable<TeamMember>>()))
                .Returns(teamMemberDTOs);
        }
    }
}