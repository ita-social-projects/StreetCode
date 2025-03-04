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
        private readonly Mock<IRepositoryWrapper> mockRepository;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<ILoggerService> mockLogger;

        public UpdateTeamTest()
        {
            this.mockMapper = new Mock<IMapper>();
            this.mockRepository = new Mock<IRepositoryWrapper>();
            this.mockLogger = new Mock<ILoggerService>();
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_TeamUpdated()
        {
            // Arrange
            var teamMember = GetTeamMember(1);
            var updateQuery = new UpdateTeamQuery(GetTeamMemberDTO());

            this.MapperSetup(teamMember, updateQuery.TeamMember);
            this.BasicRepositorySetup(updateQuery.TeamMember);
            this.GetsAsyncRepositorySetup(teamMember);
            this.mockRepository.Setup(repo => repo.TeamPositionRepository.Create(It.IsAny<TeamMemberPositions>()));

            var handler = new UpdateTeamHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object);

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
            var updateQuery = new UpdateTeamQuery(GetTeamMemberDTO());
            this.MapperSetup(teamMember, updateQuery.TeamMember);
            this.BasicRepositorySetup(updateQuery.TeamMember);
            this.GetsAsyncRepositorySetup(teamMember);
            this.mockRepository.Setup(repo => repo.SaveChangesAsync())
            .Throws(new Exception(exceptionMessage));

            var handler = new UpdateTeamHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object);

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
            var updateQuery = new UpdateTeamQuery(GetTeamMemberDTO());
            var existingLinks = GetTeamMemberLinksList().ToList();
            existingLinks.Add(new TeamMemberLink { Id = unusedLinkId });

            this.MapperSetup(teamMember, updateQuery.TeamMember);
            this.BasicRepositorySetup(updateQuery.TeamMember);
            this.GetsAsyncRepositorySetup(teamMember, link: existingLinks);
            this.mockRepository.Setup(repo => repo.TeamLinkRepository.Delete(It.IsAny<TeamMemberLink>()));
            this.mockRepository.Setup(repo => repo.TeamPositionRepository.Create(It.IsAny<TeamMemberPositions>()));

            var handler = new UpdateTeamHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object);

            // Act
            var result = await handler.Handle(updateQuery, CancellationToken.None);

            // Assert
            this.mockRepository.Verify(repo => repo.TeamLinkRepository.Delete(It.IsAny<TeamMemberLink>()), Times.Once);
            Assert.True(result.IsSuccess);
        }

        [Theory]
        [InlineData(3)]
        public async Task ShouldDeleteUnusedTeamPositions(int unusedPositionId)
        {
            // Arrange
            var teamMember = GetTeamMember(1);
            var updateQuery = new UpdateTeamQuery(GetTeamMemberDTO());
            var existingPositions = GetTeamMemberPositionsList().ToList();
            existingPositions.Add(GetTeamMemberPositions(teamMember.Id, unusedPositionId));

            this.MapperSetup(teamMember, updateQuery.TeamMember);
            this.BasicRepositorySetup(updateQuery.TeamMember);
            this.GetsAsyncRepositorySetup(teamMember, memberPos: existingPositions);
            this.mockRepository.Setup(repo => repo.TeamPositionRepository.Delete(It.IsAny<TeamMemberPositions>()));

            var handler = new UpdateTeamHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object);

            // Act
            var result = await handler.Handle(updateQuery, CancellationToken.None);

            // Assert
            this.mockRepository.Verify(repo => repo.TeamPositionRepository.Delete(It.IsAny<TeamMemberPositions>()), Times.Once);
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

            this.MapperSetup(teamMember, updateQuery.TeamMember);
            this.BasicRepositorySetup(updateQuery.TeamMember);
            this.GetsAsyncRepositorySetup(teamMember, memberPos: existingPositions);
            this.mockRepository.Setup(repo => repo.TeamPositionRepository.Delete(It.IsAny<TeamMemberPositions>()));
            this.mockRepository.Setup(repo => repo.TeamPositionRepository.CreateAsync(It.IsAny<TeamMemberPositions>()));
            this.mockRepository.Setup(repo => repo.PositionRepository.CreateAsync(It.IsAny<Positions>())).ReturnsAsync(new Positions());

            var handler = new UpdateTeamHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object);

            // Act
            var result = await handler.Handle(updateQuery, CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.True(result.IsSuccess),
                () => Assert.Equal(0, result.Value.Id));

            this.mockRepository.Verify(repo => repo.PositionRepository.CreateAsync(It.IsAny<Positions>()), Times.Once);
            this.mockRepository.Verify(repo => repo.TeamPositionRepository.CreateAsync(It.IsAny<TeamMemberPositions>()), Times.Once);
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

        private static UpdateTeamMemberDTO GetTeamMemberDTO()
        {
            return new UpdateTeamMemberDTO
            {
                Id = 1,
                Name = "Test",
                Description = "Test",
                IsMain = true,
                Positions = GetPositionsDTOList(),
                TeamMemberLinks = GetTeamMemberLinksDTOList(),
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

        private static List<PositionDTO> GetPositionsDTOList()
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

        private static List<TeamMemberLinkDTO> GetTeamMemberLinksDTOList()
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
            this.mockRepository.Setup(repo => repo.TeamRepository.Update(It.IsAny<TeamMember>()))
            .Callback<TeamMember>(tm =>
            {
                tm.Positions = updatedTeamMember.Positions?.Select(p => new Positions { Id = p.Id, Position = p.Position }).ToList();
            });
            this.mockRepository.Setup(repo => repo.SaveChangesAsync());
        }

        private void GetsAsyncRepositorySetup(TeamMember teamMember, List<TeamMemberLink>? link = null, List<TeamMemberPositions>? memberPos = null)
        {
            var linkList = link ?? teamMember.TeamMemberLinks!;
            var posList = memberPos ?? new List<TeamMemberPositions>();

            this.mockRepository.Setup(repo => repo.TeamLinkRepository
               .GetAllAsync(It.IsAny<Expression<Func<TeamMemberLink, bool>>>(), It.IsAny<Func<IQueryable<TeamMemberLink>,
               IIncludableQueryable<TeamMemberLink, object>>>())).ReturnsAsync(linkList);

            this.mockRepository.Setup(repo => repo.TeamPositionRepository
               .GetAllAsync(It.IsAny<Expression<Func<TeamMemberPositions, bool>>>(), It.IsAny<Func<IQueryable<TeamMemberPositions>,
               IIncludableQueryable<TeamMemberPositions, object>>>())).ReturnsAsync(posList);
        }

        private void MapperSetup(TeamMember teamMember, UpdateTeamMemberDTO updatedTeamMember)
        {
            this.mockMapper.Setup(mapper => mapper.Map<TeamMember>(updatedTeamMember))
                .Returns(teamMember);

            this.mockMapper.Setup(mapper => mapper.Map<UpdateTeamMemberDTO>(teamMember))
                .Returns(updatedTeamMember);
        }
    }
}
