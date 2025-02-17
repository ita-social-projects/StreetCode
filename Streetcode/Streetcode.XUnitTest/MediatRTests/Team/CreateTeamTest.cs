using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Team.Create;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Team
{
    public class CreateTeamTest
    {
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly MockFailedToCreateLocalizer _mockLocalizerFailed;

        public CreateTeamTest()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockLogger = new Mock<ILoggerService>();
            _mockLocalizerFailed = new MockFailedToCreateLocalizer();
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_TeamCreated()
        {
            // Arrange
            var teamMember = GetTeamMember(1);
            this.MapperSetup(teamMember);
            this.BasicRepositorySetup(teamMember);
            this.GetsAsyncRepositorySetup();

            var handler = new CreateTeamHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object, _mockLocalizerFailed);

            // Act
            var result = await handler.Handle(new CreateTeamQuery(new TeamMemberCreateDTO()), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task ShouldThrowException_SaveChangesIsNotSuccessful()
        {
            // Arrange
            string exceptionMessage = _mockLocalizerFailed["FailedToCreateTeam"];

            var teamMember = GetTeamMember(1);
            this.MapperSetup(teamMember);
            this.GetsAsyncRepositorySetup();
            _mockRepository.Setup(repo => repo.TeamRepository.CreateAsync(teamMember)).ReturnsAsync(teamMember);
            _mockRepository.Setup(repo => repo.SaveChangesAsync()).Throws(new Exception(exceptionMessage));

            var handler = new CreateTeamHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object, _mockLocalizerFailed);

            // Act
            var result = await handler.Handle(new CreateTeamQuery(new TeamMemberCreateDTO()), CancellationToken.None);

            // Assert
            Assert.Equal(exceptionMessage, result.Errors.Single().Message);
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_CorrectType()
        {
            // Arrange
            var teamMember = GetTeamMember(1);
            this.MapperSetup(teamMember);
            this.BasicRepositorySetup(teamMember);
            this.GetsAsyncRepositorySetup();

            var handler = new CreateTeamHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object, _mockLocalizerFailed);

            // Act
            var result = await handler.Handle(new CreateTeamQuery(new TeamMemberCreateDTO()), CancellationToken.None);

            // Assert
            Assert.IsType<TeamMemberDTO>(result.Value);
        }

        [Theory]
        [InlineData(-1, "New Position")]
        public async Task WhenNewPositionIdIsNegative_CreatesNewPositionAndTeamMemberPosition(int id, string positionName)
        {
            // Arrange
            var newPosition = new PositionDTO { Id = id, Position = positionName };
            var newPositions = new List<PositionDTO> { newPosition };
            var teamMember = GetTeamMember(1);
            var teamMemberDTO = GetTeamMemberDTO(newPositions);

            this.MapperSetup(teamMember);
            this.BasicRepositorySetup(teamMember);
            this.GetsAsyncRepositorySetup();
            _mockRepository.Setup(repo => repo.TeamPositionRepository.CreateAsync(It.IsAny<TeamMemberPositions>()));
            _mockRepository.Setup(repo => repo.PositionRepository.CreateAsync(It.IsAny<Positions>()))
                .ReturnsAsync(new Positions());

            var handler = new CreateTeamHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object, _mockLocalizerFailed);

            // Act
            var result = await handler.Handle(new CreateTeamQuery(teamMemberDTO), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.True(result.IsSuccess),
                () => Assert.Equal(0, result.Value.Id));

            _mockRepository.Verify(repo => repo.PositionRepository.CreateAsync(It.IsAny<Positions>()), Times.Once);
            _mockRepository.Verify(repo => repo.TeamPositionRepository.CreateAsync(It.IsAny<TeamMemberPositions>()), Times.Once);
        }

        private static TeamMember GetTeamMember(int imageId = 0)
        {
            return new TeamMember
            {
                Id = 1,
                ImageId = imageId,
                Name = "Test",
                Description = "Test",
                IsMain = true,
                Positions = new List<Positions>(),
            };
        }

        private static TeamMemberCreateDTO GetTeamMemberDTO(List<PositionDTO> newPositions)
        {
            return new TeamMemberCreateDTO
            {
                ImageId = 1,
                Name = "Test",
                Description = "Test",
                IsMain = true,
                Positions = newPositions,
            };
        }

        private void BasicRepositorySetup(TeamMember teamMember)
        {
            _mockRepository.Setup(repo => repo.TeamRepository.CreateAsync(teamMember)).ReturnsAsync(teamMember);

            _mockRepository.Setup(repo => repo.SaveChangesAsync());
        }

        private void GetsAsyncRepositorySetup(List<TeamMemberLink>? link = null)
        {
            var linkList = link ?? new List<TeamMemberLink>();

            _mockRepository.Setup(repo => repo.TeamLinkRepository
               .GetAllAsync(It.IsAny<Expression<Func<TeamMemberLink, bool>>>(), It.IsAny<Func<IQueryable<TeamMemberLink>,
               IIncludableQueryable<TeamMemberLink, object>>>())).ReturnsAsync(linkList);

            _mockRepository.Setup(repo => repo.TeamPositionRepository
                .GetAllAsync(It.IsAny<Expression<Func<TeamMemberPositions, bool>>>(), It.IsAny<Func<IQueryable<TeamMemberPositions>,
                IIncludableQueryable<TeamMemberPositions, object>>>())).ReturnsAsync(new List<TeamMemberPositions>());
        }

        private void MapperSetup(TeamMember member)
        {
            _mockMapper.Setup(mapper => mapper.Map<TeamMember>(It.IsAny<object>()))
                .Returns(member);

            _mockMapper.Setup(mapper => mapper.Map<TeamMemberDTO>(It.IsAny<object>()))
                .Returns(new TeamMemberDTO());
        }
    }
}
