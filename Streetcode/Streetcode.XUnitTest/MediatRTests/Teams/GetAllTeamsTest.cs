using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.MediatR.Team.GetAll;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;
using Streetcode.BLL.Interfaces.Logging;

namespace Streetcode.XUnitTest.MediatRTests.Teams
{
    public class GetAllTeamTest
    {
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;

        public GetAllTeamTest()
        {
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerService>();
        }

        private void SetupGetAllTeams(List<TeamMember> teamList)
        {
            if (teamList == null)
            {
                _mockRepository.Setup(x => x.TeamRepository.GetAllAsync(
                        null,
                        It.IsAny<Func<IQueryable<TeamMember>, IIncludableQueryable<TeamMember, object>>>()))
                    .ReturnsAsync((List<TeamMember>)null);
            }
            else
            {
                _mockRepository.Setup(x => x.TeamRepository.GetAllAsync(
                        null,
                        It.IsAny<Func<IQueryable<TeamMember>, IIncludableQueryable<TeamMember, object>>>()))
                    .ReturnsAsync(teamList);
            }
        }

        private void SetupMapTeamMembers(IEnumerable<TeamMember> teamList, IEnumerable<TeamMemberDTO> teamDTOList)
        {
            _mockMapper.Setup(x => x.Map<IEnumerable<TeamMemberDTO>>(It.IsAny<IEnumerable<TeamMember>>()))
                .Returns(teamDTOList);
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_CorrectType()
        {
            // Arrange
            var teamList = GetTeamList();
            var teamDTOList = GetListTeamDTO();

            SetupGetAllTeams(teamList);
            SetupMapTeamMembers(teamList, teamDTOList);

            var handler = new GetAllTeamHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetAllTeamQuery(), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.NotNull(result.Value),
                () => Assert.IsType<List<TeamMemberDTO>>(result.ValueOrDefault)
            );
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_CountMatch()
        {
            // Arrange
            var teamList = GetTeamList();
            var teamDTOList = GetListTeamDTO();

            SetupGetAllTeams(teamList);
            SetupMapTeamMembers(teamList, teamDTOList);

            var handler = new GetAllTeamHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetAllTeamQuery(), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.NotNull(result.Value),
                () => Assert.Equal(GetTeamList().Count(), result.Value.Count())
            );
        }

        [Fact]
        public async Task ShouldThrowException_IdNotExist()
        {
            // Arrange
            var expectedError = "Cannot find any team";

            SetupGetAllTeams(GetTeamListWithNotExistingId());

            var handler = new GetAllTeamHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetAllTeamQuery(), CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors.First().Message);

            _mockMapper.Verify(x => x.Map<IEnumerable<TeamMemberDTO>>(It.IsAny<IEnumerable<TeamMember>>()), Times.Never);
        }

        private static List<TeamMember> GetTeamList()
        {
            return new List<TeamMember>
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
        }

        private static List<TeamMember> GetTeamListWithNotExistingId()
        {
            return null;
        }

        private static List<TeamMemberDTO> GetListTeamDTO()
        {
            return new List<TeamMemberDTO>
            {
                new TeamMemberDTO
                {
                    Id = 1
                },
                new TeamMemberDTO
                {
                    Id = 2
                }
            };
        }
    }
}
