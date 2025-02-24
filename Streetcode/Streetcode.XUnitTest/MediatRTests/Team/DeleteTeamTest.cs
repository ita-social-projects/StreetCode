using System.Linq.Expressions;
using AutoMapper;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Team.Delete;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Team
{
    public class DeleteTeamTest
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<NoSharedResource>> _mockLocalizerNo;

        public DeleteTeamTest()
        {
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerService>();
            _mockLocalizerNo = new Mock<IStringLocalizer<NoSharedResource>>();
        }

        [Fact]
        public async Task ShouldDeleteSuccessfully()
        {
            // Arrange
            var testTeam = GetTeam();

            this.SetupMapTeamMember(testTeam);
            this.SetupGetFirstOrDefaultAsync(testTeam);

            var handler = new DeleteTeamHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerNo.Object);

            // Act
            var result = await handler.Handle(new DeleteTeamQuery(testTeam.Id), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.True(result.IsSuccess));

            _mockRepository.Verify(x => x.TeamRepository.Delete(It.Is<TeamMember>(x => x.Id == testTeam.Id)), Times.Once);
            _mockRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowException_IdNotExisting()
        {
            // Arrange
            var testTeam = GetTeam();
            var expectedError = "No team with such id";
            this._mockLocalizerNo.Setup(x => x["NoTeamWithSuchId"])
                .Returns(new LocalizedString("NoTeamWithSuchId", expectedError));

            this.SetupGetFirstOrDefaultAsync(null);

            var handler = new DeleteTeamHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerNo.Object);

            // Act
            var result = await handler.Handle(new DeleteTeamQuery(testTeam.Id), CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors[0].Message);

            _mockRepository.Verify(x => x.TeamRepository.Delete(It.IsAny<TeamMember>()), Times.Never);
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

            var handler = new DeleteTeamHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerNo.Object);

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
            _mockMapper.Setup(x => x.Map<TeamMemberDTO>(teamMember))
                .Returns(GetTeamDTO());
        }

        private void SetupGetFirstOrDefaultAsync(TeamMember? teamMember)
        {
            _mockRepository
                .Setup(x => x.TeamRepository.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<TeamMember, bool>>>(), null))
                .ReturnsAsync(teamMember);
        }

        private void SetupSaveChangesException(string errorMessage)
        {
            _mockRepository
                .Setup(x => x.SaveChangesAsync())
                .Throws(new Exception(errorMessage));
        }
    }
}
