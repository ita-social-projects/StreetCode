using System.Linq.Expressions;
using AutoMapper;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Team.TeamMembersLinks.Create;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Team.TeamLink
{
    public class CreateTeamMembersLinkTest
    {
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<CannotCreateSharedResource>> _mockLocalizerCannotCreate;
        private readonly Mock<IStringLocalizer<FailedToCreateSharedResource>> _mockLocalizerFailedToCreate;
        private readonly Mock<IStringLocalizer<CannotConvertNullSharedResource>> _mockLocalizerConvertNull;

        public CreateTeamMembersLinkTest()
        {
            this._mockRepository = new Mock<IRepositoryWrapper>();
            this._mockMapper = new Mock<IMapper>();
            this._mockLogger = new Mock<ILoggerService>();
            this._mockLocalizerCannotCreate = new Mock<IStringLocalizer<CannotCreateSharedResource>>();
            this._mockLocalizerFailedToCreate = new Mock<IStringLocalizer<FailedToCreateSharedResource>>();
            this._mockLocalizerConvertNull = new Mock<IStringLocalizer<CannotConvertNullSharedResource>>();
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_WhenTypeIsCorrect()
        {
            // Arrange
            var testTeamMemberLink = GetTeamMemberLink();

            this.SetupMapMethod(testTeamMemberLink);
            this.SetupCreateMethod(testTeamMemberLink);
            this.SetupSaveChangesMethod();

            var handler = new CreateTeamLinkHandler(this._mockMapper.Object, this._mockRepository.Object, this._mockLogger.Object, this._mockLocalizerCannotCreate.Object, this._mockLocalizerFailedToCreate.Object, this._mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new CreateTeamLinkQuery(GetTeamMemberLinkDTO()), CancellationToken.None);

            // Assert
            Assert.IsType<TeamMemberLinkDTO>(result.Value);
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_WhenPartnerAdded()
        {
            // Arrange
            var testTeamMemberLink = GetTeamMemberLink();

            this.SetupMapMethod(testTeamMemberLink);
            this.SetupCreateMethod(testTeamMemberLink);
            this.SetupSaveChangesMethod();

            var handler = new CreateTeamLinkHandler(this._mockMapper.Object, this._mockRepository.Object, this._mockLogger.Object, this._mockLocalizerCannotCreate.Object, this._mockLocalizerFailedToCreate.Object, this._mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new CreateTeamLinkQuery(GetTeamMemberLinkDTO()), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task ShouldThrowExeption_WhenSaveChangesIsNotSuccessful()
        {
            string expectedErrorMessage = "Failed to create a team";
            this._mockLocalizerFailedToCreate.Setup(x => x["FailedToCreateTeam"])
                .Returns(new LocalizedString("FailedToCreateTeam", expectedErrorMessage));

            // Arrange
            var testTeamMemberLink = GetTeamMemberLink();

            this.SetupMapMethod(testTeamMemberLink);
            this.SetupCreateMethod(testTeamMemberLink);
            this.SetupSaveChangesMethodWithErrorThrow(expectedErrorMessage);

            var handler = new CreateTeamLinkHandler(this._mockMapper.Object, this._mockRepository.Object, this._mockLogger.Object, this._mockLocalizerCannotCreate.Object, this._mockLocalizerFailedToCreate.Object, this._mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new CreateTeamLinkQuery(null!), CancellationToken.None);

            // Assert
            Assert.Equal(expectedErrorMessage, result.Errors[0].Message);

            this._mockRepository.Verify(x => x.StreetcodeRepository.GetAllAsync(It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), null), Times.Never);
        }

        [Fact]
        public async Task ShouldThrowExeption_WhenCreateNotSuccessful()
        {
            string expectedErrorMessage = "Cannot create team link";
            this._mockLocalizerCannotCreate.Setup(x => x["CannotCreateTeamLink"])
                .Returns(new LocalizedString("CannotCreateTeamLink", expectedErrorMessage));

            // Arrange
            var testTeamMemberLink = GetTeamMemberLink();

            this.SetupMapMethod(testTeamMemberLink);

            // The specific setup of the 'Create' method returned null, causing an error.
            this._mockRepository.Setup(x => x.TeamLinkRepository.Create(It.Is<TeamMemberLink>(y => y.Id == testTeamMemberLink.Id)))
                .Returns((TeamMemberLink)null!);

            var handler = new CreateTeamLinkHandler(this._mockMapper.Object, this._mockRepository.Object, this._mockLogger.Object, this._mockLocalizerCannotCreate.Object, this._mockLocalizerFailedToCreate.Object, this._mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new CreateTeamLinkQuery(null!), CancellationToken.None);

            // Assert
            Assert.Equal(expectedErrorMessage, result.Errors[0].Message);

            this._mockRepository.Verify(x => x.StreetcodeRepository.GetAllAsync(It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), null), Times.Never);
        }

        [Fact]
        public async Task ShouldThrowExeption_WhenMapNotSuccessful()
        {
            string expectedErrorMessage = "Cannot convert null to team link";
            this._mockLocalizerConvertNull.Setup(x => x["CannotConvertNullToTeamLink"])
                .Returns(new LocalizedString("CannotConvertNullToTeamLink", expectedErrorMessage));

            // Arrange
            // The specific setup of the 'Map' method returned null, causing an error.
            this._mockMapper.Setup(x => x.Map<TeamMemberLink>(It.IsAny<TeamMemberLinkCreateDTO>()))
                .Returns((TeamMemberLink)null!);

            var handler = new CreateTeamLinkHandler(this._mockMapper.Object, this._mockRepository.Object, this._mockLogger.Object, this._mockLocalizerCannotCreate.Object, this._mockLocalizerFailedToCreate.Object, this._mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new CreateTeamLinkQuery(null!), CancellationToken.None);

            // Assert
            Assert.Equal(expectedErrorMessage, result.Errors[0].Message);

            this._mockRepository.Verify(x => x.StreetcodeRepository.GetAllAsync(It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), null), Times.Never);
        }

        private static TeamMemberLink GetTeamMemberLink()
        {
            return new TeamMemberLink()
            {
                Id = 0,
            };
        }

        private static TeamMemberLinkCreateDTO GetTeamMemberLinkDTO()
        {
            return new TeamMemberLinkCreateDTO();
        }

        private void SetupMapMethod(TeamMemberLink teamMemberLink)
        {
            this._mockMapper.Setup(x => x.Map<TeamMemberLink>(It.IsAny<TeamMemberLinkCreateDTO>()))
                .Returns(teamMemberLink);
            this._mockMapper.Setup(x => x.Map<TeamMemberLinkDTO>(It.IsAny<TeamMemberLink>()))
                .Returns(new TeamMemberLinkDTO());
        }

        private void SetupCreateMethod(TeamMemberLink teamMemberLink)
        {
            this._mockRepository.Setup(x => x.TeamLinkRepository.Create(It.Is<TeamMemberLink>(y => y.Id == teamMemberLink.Id)))
                .Returns(teamMemberLink);
        }

        private void SetupSaveChangesMethod()
        {
            this._mockRepository.Setup(x => x.SaveChangesAsync())
                .ReturnsAsync(1);
        }

        private void SetupSaveChangesMethodWithErrorThrow(string expectedError)
        {
            this._mockRepository.Setup(x => x.SaveChanges())
                .Throws(new Exception(expectedError));
        }
    }
}
