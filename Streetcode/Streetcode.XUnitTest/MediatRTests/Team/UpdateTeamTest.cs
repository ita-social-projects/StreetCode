using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Team.Update;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Repositories.Interfaces.Base;
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
            var teamMember = GetTeamMember(1);
            var updateQuery = new UpdateTeamQuery(GetTeamMemberDto());

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
                () => Assert.Equal(updateQuery.TeamMember.Positions, result.Value.Positions));
        }

        [Fact]
        public async Task ShouldThrowExeption_SaveChangesIsNotSuccessful()
        {
            // Arrange
            const string exceptionMessage = "Failed to update a team";

            var teamMember = GetTeamMember(1);
            var updateQuery = new UpdateTeamQuery(GetTeamMemberDto());
            MapperSetup(teamMember, updateQuery.TeamMember);
            BasicRepositorySetup(updateQuery.TeamMember);
            GetsAsyncRepositorySetup(teamMember);
            _mockRepository.Setup(repo => repo.SaveChangesAsync())
            .Throws(new Exception(exceptionMessage));

            var handler = new UpdateTeamHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);

            // Act
            var result = await handler.Handle(updateQuery, CancellationToken.None);

            // Assert
            Assert.Equal(exceptionMessage, result.Errors[0].Message);
        }

        [Theory]
        [InlineData(3)]
        public async Task ShouldDeleteUnusedTeamLinks(int unusedLinkId)
        {
            // Arrange
            var teamMember = GetTeamMember(1);
            var updateQuery = new UpdateTeamQuery(GetTeamMemberDto());
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
            var teamMember = GetTeamMember(1);
            var updateQuery = new UpdateTeamQuery(GetTeamMemberDto());
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
            var teamMember = GetTeamMember(1);
            var existingPositions = GetTeamMemberPositionsList().ToList();
            existingPositions.Add(GetTeamMemberPositions(teamMember.Id, id));

            var teamMemberDto = new UpdateTeamMemberDTO
            {
                Positions = new List<PositionDTO>
                {
                    new () { Id = id, Position = posName },
                },
            };
            var updateQuery = new UpdateTeamQuery(teamMemberDto);

            MapperSetup(teamMember, updateQuery.TeamMember);
            BasicRepositorySetup(updateQuery.TeamMember);
            GetsAsyncRepositorySetup(teamMember, memberPos: existingPositions);
            _mockRepository.Setup(repo => repo.TeamPositionRepository.Delete(It.IsAny<TeamMemberPositions>()));
            _mockRepository.Setup(repo => repo.TeamPositionRepository.CreateAsync(It.IsAny<TeamMemberPositions>()));
            _mockRepository.Setup(repo => repo.PositionRepository.CreateAsync(It.IsAny<Positions>())).ReturnsAsync(new Positions());

            var handler = new UpdateTeamHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);

            // Act
            var result = await handler.Handle(updateQuery, CancellationToken.None);

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
                Name = "Test",
                Description = "Test",
                IsMain = true,
                Positions = GetPositionsList(),
                TeamMemberLinks = GetTeamMemberLinksList(),
                ImageId = imageId,
            };
        }

        private static UpdateTeamMemberDTO GetTeamMemberDto()
        {
            return new UpdateTeamMemberDTO
            {
                Id = 1,
                Name = "Test",
                Description = "Test",
                IsMain = true,
                Positions = GetPositionsDtoList(),
                TeamMemberLinks = GetTeamMemberLinksDtoList(),
            };
        }

        private static TeamMemberPositions GetTeamMemberPositions(int teamMemberId, int posId)
        {
            return new TeamMemberPositions
            {
                TeamMemberId = teamMemberId,
                PositionsId = posId,
            };
        }

        private static List<PositionDTO> GetPositionsDtoList()
        {
            return new List<PositionDTO>
            {
                new () { Id = 1, Position = "Position 1" },
                new () { Id = 2, Position = "Position 2" },
            };
        }

        private static List<Positions> GetPositionsList()
        {
            return new List<Positions>
            {
                new () { Id = 1, Position = "Position 1" },
                new () { Id = 2, Position = "Position 2" },
            };
        }

        private static List<TeamMemberPositions> GetTeamMemberPositionsList()
        {
            return new List<TeamMemberPositions>
            {
                new () { TeamMemberId = 1, PositionsId = 1 },
                new () { TeamMemberId = 1, PositionsId = 2 },
            };
        }

        private static List<TeamMemberLinkDTO> GetTeamMemberLinksDtoList()
        {
            return new List<TeamMemberLinkDTO>
            {
                new () { Id = 1 },
                new () { Id = 2 },
            };
        }

        private static List<TeamMemberLink> GetTeamMemberLinksList()
        {
            return new List<TeamMemberLink>
            {
                new () { Id = 1 },
                new () { Id = 2 },
            };
        }

        private void BasicRepositorySetup(UpdateTeamMemberDTO updatedTeamMember)
        {
            _mockRepository.Setup(repo => repo.TeamRepository.Update(It.IsAny<TeamMember>()))
            .Callback<TeamMember>(tm =>
            {
                tm.Positions = updatedTeamMember.Positions?.Select(p => new Positions { Id = p.Id, Position = p.Position }).ToList();
            });
            _mockRepository.Setup(repo => repo.SaveChangesAsync());
        }

        private void GetsAsyncRepositorySetup(TeamMember teamMember, List<TeamMemberLink>? link = null, List<TeamMemberPositions>? memberPos = null)
        {
            var linkList = link ?? teamMember.TeamMemberLinks!;
            var posList = memberPos ?? new List<TeamMemberPositions>();

            _mockRepository.Setup(repo => repo.TeamLinkRepository
               .GetAllAsync(It.IsAny<Expression<Func<TeamMemberLink, bool>>>(), It.IsAny<Func<IQueryable<TeamMemberLink>,
               IIncludableQueryable<TeamMemberLink, object>>>())).ReturnsAsync(linkList);

            _mockRepository.Setup(repo => repo.TeamPositionRepository
               .GetAllAsync(It.IsAny<Expression<Func<TeamMemberPositions, bool>>>(), It.IsAny<Func<IQueryable<TeamMemberPositions>,
               IIncludableQueryable<TeamMemberPositions, object>>>())).ReturnsAsync(posList);
        }

        private void MapperSetup(TeamMember teamMember, UpdateTeamMemberDTO updatedTeamMember)
        {
            _mockMapper.Setup(mapper => mapper.Map<TeamMember>(updatedTeamMember))
                .Returns(teamMember);

            _mockMapper.Setup(mapper => mapper.Map<UpdateTeamMemberDTO>(teamMember))
                .Returns(updatedTeamMember);
        }
    }
}
