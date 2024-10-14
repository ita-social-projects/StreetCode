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

namespace Streetcode.XUnitTest.MediatRTests.Team
{
    public class GetAllTeamsTest
    {
        private readonly Mock<IRepositoryWrapper> mockRepository;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> mockLocalizerCannotFind;

        public GetAllTeamsTest()
        {
            this.mockRepository = new Mock<IRepositoryWrapper>();
            this.mockMapper = new Mock<IMapper>();
            this.mockLogger = new Mock<ILoggerService>();
            this.mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_CorrectType()
        {
            // Arrange
            var teamList = GetTeamList();
            var teamDTOList = GetListTeamDTO();

            this.SetupGetAllTeams(teamList);
            this.SetupMapTeamMembers(teamDTOList);

            var handler = new GetAllTeamHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new GetAllTeamQuery(), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.NotNull(result.Value),
                () => Assert.IsType<List<TeamMemberDTO>>(result.ValueOrDefault));
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_CountMatch()
        {
            // Arrange
            var teamList = GetTeamList();
            var teamDTOList = GetListTeamDTO();

            this.SetupGetAllTeams(teamList);
            this.SetupMapTeamMembers(teamDTOList);

            var handler = new GetAllTeamHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new GetAllTeamQuery(), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.NotNull(result.Value),
                () => Assert.Equal(GetTeamList().Count, result.Value.Count()));
        }

        [Fact]
        public async Task ShouldThrowException_IdNotExist()
        {
            // Arrange
            var expectedError = "Cannot find any team";
            this.mockLocalizerCannotFind.Setup(x => x["CannotFindAnyTeam"])
                .Returns(new LocalizedString("CannotFindAnyTeam", expectedError));

            this.SetupGetAllTeams(GetTeamListWithNotExistingId());

            var handler = new GetAllTeamHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new GetAllTeamQuery(), CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors[0].Message);

            this.mockMapper.Verify(x => x.Map<IEnumerable<TeamMemberDTO>>(It.IsAny<IEnumerable<TeamMember>>()), Times.Never);
        }

        private static List<TeamMember> GetTeamList()
        {
            return new List<TeamMember>
            {
                new TeamMember
                {
                    Id = 1,
                },
                new TeamMember
                {
                    Id = 2,
                },
            };
        }

        private static List<TeamMember> GetTeamListWithNotExistingId()
        {
            return new List<TeamMember>();
        }

        private static List<TeamMemberDTO> GetListTeamDTO()
        {
            return new List<TeamMemberDTO>
            {
                new TeamMemberDTO
                {
                    Id = 1,
                },
                new TeamMemberDTO
                {
                    Id = 2,
                },
            };
        }

        private void SetupGetAllTeams(List<TeamMember> teamList)
        {
            if (teamList == null)
            {
                this.mockRepository.Setup(x => x.TeamRepository.GetAllAsync(
                        null,
                        It.IsAny<Func<IQueryable<TeamMember>, IIncludableQueryable<TeamMember, object>>>()))
                    .ReturnsAsync(new List<TeamMember>());
            }
            else
            {
                this.mockRepository.Setup(x => x.TeamRepository.GetAllAsync(
                        null,
                        It.IsAny<Func<IQueryable<TeamMember>, IIncludableQueryable<TeamMember, object>>>()))
                    .ReturnsAsync(teamList);
            }
        }

        private void SetupMapTeamMembers(IEnumerable<TeamMemberDTO> teamDTOList)
        {
            this.mockMapper.Setup(x => x.Map<IEnumerable<TeamMemberDTO>>(It.IsAny<IEnumerable<TeamMember>>()))
                .Returns(teamDTOList);
        }
    }
}
