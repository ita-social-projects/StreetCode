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
        private readonly Mock<IRepositoryWrapper> mockRepository;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly Mock<IStringLocalizer<CannotCreateSharedResource>> mockLocalizerCannotCreate;
        private readonly Mock<IStringLocalizer<FailedToCreateSharedResource>> mockLocalizerFailedToCreate;
        private readonly Mock<IStringLocalizer<CannotConvertNullSharedResource>> mockLocalizerConvertNull;

        public CreateTeamMembersLinkTest()
        {
            this.mockRepository = new Mock<IRepositoryWrapper>();
            this.mockMapper = new Mock<IMapper>();
            this.mockLogger = new Mock<ILoggerService>();
            this.mockLocalizerCannotCreate = new Mock<IStringLocalizer<CannotCreateSharedResource>>();
            this.mockLocalizerFailedToCreate = new Mock<IStringLocalizer<FailedToCreateSharedResource>>();
            this.mockLocalizerConvertNull = new Mock<IStringLocalizer<CannotConvertNullSharedResource>>();
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_WhenTypeIsCorrect()
        {
            // Arrange
            var testTeamMemberLink = GetTeamMemberLink();

            this.SetupMapMethod(testTeamMemberLink);
            this.SetupCreateMethod(testTeamMemberLink);
            this.SetupSaveChangesMethod();

            var handler = new CreateTeamLinkHandler(this.mockMapper.Object, this.mockRepository.Object, this.mockLogger.Object, this.mockLocalizerCannotCreate.Object, this.mockLocalizerFailedToCreate.Object, this.mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new CreateTeamLinkQuery(GetTeamMemberLinkDTO()), CancellationToken.None);

            // Assert
            Assert.IsType<TeamMemberLinkDto>(result.Value);
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_WhenPartnerAdded()
        {
            // Arrange
            var testTeamMemberLink = GetTeamMemberLink();

            this.SetupMapMethod(testTeamMemberLink);
            this.SetupCreateMethod(testTeamMemberLink);
            this.SetupSaveChangesMethod();

            var handler = new CreateTeamLinkHandler(this.mockMapper.Object, this.mockRepository.Object, this.mockLogger.Object, this.mockLocalizerCannotCreate.Object, this.mockLocalizerFailedToCreate.Object, this.mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new CreateTeamLinkQuery(GetTeamMemberLinkDTO()), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task ShouldThrowExeption_WhenSaveChangesIsNotSuccessful()
        {
            string expectedErrorMessage = "Failed to create a team";
            this.mockLocalizerFailedToCreate.Setup(x => x["FailedToCreateTeam"])
                .Returns(new LocalizedString("FailedToCreateTeam", expectedErrorMessage));

            // Arrange
            var testTeamMemberLink = GetTeamMemberLink();

            this.SetupMapMethod(testTeamMemberLink);
            this.SetupCreateMethod(testTeamMemberLink);
            this.SetupSaveChangesMethodWithErrorThrow(expectedErrorMessage);

            var handler = new CreateTeamLinkHandler(this.mockMapper.Object, this.mockRepository.Object, this.mockLogger.Object, this.mockLocalizerCannotCreate.Object, this.mockLocalizerFailedToCreate.Object, this.mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new CreateTeamLinkQuery(null!), CancellationToken.None);

            // Assert
            Assert.Equal(expectedErrorMessage, result.Errors[0].Message);

            this.mockRepository.Verify(x => x.StreetcodeRepository.GetAllAsync(It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), null), Times.Never);
        }

        [Fact]
        public async Task ShouldThrowExeption_WhenCreateNotSuccessful()
        {
            string expectedErrorMessage = "Cannot create team link";
            this.mockLocalizerCannotCreate.Setup(x => x["CannotCreateTeamLink"])
                .Returns(new LocalizedString("CannotCreateTeamLink", expectedErrorMessage));

            // Arrange
            var testTeamMemberLink = GetTeamMemberLink();

            this.SetupMapMethod(testTeamMemberLink);

            // The specific setup of the 'Create' method returned null, causing an error.
            this.mockRepository.Setup(x => x.TeamLinkRepository.Create(It.Is<TeamMemberLink>(y => y.Id == testTeamMemberLink.Id)))
                .Returns((TeamMemberLink)null!);

            var handler = new CreateTeamLinkHandler(this.mockMapper.Object, this.mockRepository.Object, this.mockLogger.Object, this.mockLocalizerCannotCreate.Object, this.mockLocalizerFailedToCreate.Object, this.mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new CreateTeamLinkQuery(null!), CancellationToken.None);

            // Assert
            Assert.Equal(expectedErrorMessage, result.Errors[0].Message);

            this.mockRepository.Verify(x => x.StreetcodeRepository.GetAllAsync(It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), null), Times.Never);
        }

        [Fact]
        public async Task ShouldThrowExeption_WhenMapNotSuccessful()
        {
            string expectedErrorMessage = "Cannot convert null to team link";
            this.mockLocalizerConvertNull.Setup(x => x["CannotConvertNullToTeamLink"])
                .Returns(new LocalizedString("CannotConvertNullToTeamLink", expectedErrorMessage));

            // Arrange
            // The specific setup of the 'Map' method returned null, causing an error.
            this.mockMapper.Setup(x => x.Map<TeamMemberLink>(It.IsAny<TeamMemberLinkCreateDto>()))
                .Returns((TeamMemberLink)null!);

            var handler = new CreateTeamLinkHandler(this.mockMapper.Object, this.mockRepository.Object, this.mockLogger.Object, this.mockLocalizerCannotCreate.Object, this.mockLocalizerFailedToCreate.Object, this.mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new CreateTeamLinkQuery(null!), CancellationToken.None);

            // Assert
            Assert.Equal(expectedErrorMessage, result.Errors[0].Message);

            this.mockRepository.Verify(x => x.StreetcodeRepository.GetAllAsync(It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), null), Times.Never);
        }

        private static TeamMemberLink GetTeamMemberLink()
        {
            return new TeamMemberLink()
            {
                Id = 0,
            };
        }

        private static TeamMemberLinkCreateDto GetTeamMemberLinkDTO()
        {
            return new TeamMemberLinkCreateDto();
        }

        private void SetupMapMethod(TeamMemberLink teamMemberLink)
        {
            this.mockMapper.Setup(x => x.Map<TeamMemberLink>(It.IsAny<TeamMemberLinkCreateDto>()))
                .Returns(teamMemberLink);
            this.mockMapper.Setup(x => x.Map<TeamMemberLinkDto>(It.IsAny<TeamMemberLink>()))
                .Returns(new TeamMemberLinkDto());
        }

        private void SetupCreateMethod(TeamMemberLink teamMemberLink)
        {
            this.mockRepository.Setup(x => x.TeamLinkRepository.Create(It.Is<TeamMemberLink>(y => y.Id == teamMemberLink.Id)))
                .Returns(teamMemberLink);
        }

        private void SetupSaveChangesMethod()
        {
            this.mockRepository.Setup(x => x.SaveChangesAsync())
                .ReturnsAsync(1);
        }

        private void SetupSaveChangesMethodWithErrorThrow(string expectedError)
        {
            this.mockRepository.Setup(x => x.SaveChanges())
                .Throws(new Exception(expectedError));
        }
    }
}
