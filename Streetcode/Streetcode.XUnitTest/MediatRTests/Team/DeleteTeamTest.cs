using System.Linq.Expressions;
using AutoMapper;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Team.Delete;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Team
{
    public class DeleteTeamTest
    {
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IRepositoryWrapper> mockRepository;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly Mock<IStringLocalizer<NoSharedResource>> mockLocalizerNo;

        public DeleteTeamTest()
        {
            this.mockRepository = new Mock<IRepositoryWrapper>();
            this.mockMapper = new Mock<IMapper>();
            this.mockLogger = new Mock<ILoggerService>();
            this.mockLocalizerNo = new Mock<IStringLocalizer<NoSharedResource>>();
        }

        [Fact]
        public async Task ShouldDeleteSuccessfully()
        {
            // Arrange
            var testTeam = GetTeam();

            this.SetupMapTeamMember(testTeam);
            this.SetupGetFirstOrDefaultAsync(testTeam);

            var handler = new DeleteTeamHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerNo.Object);

            // Act
            var result = await handler.Handle(new DeleteTeamQuery(testTeam.Id), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.True(result.IsSuccess));

            this.mockRepository.Verify(x => x.TeamRepository.Delete(It.Is<TeamMember>(x => x.Id == testTeam.Id)), Times.Once);
            this.mockRepository.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowException_IdNotExisting()
        {
            // Arrange
            var testTeam = GetTeam();
            var expectedError = "No team with such id";
            this.mockLocalizerNo.Setup(x => x["NoTeamWithSuchId"])
                .Returns(new LocalizedString("NoTeamWithSuchId", expectedError));

            this.SetupGetFirstOrDefaultAsync(null);

            var handler = new DeleteTeamHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerNo.Object);

            // Act
            var result = await handler.Handle(new DeleteTeamQuery(testTeam.Id), CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors[0].Message);

            this.mockRepository.Verify(x => x.TeamRepository.Delete(It.IsAny<TeamMember>()), Times.Never);
        }

        [Fact]
        public async Task ShouldThrowException_SaveChangesAsyncIsNotSuccessful()
        {
            // Arrange
            var testTeam = GetTeam();
            var expectedError = "The team wasn't added";

            this.SetupMapTeamMember(testTeam);
            this.SetupGetFirstOrDefaultAsync(testTeam);
            this.SetupSaveChangesException(expectedError);

            var handler = new DeleteTeamHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerNo.Object);

            // Act
            var result = await handler.Handle(new DeleteTeamQuery(testTeam.Id), CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors[0].Message);
        }

        private static TeamMember GetTeam()
        {
            return new TeamMember
            {
                Id = 1,
            };
        }

        private static TeamMemberDTO GetTeamDTO()
        {
            return new TeamMemberDTO();
        }

        private void SetupMapTeamMember(TeamMember teamMember)
        {
            this.mockMapper.Setup(x => x.Map<TeamMemberDTO>(teamMember))
                .Returns(GetTeamDTO());
        }

        private void SetupGetFirstOrDefaultAsync(TeamMember? teamMember)
        {
            this.mockRepository
                .Setup(x => x.TeamRepository.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<TeamMember, bool>>>(), null))
                .ReturnsAsync(teamMember);
        }

        private void SetupSaveChangesException(string errorMessage)
        {
            this.mockRepository
                .Setup(x => x.SaveChanges())
                .Throws(new Exception(errorMessage));
        }
    }
}
