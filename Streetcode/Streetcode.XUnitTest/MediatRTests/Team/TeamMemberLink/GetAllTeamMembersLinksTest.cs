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
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizerCannotFind;

        public GetAllTeamMembersLinksTest()
        {
            this._mockRepository = new Mock<IRepositoryWrapper>();
            this._mockMapper = new Mock<IMapper>();
            this._mockLogger = new Mock<ILoggerService>();
            this._mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_WhenTypeIsCorrect()
        {
            // Arrange
            this.SetupMapMethod(GetListTeamMemberLinkDTO());
            this.SetupGetAllAsyncMethod(GetTeamMemberLinksList());

            var handler = new GetAllTeamLinkHandler(this._mockRepository.Object, this._mockMapper.Object, this._mockLogger.Object, this._mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new GetAllTeamLinkQuery(), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.IsType<List<TeamMemberLinkDTO>>(result.ValueOrDefault));
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_WhenCountMatch()
        {
            // Arrange
            this.SetupMapMethod(GetListTeamMemberLinkDTO());
            this.SetupGetAllAsyncMethod(GetTeamMemberLinksList());

            var handler = new GetAllTeamLinkHandler(this._mockRepository.Object, this._mockMapper.Object, this._mockLogger.Object, this._mockLocalizerCannotFind.Object);

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
            this._mockLocalizerCannotFind.Setup(x => x["CannotFindAnyTeamLinks"])
              .Returns(new LocalizedString("CannotFindAnyTeamLinks", expectedError));
            this.SetupMapMethod(GetTeamMemberLinksListWithNotExistingId());

            var handler = new GetAllTeamLinkHandler(this._mockRepository.Object, this._mockMapper.Object, this._mockLogger.Object, this._mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new GetAllTeamLinkQuery(), CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors[0].Message);

            this._mockMapper.Verify(x => x.Map<IEnumerable<TeamMemberLinkDTO>>(It.IsAny<IEnumerable<TeamMemberLink>>()), Times.Never);
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
            return new List<TeamMemberLink>();
        }

        private static List<TeamMemberLinkDTO> GetListTeamMemberLinkDTO()
        {
            var partnersDTO = new List<TeamMemberLinkDTO>
            {
                new TeamMemberLinkDTO
                {
                    Id = 1,
                },
                new TeamMemberLinkDTO
                {
                    Id = 2,
                },
            };
            return partnersDTO;
        }

        private void SetupMapMethod(IEnumerable<TeamMemberLinkDTO> teamMemberLinksDTO)
        {
            this._mockMapper.Setup(x => x.Map<IEnumerable<TeamMemberLinkDTO>>(It.IsAny<IEnumerable<TeamMemberLink>>()))
                .Returns(teamMemberLinksDTO);
        }

        private void SetupMapMethod(IEnumerable<TeamMemberLink> teamMemberLinks)
        {
            this._mockRepository.Setup(x => x.TeamLinkRepository.GetAllAsync(
                null,
                It.IsAny<Func<IQueryable<TeamMemberLink>, IIncludableQueryable<TeamMemberLink, object>>>()))
                .ReturnsAsync(teamMemberLinks);
        }

        private void SetupGetAllAsyncMethod(IEnumerable<TeamMemberLink> teamMemberLinks)
        {
            this._mockRepository.Setup(x => x.TeamLinkRepository.GetAllAsync(
                null,
                It.IsAny<Func<IQueryable<TeamMemberLink>, IIncludableQueryable<TeamMemberLink, object>>>()))
                .ReturnsAsync(teamMemberLinks);
        }
    }
}
