using AutoMapper;
using Moq;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.MediatR.Team.Delete;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.SharedResource;

namespace Streetcode.XUnitTest.MediatRTests.Teams
{
    public class DeleteTeamTest
    {
        private Mock<IMapper> _mockMapper;
        private Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<IStringLocalizer<NoSharedResource>> _mockLocalizerNo;


        public DeleteTeamTest()
        {
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _mockLocalizerNo = new Mock<IStringLocalizer<NoSharedResource>>();
        }

        private void SetupMapTeamMember(TeamMember teamMember)
        {
            _mockMapper.Setup(x => x.Map<TeamMemberDTO>(teamMember))
                .Returns(GetTeamDTO());
        }

        private void SetupGetFirstOrDefaultAsync(TeamMember? teamMember)
        {
            _mockRepository.Setup(x => x.TeamRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<TeamMember, bool>>>(), null))
                .ReturnsAsync(teamMember);
        }

        private void SetupSaveChangesException(string errorMessage)
        {
            _mockRepository.Setup(x => x.SaveChanges())
                .Throws(new Exception(errorMessage));
        }

        [Fact]
        public async Task ShouldDeleteSuccessfully()
        {
            // Arrange
            var testTeam = GetTeam();

            SetupMapTeamMember(testTeam);
            SetupGetFirstOrDefaultAsync(testTeam);

            var handler = new DeleteTeamHandler(_mockRepository.Object, _mockMapper.Object, _mockLocalizerNo.Object);

            // Act
            var result = await handler.Handle(new DeleteTeamQuery(testTeam.Id), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.True(result.IsSuccess)
            );

            _mockRepository.Verify(x => x.TeamRepository.Delete(It.Is<TeamMember>(x => x.Id == testTeam.Id)), Times.Once);
            _mockRepository.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowException_IdNotExisting()
        {
            // Arrange
            var testTeam = GetTeam();
            var expectedError = "No team with such id";

            SetupGetFirstOrDefaultAsync(null);

            var handler = new DeleteTeamHandler(_mockRepository.Object, _mockMapper.Object, _mockLocalizerNo.Object);

            // Act
            var result = await handler.Handle(new DeleteTeamQuery(testTeam.Id), CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors.First().Message);

            _mockRepository.Verify(x => x.TeamRepository.Delete(It.IsAny<TeamMember>()), Times.Never);
        }

        [Fact]
        public async Task ShouldThrowException_SaveChangesAsyncIsNotSuccessful()
        {
            // Arrange
            var testTeam = GetTeam();
            var expectedError = "The team wasn't added";

            SetupMapTeamMember(testTeam);
            SetupGetFirstOrDefaultAsync(testTeam);
            SetupSaveChangesException(expectedError);

            var handler = new DeleteTeamHandler(_mockRepository.Object, _mockMapper.Object, _mockLocalizerNo.Object);

            // Act
            var result = await handler.Handle(new DeleteTeamQuery(testTeam.Id), CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors.First().Message);
        }

        private static TeamMember GetTeam()
        {
            return new TeamMember
            {
                Id = 1
            };
        }

        private static TeamMemberDTO GetTeamDTO()
        {
            return new TeamMemberDTO();
        }
    }
}
