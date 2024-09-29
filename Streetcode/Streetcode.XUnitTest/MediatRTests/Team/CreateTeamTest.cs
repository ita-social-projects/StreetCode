using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Team.Create;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Repositories.Interfaces.Base;

using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Team
{
    public class CreateTeamTest
    {
        private readonly Mock<IRepositoryWrapper> mockRepository;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly Mock<IStringLocalizer<CannotConvertNullSharedResource>> mockLocalizerConvertNull;

        public CreateTeamTest()
        {
            this.mockMapper = new Mock<IMapper>();
            this.mockRepository = new Mock<IRepositoryWrapper>();
            this.mockLogger = new Mock<ILoggerService>();
            this.mockLocalizerConvertNull = new Mock<IStringLocalizer<CannotConvertNullSharedResource>>();
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_TeamCreated()
        {
            // Arrange
            var teamMember = GetTeamMember(1);
            this.MapperSetup(teamMember);
            this.BasicRepositorySetup(teamMember);
            this.GetsAsyncRepositorySetup();

            var handler = new CreateTeamHandler(this.mockMapper.Object, this.mockRepository.Object, this.mockLogger.Object, this.mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new CreateTeamQuery(new TeamMemberCreateDTO()), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task ShouldThrowExeption_SaveChangesIsNotSuccessful()
        {
            // Arrange
            const string exceptionMessage = "Failed to create a team member";

            var teamMember = GetTeamMember(1);
            this.MapperSetup(teamMember);
            this.GetsAsyncRepositorySetup();
            this.mockRepository.Setup(repo => repo.TeamRepository.CreateAsync(teamMember)).ReturnsAsync(teamMember);
            this.mockRepository.Setup(repo => repo.SaveChanges()).Throws(new Exception(exceptionMessage));

            var handler = new CreateTeamHandler(this.mockMapper.Object, this.mockRepository.Object, this.mockLogger.Object, this.mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new CreateTeamQuery(new TeamMemberCreateDTO()), CancellationToken.None);

            // Assert
            Assert.Equal(exceptionMessage, result.Errors[0].Message);
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_CorrectType()
        {
            // Arrange
            var teamMember = GetTeamMember(1);
            this.MapperSetup(teamMember);
            this.BasicRepositorySetup(teamMember);
            this.GetsAsyncRepositorySetup();

            var handler = new CreateTeamHandler(this.mockMapper.Object, this.mockRepository.Object, this.mockLogger.Object, this.mockLocalizerConvertNull.Object);

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
            this.mockRepository.Setup(repo => repo.TeamPositionRepository.Create(It.IsAny<TeamMemberPositions>()));
            this.mockRepository.Setup(repo => repo.PositionRepository.Create(It.IsAny<Positions>()))
                .Returns(new Positions());

            var handler = new CreateTeamHandler(this.mockMapper.Object, this.mockRepository.Object, this.mockLogger.Object, this.mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new CreateTeamQuery(teamMemberDTO), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.True(result.IsSuccess),
                () => Assert.Equal(0, result.Value.Id));

            this.mockRepository.Verify(repo => repo.PositionRepository.Create(It.IsAny<Positions>()), Times.Once);
            this.mockRepository.Verify(repo => repo.TeamPositionRepository.Create(It.IsAny<TeamMemberPositions>()), Times.Once);
        }

        [Fact]
        public async Task ShouldReturnFail_ImageIdIsZero()
        {
            // Arrange
            string expectedErrorMessage = "Invalid ImageId Value";
            var teamMember = GetTeamMember();
            this.MapperSetup(teamMember);
            var handler = new CreateTeamHandler(this.mockMapper.Object, this.mockRepository.Object, this.mockLogger.Object, this.mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new CreateTeamQuery(new TeamMemberCreateDTO()), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.True(result.IsFailed),
                () => Assert.Equal(expectedErrorMessage, result.Errors[0].Message));
        }

        [Fact]
        public async Task ShouldReturnFail_InvalidLogoType()
        {
            // Arrange
            string expectedErrorMessage = "CannotCreateTeamMemberLinkWithInvalidLogoType";
            this.mockLocalizerConvertNull.Setup(x => x["CannotCreateTeamMemberLinkWithInvalidLogoType"])
            .Returns(new LocalizedString("CannotCreateTeamMemberLinkWithInvalidLogoType", expectedErrorMessage));

            var teamMemberDTO = this.GetTeamMemberWithLinksDTO();
            var teamMember = this.GetTeamMemberWithLinks();
            this.GetsAsyncRepositorySetup();
            this.MapperSetupWithLinks(teamMember, teamMemberDTO);
            var handler = new CreateTeamHandler(this.mockMapper.Object, this.mockRepository.Object, this.mockLogger.Object, this.mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new CreateTeamQuery(teamMemberDTO), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.True(result.IsFailed),
                () => Assert.Equal(expectedErrorMessage, result.Errors[0].Message));
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
            this.mockRepository.Setup(repo => repo.TeamRepository.CreateAsync(teamMember)).ReturnsAsync(teamMember);

            this.mockRepository.Setup(repo => repo.SaveChanges());
        }

        private void GetsAsyncRepositorySetup(List<TeamMemberLink>? link = null)
        {
            var linkList = link ?? new List<TeamMemberLink>();

            this.mockRepository.Setup(repo => repo.TeamLinkRepository
               .GetAllAsync(It.IsAny<Expression<Func<TeamMemberLink, bool>>>(), It.IsAny<Func<IQueryable<TeamMemberLink>,
               IIncludableQueryable<TeamMemberLink, object>>>())).ReturnsAsync(linkList);

            this.mockRepository.Setup(repo => repo.TeamPositionRepository
                .GetAllAsync(It.IsAny<Expression<Func<TeamMemberPositions, bool>>>(), It.IsAny<Func<IQueryable<TeamMemberPositions>,
                IIncludableQueryable<TeamMemberPositions, object>>>())).ReturnsAsync(new List<TeamMemberPositions>());
        }

        private void MapperSetupWithLinks(TeamMember member, TeamMemberCreateDTO dto)
        {
            this.mockMapper.Setup(mapper => mapper.Map<TeamMember>(It.IsAny<object>()))
                .Returns(member);

            this.mockMapper.Setup(mapper => mapper.Map<TeamMemberCreateDTO>(It.IsAny<object>()))
                .Returns(dto);
        }

        private void MapperSetup(TeamMember member)
        {
            this.mockMapper.Setup(mapper => mapper.Map<TeamMember>(It.IsAny<object>()))
                .Returns(member);

            this.mockMapper.Setup(mapper => mapper.Map<TeamMemberDTO>(It.IsAny<object>()))
                .Returns(new TeamMemberDTO());
        }

        private TeamMemberCreateDTO GetTeamMemberWithLinksDTO()
        {
            var teamMemberLink = new TeamMemberLinkCreateDTO { LogoType = (BLL.DTO.Partners.LogoTypeDTO)10 };
            return new TeamMemberCreateDTO
            {
                Name = "Test",
                Description = "Test",
                IsMain = true,
                ImageId = 1,
                TeamMemberLinks = new List<TeamMemberLinkCreateDTO> { teamMemberLink },
                Positions = new List<PositionDTO>(),
            };
        }

        private TeamMember GetTeamMemberWithLinks()
        {
            var teamMemberLink = new TeamMemberLink { LogoType = (DAL.Enums.LogoType)10 };
            return new TeamMember
            {
                Id = 1,
                Name = "Test",
                Description = "Test",
                IsMain = true,
                ImageId = 1,
                TeamMemberLinks = new List<TeamMemberLink> { teamMemberLink },
                Positions = new List<Positions>(),
            };
        }
    }
}
