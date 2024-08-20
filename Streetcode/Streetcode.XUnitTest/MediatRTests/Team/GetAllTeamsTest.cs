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
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizerCannotFind;

        public GetAllTeamsTest()
        {
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerService>();
            _mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_CorrectType()
        {
            // Arrange
            var teamList = GetTeamList();
            var teamDTOList = GetListTeamDTO();

            SetupGetAllTeams(teamList);
            SetupMapTeamMembers(teamDTOList);

            var handler = new GetAllTeamHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);

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

            SetupGetAllTeams(teamList);
            SetupMapTeamMembers(teamDTOList);

            var handler = new GetAllTeamHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);

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
            _mockLocalizerCannotFind.Setup(x => x["CannotFindAnyTeam"])
                .Returns(new LocalizedString("CannotFindAnyTeam", expectedError));

            SetupGetAllTeams(GetTeamListWithNotExistingId());

            var handler = new GetAllTeamHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new GetAllTeamQuery(), CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors[0].Message);

            _mockMapper.Verify(x => x.Map<IEnumerable<TeamMemberDTO>>(It.IsAny<IEnumerable<TeamMember>>()), Times.Never);
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
                _mockRepository.Setup(x => x.TeamRepository.GetAllAsync(
                        null,
                        It.IsAny<Func<IQueryable<TeamMember>, IIncludableQueryable<TeamMember, object>>>()))
                    .ReturnsAsync(new List<TeamMember>());
            }
            else
            {
                _mockRepository.Setup(x => x.TeamRepository.GetAllAsync(
                        null,
                        It.IsAny<Func<IQueryable<TeamMember>, IIncludableQueryable<TeamMember, object>>>()))
                    .ReturnsAsync(teamList);
            }
        }

        private void SetupMapTeamMembers(IEnumerable<TeamMemberDTO> teamDTOList)
        {
            _mockMapper.Setup(x => x.Map<IEnumerable<TeamMemberDTO>>(It.IsAny<IEnumerable<TeamMember>>()))
                .Returns(teamDTOList);
        }
    }
}
