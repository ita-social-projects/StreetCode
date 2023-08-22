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
using Streetcode.BLL.Interfaces.Logging;

namespace Streetcode.XUnitTest.MediatRTests.Teams
{
    public class GetTeamByIdTest
    {
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;

        public GetTeamByIdTest()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockLogger = new Mock<ILoggerService>();
        }

        private void SetupGetTeamById(TeamMember testTeam)
        {
            _mockRepository.Setup(x => x.TeamRepository
                .GetSingleOrDefaultAsync(
                    It.IsAny<Expression<Func<TeamMember, bool>>>(),
                    It.IsAny<Func<IQueryable<TeamMember>, IIncludableQueryable<TeamMember, object>>>()))
                .ReturnsAsync(testTeam);
        }

        private void SetupMapTeamMember(TeamMemberDTO teamMemberDTO)
        {
            _mockMapper.Setup(x => x
                .Map<TeamMemberDTO>(It.IsAny<TeamMember>()))
                .Returns(teamMemberDTO);
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_ExistingId()
        {
            // Arrange
            var testTeam = GetTeam();
            var teamDTO = GetTeamDTO();

            SetupGetTeamById(testTeam);
            SetupMapTeamMember(teamDTO);

            var handler = new GetByIdTeamHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetByIdTeamQuery(testTeam.Id), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.True(result.IsSuccess),
                () => Assert.Equal(result.Value.Id, testTeam.Id)
            );
        }

        [Fact]
        public async Task ShouldReturnErrorResponse_NotExistingId()
        {
            // Arrange
            var testTeam = GetTeam();
            var expectedError = $"Cannot find any team with corresponding id: {testTeam.Id}";
            var teamDTOWithNotExistingId = GetTeamDTOWithNotExistingId();

            SetupGetTeamById(GetTeamWithNotExistingId());
            SetupMapTeamMember(teamDTOWithNotExistingId);

            var handler = new GetByIdTeamHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetByIdTeamQuery(testTeam.Id), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.True(result.IsFailed),
                () => Assert.Equal(expectedError, result.Errors.First().Message)
            );
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_CorrectType()
        {
            // Arrange
            var testTeam = GetTeam();
            var teamDTO = GetTeamDTO();

            SetupGetTeamById(testTeam);
            SetupMapTeamMember(teamDTO);

            var handler = new GetByIdTeamHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetByIdTeamQuery(testTeam.Id), CancellationToken.None);

            // Assert
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

        private static TeamMember GetTeamWithNotExistingId()
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

        private static TeamMemberDTO GetTeamDTOWithNotExistingId()
        {
            return null;
        }
    }
}
