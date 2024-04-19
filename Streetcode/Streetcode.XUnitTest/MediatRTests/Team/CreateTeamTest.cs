using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Org.BouncyCastle.Crypto;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Team.Create;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Team
{
    public class CreateTeamTest
    {
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<CannotConvertNullSharedResource>> _mockLocalizerConvertNull;

        public CreateTeamTest()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockLogger = new Mock<ILoggerService>();
            _mockLocalizerConvertNull = new Mock<IStringLocalizer<CannotConvertNullSharedResource>>();
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_TeamCreated()
        {
            // Arrange
            var teamMember = GetTeamMember(1);
            MapperSetup(teamMember);
            BasicRepositorySetup(teamMember);
            GetsAsyncRepositorySetup();

            var handler = new CreateTeamHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object, _mockLocalizerConvertNull.Object);
            // Act
            var result = await handler.Handle(new CreateTeamQuery(new TeamMemberDTO()), CancellationToken.None);
            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task ShouldThrowExeption_SaveChangesIsNotSuccessful()
        {
            // Arrange
            const string exceptionMessage = "Failed to create a team member";

            var teamMember = GetTeamMember(1);
            MapperSetup(teamMember);
            GetsAsyncRepositorySetup();
            _mockRepository.Setup(repo => repo.TeamRepository.CreateAsync(teamMember)).ReturnsAsync(teamMember);
            _mockRepository.Setup(repo => repo.SaveChanges()).Throws(new Exception(exceptionMessage));

            var handler = new CreateTeamHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object, _mockLocalizerConvertNull.Object);
            // Act
            var result = await handler.Handle(new CreateTeamQuery(new TeamMemberDTO()), CancellationToken.None);
            // Assert
            Assert.Equal(exceptionMessage, result.Errors.First().Message);
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_CorrectType()
        {
            // Arrange
            var teamMember = GetTeamMember(1);
            MapperSetup(teamMember);
            BasicRepositorySetup(teamMember);
            GetsAsyncRepositorySetup();

            var handler = new CreateTeamHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object, _mockLocalizerConvertNull.Object);
            // Act
            var result = await handler.Handle(new CreateTeamQuery(new TeamMemberDTO()), CancellationToken.None);
            // Assert
            Assert.IsType<TeamMemberDTO>(result.Value);
        }

        [Theory]
        [InlineData(1, 2)]
        public async Task ShouldDeleteLinks_LinksToBeDeletedExist(int idFirst, int idSecond)
        {
            // Arrange
            var teamMember = GetTeamMember(1);
            var linkFirst = GetTeamMemberLink(idFirst);
            var linkSecond = GetTeamMemberLink(idSecond);
            var links = new List<TeamMemberLink> { linkFirst, linkSecond };

            MapperSetup(teamMember);
            BasicRepositorySetup(teamMember);
            GetsAsyncRepositorySetup(link: links);
            _mockRepository.Setup(repo => repo.TeamLinkRepository.Delete(It.IsAny<TeamMemberLink>()));

            var handler = new CreateTeamHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object, _mockLocalizerConvertNull.Object);
            // Act
            var result = await handler.Handle(new CreateTeamQuery(new TeamMemberDTO()), CancellationToken.None);
            // Assert
            Assert.Multiple(
                () => Assert.True(result.IsSuccess),
                () => Assert.NotNull(result.Value)
            );

            _mockRepository.Verify(repo => repo.TeamLinkRepository.Delete(It.IsAny<TeamMemberLink>()), Times.Exactly(links.Count));
        }

        [Theory]
        [InlineData(1, 2)]
        public async Task ShouldDeletePositions_PositionsToBeDeletedExist(int idFirst, int idSecond)
        {
            // Arrange
            var teamMember = GetTeamMember(1);
            var positionFirst = GetTeamMemberPositions(idFirst);
            var positionSecond = GetTeamMemberPositions(idSecond);
            var oldPositions = new List<TeamMemberPositions> { positionFirst, positionSecond };

            MapperSetup(teamMember);
            BasicRepositorySetup(teamMember);
            GetsAsyncRepositorySetup(memberPos: oldPositions);
            _mockRepository.Setup(repo => repo.TeamPositionRepository.Delete(It.IsAny<TeamMemberPositions>()));
            
            _mockRepository.Setup(repo => repo.TeamPositionRepository
                .GetAllAsync(It.IsAny<Expression<Func<TeamMemberPositions, bool>>>(), It.IsAny<Func<IQueryable<TeamMemberPositions>,
                IIncludableQueryable<TeamMemberPositions, object>>>())).ReturnsAsync(oldPositions);

            var handler = new CreateTeamHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object, _mockLocalizerConvertNull.Object);
            // Act
            var result = await handler.Handle(new CreateTeamQuery(new TeamMemberDTO()), CancellationToken.None);
            // Assert
            Assert.Multiple(
                () => Assert.True(result.IsSuccess),
                () => Assert.NotNull(result.Value)
            );

            _mockRepository.Verify(repo => repo.TeamPositionRepository.Delete(It.IsAny<TeamMemberPositions>()), Times.Exactly(oldPositions.Count));
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
            
            MapperSetup(teamMember);
            BasicRepositorySetup(teamMember);
            GetsAsyncRepositorySetup();
            _mockRepository.Setup(repo => repo.TeamPositionRepository.Create(It.IsAny<TeamMemberPositions>()));
            _mockRepository.Setup(repo => repo.PositionRepository.Create(It.IsAny<Positions>()))
                .Returns(new Positions());

            var handler = new CreateTeamHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object, _mockLocalizerConvertNull.Object);
            // Act
            var result = await handler.Handle(new CreateTeamQuery(teamMemberDTO), CancellationToken.None);
            // Assert
            Assert.Multiple(
                () => Assert.True(result.IsSuccess),
                () => Assert.Equal(0, result.Value.Id)
            );

            _mockRepository.Verify(repo => repo.PositionRepository.Create(It.IsAny<Positions>()), Times.Once);
            _mockRepository.Verify(repo => repo.TeamPositionRepository.Create(It.IsAny<TeamMemberPositions>()), Times.Once);
        }

        [Fact]
        public async Task ShouldReturnFail_ImageIdIsZero()
        {
            // Arrange
            string expectedErrorMessage = "Invalid ImageId Value";
            var teamMember = GetTeamMember();
            MapperSetup(teamMember);
            var handler = new CreateTeamHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object, _mockLocalizerConvertNull.Object    );

            // Act
            var result = await handler.Handle(new CreateTeamQuery(new TeamMemberDTO()), CancellationToken.None);
            // Assert
            Assert.Multiple(
                () => Assert.True(result.IsFailed),
                () => Assert.Equal(expectedErrorMessage, result.Errors.First().Message)
            );

        }

        [Fact]
        public async Task ShouldReturnFail_InvalidLogoType()
        {
            // Arrange
            string expectedErrorMessage = "CannotCreateTeamMemberLinkWithInvalidLogoType";
            _mockLocalizerConvertNull.Setup(x => x["CannotCreateTeamMemberLinkWithInvalidLogoType"])
            .Returns(new LocalizedString("CannotCreateTeamMemberLinkWithInvalidLogoType", expectedErrorMessage));

            var teamMemberDTO = GetTeamMemberWithLinksDTO();
            var teamMember = GetTeamMemberWithLinks();
            GetsAsyncRepositorySetup();
            MapperSetupWithLinks(teamMember, teamMemberDTO);
            var handler = new CreateTeamHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object, _mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new CreateTeamQuery(teamMemberDTO), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.True(result.IsFailed),
                () => Assert.Equal(expectedErrorMessage, result.Errors.First().Message)
            );

        }

        private void BasicRepositorySetup(TeamMember teamMember)
        {
            _mockRepository.Setup(repo => repo.TeamRepository.CreateAsync(teamMember)).ReturnsAsync(teamMember);

            _mockRepository.Setup(repo => repo.SaveChanges());
        }

        private void GetsAsyncRepositorySetup(List<TeamMemberLink>? link=null, List<TeamMemberPositions>? memberPos = null)
        {
            var linkList = link ?? new List<TeamMemberLink>();
            var posList = memberPos ?? new List<TeamMemberPositions>();

            _mockRepository.Setup(repo => repo.TeamLinkRepository
               .GetAllAsync(It.IsAny<Expression<Func<TeamMemberLink, bool>>>(), It.IsAny<Func<IQueryable<TeamMemberLink>,
               IIncludableQueryable<TeamMemberLink, object>>>())).ReturnsAsync(linkList);

            _mockRepository.Setup(repo => repo.TeamPositionRepository
                .GetAllAsync(It.IsAny<Expression<Func<TeamMemberPositions, bool>>>(), It.IsAny<Func<IQueryable<TeamMemberPositions>,
                IIncludableQueryable<TeamMemberPositions, object>>>())).ReturnsAsync(new List<TeamMemberPositions>());
        }
        private void MapperSetupWithLinks(TeamMember member, TeamMemberDTO dto)
        {
            _mockMapper.Setup(mapper => mapper.Map<TeamMember>(It.IsAny<object>()))
                .Returns(member);

            _mockMapper.Setup(mapper => mapper.Map<TeamMemberDTO>(It.IsAny<object>()))
                .Returns(dto);
        }
        
        private void MapperSetup(TeamMember member)
        {
            _mockMapper.Setup(mapper => mapper.Map<TeamMember>(It.IsAny<object>()))
                .Returns(member);

            _mockMapper.Setup(mapper => mapper.Map<TeamMemberDTO>(It.IsAny<object>()))
                .Returns(new TeamMemberDTO());
        }

        private TeamMemberDTO GetTeamMemberWithLinksDTO()
        {
            var teamMemberLink = new TeamMemberLinkDTO { Id = 1, LogoType = (BLL.DTO.Partners.LogoTypeDTO)10 };
            return new TeamMemberDTO
            {
                Id = 1,
                Name = "Test",
                Description = "Test",
                IsMain = true,
                ImageId = 1,
                TeamMemberLinks = new List<TeamMemberLinkDTO> { teamMemberLink },
                Positions = new List<PositionDTO>()
            };
        }

        private TeamMember GetTeamMemberWithLinks()
        {
            var teamMemberLink = new TeamMemberLink { Id = 1, LogoType = (DAL.Enums.LogoType)10 };
            return new TeamMember
            {
                Id = 1,
                Name = "Test",
                Description = "Test",
                IsMain = true,
                ImageId = 1,
                TeamMemberLinks = new List<TeamMemberLink> { teamMemberLink },
                Positions = new List<Positions>()
            };
        }

        private static TeamMember GetTeamMember(int imageId = 0)
        {
            return new TeamMember
            {
                Id = 1, Positions = new List<Positions>(), ImageId = imageId
            };
        }

        private static TeamMemberLink GetTeamMemberLink(int id)
        {
            return new TeamMemberLink
            {
                Id = id
            };
        }

        private static TeamMemberPositions GetTeamMemberPositions(int positionsId)
        {
            return new TeamMemberPositions
            {
                PositionsId = positionsId
            };
        }

        private static TeamMemberDTO GetTeamMemberDTO(List<PositionDTO> newPositions)
        {
            return new TeamMemberDTO
            {
                Positions = newPositions
            };
        }
    }
}
