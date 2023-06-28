using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Team.Update;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Team
{
    public class UpdateTeamTest
    {
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;

        public UpdateTeamTest()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockLogger = new Mock<ILoggerService>();
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_TeamUpdated()
        {
            // Arrange
            var teamMember = GetTeamMember();
            var updateQuery = new UpdateTeamQuery(GetTeamMemberDTO());

            MapperSetup(teamMember, updateQuery.TeamMember);
            BasicRepositorySetup(updateQuery.TeamMember);
            GetsAsyncRepositorySetup(teamMember);
            _mockRepository.Setup(repo => repo.TeamPositionRepository.Create(It.IsAny<TeamMemberPositions>()));

            var handler = new UpdateTeamHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);
            // Act
            var result = await handler.Handle(updateQuery, CancellationToken.None);
            // Assert
            Assert.Multiple(
                () => Assert.True(result.IsSuccess),
                () => Assert.Equal(updateQuery.TeamMember.Positions, result.Value.Positions)
            );
        }

        [Fact]
        public async Task ShouldThrowExeption_SaveChangesIsNotSuccessful()
        {
            // Arrange
            const string exceptionMessage = "Failed to update a team";

            var teamMember = GetTeamMember();
            var updateQuery = new UpdateTeamQuery(GetTeamMemberDTO());
            MapperSetup(teamMember, updateQuery.TeamMember);
            BasicRepositorySetup(updateQuery.TeamMember);
            GetsAsyncRepositorySetup(teamMember);
            _mockRepository.Setup(repo => repo.SaveChanges())
            .Throws(new Exception(exceptionMessage));

            var handler = new UpdateTeamHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);
            // Act
            var result = await handler.Handle(updateQuery, CancellationToken.None);
            // Assert
            Assert.Equal(exceptionMessage, result.Errors.First().Message);
        }

        [Theory]
        [InlineData(3)]
        public async Task ShouldDeleteUnusedTeamLinks(int unusedLinkId)
        {
            // Arrange
            var teamMember = GetTeamMember();
            var updateQuery = new UpdateTeamQuery(GetTeamMemberDTO());
            var existingLinks = GetTeamMemberLinksList().ToList();
            existingLinks.Add(new TeamMemberLink { Id = unusedLinkId });

            MapperSetup(teamMember, updateQuery.TeamMember);
            BasicRepositorySetup(updateQuery.TeamMember);
            GetsAsyncRepositorySetup(teamMember, link: existingLinks);
            _mockRepository.Setup(repo => repo.TeamLinkRepository.Delete(It.IsAny<TeamMemberLink>()));
            _mockRepository.Setup(repo => repo.TeamPositionRepository.Create(It.IsAny<TeamMemberPositions>()));

            var handler = new UpdateTeamHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);
            // Act
            var result = await handler.Handle(updateQuery, CancellationToken.None);
            // Assert
            _mockRepository.Verify(repo => repo.TeamLinkRepository.Delete(It.IsAny<TeamMemberLink>()), Times.Once);
            Assert.True(result.IsSuccess);
        }

        [Theory]
        [InlineData(3)]
        public async Task ShouldDeleteUnusedTeamPositions(int unusedPositionId)
        {
            // Arrange
            var teamMember = GetTeamMember();
            var updateQuery = new UpdateTeamQuery(GetTeamMemberDTO());
            var existingPositions = GetTeamMemberPositionsList().ToList();
            existingPositions.Add(GetTeamMemberPositions(teamMember.Id, unusedPositionId));

            MapperSetup(teamMember, updateQuery.TeamMember);
            BasicRepositorySetup(updateQuery.TeamMember);
            GetsAsyncRepositorySetup(teamMember, memberPos: existingPositions);
            _mockRepository.Setup(repo => repo.TeamPositionRepository.Delete(It.IsAny<TeamMemberPositions>()));

            var handler = new UpdateTeamHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);
            // Act
            var result = await handler.Handle(updateQuery, CancellationToken.None);
            // Assert
            _mockRepository.Verify(repo => repo.TeamPositionRepository.Delete(It.IsAny<TeamMemberPositions>()), Times.Once);
            Assert.True(result.IsSuccess);
        }

        [Theory]
        [InlineData(-1, "New Position")]
        public async Task WhenNewPositionIdIsNegative_UpdatesPositionAndTeamMemberPosition(int id, string posName)
        {
            // Arrange
            var teamMember = GetTeamMember();
            var existingPositions = GetTeamMemberPositionsList().ToList();
            existingPositions.Add(GetTeamMemberPositions(teamMember.Id, id));

            var teamMemberDto = new TeamMemberDTO
            {
                Positions = new List<PositionDTO>
                {
                    new PositionDTO { Id = id, Position = posName }
                }
            };
            var updateQuery = new UpdateTeamQuery(teamMemberDto);

            MapperSetup(teamMember, updateQuery.TeamMember);
            BasicRepositorySetup(updateQuery.TeamMember);
            GetsAsyncRepositorySetup(teamMember, memberPos: existingPositions);
            _mockRepository.Setup(repo => repo.TeamPositionRepository.Delete(It.IsAny<TeamMemberPositions>()));
            _mockRepository.Setup(repo => repo.TeamPositionRepository.Create(It.IsAny<TeamMemberPositions>()));
            _mockRepository.Setup(repo => repo.PositionRepository.Create(It.IsAny<Positions>())).Returns(new Positions());

            var handler = new UpdateTeamHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);
            // Act
            var result = await handler.Handle(updateQuery, CancellationToken.None);
            // Assert
            Assert.Multiple(
                () => Assert.True(result.IsSuccess),
                () => Assert.Equal(0, result.Value.Id)
            );

            _mockRepository.Verify(repo => repo.PositionRepository.Create(It.IsAny<Positions>()), Times.Once);
            _mockRepository.Verify(repo => repo.TeamPositionRepository.Create(It.IsAny<TeamMemberPositions>()), Times.Once);
        }
        private void BasicRepositorySetup(TeamMemberDTO updatedTeamMember)
        {
            _mockRepository.Setup(repo => repo.TeamRepository.Update(It.IsAny<TeamMember>()))
            .Callback<TeamMember>(tm => {
                tm.Positions = updatedTeamMember.Positions.Select(p => new Positions { Id = p.Id, Position = p.Position }).ToList();
            });
            _mockRepository.Setup(repo => repo.SaveChanges());
        }

        private void GetsAsyncRepositorySetup(TeamMember teamMember, List<TeamMemberLink>? link = null, List<TeamMemberPositions>? memberPos = null)
        {
            var linkList = link ?? teamMember.TeamMemberLinks;
            var posList = memberPos ?? new List<TeamMemberPositions>();

            _mockRepository.Setup(repo => repo.TeamLinkRepository
               .GetAllAsync(It.IsAny<Expression<Func<TeamMemberLink, bool>>>(), It.IsAny<Func<IQueryable<TeamMemberLink>,
               IIncludableQueryable<TeamMemberLink, object>>>())).ReturnsAsync(linkList);

            _mockRepository.Setup(repo => repo.TeamPositionRepository
               .GetAllAsync(It.IsAny<Expression<Func<TeamMemberPositions, bool>>>(), It.IsAny<Func<IQueryable<TeamMemberPositions>,
               IIncludableQueryable<TeamMemberPositions, object>>>())).ReturnsAsync(posList);
        }

        private void MapperSetup(TeamMember teamMember, TeamMemberDTO updatedTeamMember)
        {
            _mockMapper.Setup(mapper => mapper.Map<TeamMember>(updatedTeamMember))
                .Returns(teamMember);

            _mockMapper.Setup(mapper => mapper.Map<TeamMemberDTO>(teamMember))
                .Returns(updatedTeamMember);
        }

        private TeamMember GetTeamMember()
        {
            return new TeamMember
            {
                Id = 1,
                Positions = GetPositionsList(),
                TeamMemberLinks = GetTeamMemberLinksList()
            };
        }
        private TeamMemberDTO GetTeamMemberDTO()
        {
            return new TeamMemberDTO
            {
                Id = 1,
                Positions = GetPositionsDTOList(),
                TeamMemberLinks = GetTeamMemberLinksDTOList()
            };
        }

        private TeamMemberPositions GetTeamMemberPositions(int TeamMemberId, int posId)
        {
            return new TeamMemberPositions
            {
                TeamMemberId = TeamMemberId,
                PositionsId = posId
            };
        }
        private List<PositionDTO> GetPositionsDTOList()
        {
            return new List<PositionDTO>
            {
                new PositionDTO { Id = 1, Position = "Position 1" },
                new PositionDTO { Id = 2, Position = "Position 2" }
            };
        }

        private List<Positions> GetPositionsList()
        {
            return new List<Positions>
            {
                new Positions { Id = 1, Position = "Position 1" },
                new Positions { Id = 2, Position = "Position 2" }
            };
        }

        private List<TeamMemberPositions> GetTeamMemberPositionsList()
        {
            return new List<TeamMemberPositions>
            {
                new TeamMemberPositions { TeamMemberId = 1, PositionsId = 1 },
                new TeamMemberPositions { TeamMemberId = 1, PositionsId = 2 }
            };
        }
        private List<TeamMemberLinkDTO> GetTeamMemberLinksDTOList()
        {
            return new List<TeamMemberLinkDTO>
            {
                new TeamMemberLinkDTO { Id = 1 },
                new TeamMemberLinkDTO { Id = 2 }
            };
        }
        private List<TeamMemberLink> GetTeamMemberLinksList()
        {
            return new List<TeamMemberLink>
            {
                new TeamMemberLink { Id = 1 },
                new TeamMemberLink { Id = 2 }
            };
        }
    }
}
