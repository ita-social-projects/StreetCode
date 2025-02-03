using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Team.TeamMembersLinks.GetAll;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Team.TeamLink
{
    public class GetAllTeamMembersLinksTest
    {
        private readonly Mock<IRepositoryWrapper> mockRepository;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> mockLocalizerCannotFind;

        public GetAllTeamMembersLinksTest()
        {
            this.mockRepository = new Mock<IRepositoryWrapper>();
            this.mockMapper = new Mock<IMapper>();
            this.mockLogger = new Mock<ILoggerService>();
            this.mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_WhenTypeIsCorrect()
        {
            // Arrange
            this.SetupMapMethod(GetListTeamMemberLinkDTO());
            this.SetupGetAllAsyncMethod(GetTeamMemberLinksList());

            var handler = new GetAllTeamLinkHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new GetAllTeamLinkQuery(), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.IsType<List<TeamMemberLinkDto>>(result.ValueOrDefault));
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_WhenCountMatch()
        {
            // Arrange
            this.SetupMapMethod(GetListTeamMemberLinkDTO());
            this.SetupGetAllAsyncMethod(GetTeamMemberLinksList());

            var handler = new GetAllTeamLinkHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new GetAllTeamLinkQuery(), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.Equal(GetTeamMemberLinksList().Count(), result.Value.Count()));
        }

        [Fact]
        public async Task ShouldThrowExeption_WhenIdNotExist()
        {
            // Arrange
            const string expectedError = "Cannot find any team links";
            this.mockLocalizerCannotFind.Setup(x => x["CannotFindAnyTeamLinks"])
              .Returns(new LocalizedString("CannotFindAnyTeamLinks", expectedError));
            this.SetupMapMethod(GetTeamMemberLinksListWithNotExistingId());

            var handler = new GetAllTeamLinkHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new GetAllTeamLinkQuery(), CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors[0].Message);

            this.mockMapper.Verify(x => x.Map<IEnumerable<TeamMemberLinkDto>>(It.IsAny<IEnumerable<TeamMemberLink>>()), Times.Never);
        }

        private static IEnumerable<TeamMemberLink> GetTeamMemberLinksList()
        {
            var partners = new List<TeamMemberLink>
            {
                new TeamMemberLink
                {
                    Id = 1,
                },
                new TeamMemberLink
                {
                    Id = 2,
                },
            };
            return partners;
        }

        private static List<TeamMemberLink> GetTeamMemberLinksListWithNotExistingId()
        {
            return null;
        }

        private static List<TeamMemberLinkDto> GetListTeamMemberLinkDTO()
        {
            var partnersDTO = new List<TeamMemberLinkDto>
            {
                new TeamMemberLinkDto
                {
                    Id = 1,
                },
                new TeamMemberLinkDto
                {
                    Id = 2,
                },
            };
            return partnersDTO;
        }

        private void SetupMapMethod(IEnumerable<TeamMemberLinkDto> teamMemberLinksDTO)
        {
            this.mockMapper.Setup(x => x.Map<IEnumerable<TeamMemberLinkDto>>(It.IsAny<IEnumerable<TeamMemberLink>>()))
                .Returns(teamMemberLinksDTO);
        }

        private void SetupMapMethod(IEnumerable<TeamMemberLink> teamMemberLinks)
        {
            this.mockRepository.Setup(x => x.TeamLinkRepository.GetAllAsync(
                null,
                It.IsAny<Func<IQueryable<TeamMemberLink>, IIncludableQueryable<TeamMemberLink, object>>>()))
                .ReturnsAsync(teamMemberLinks);
        }

        private void SetupGetAllAsyncMethod(IEnumerable<TeamMemberLink> teamMemberLinks)
        {
            this.mockRepository.Setup(x => x.TeamLinkRepository.GetAllAsync(
                null,
                It.IsAny<Func<IQueryable<TeamMemberLink>, IIncludableQueryable<TeamMemberLink, object>>>()))
                .ReturnsAsync(teamMemberLinks);
        }
    }
}
