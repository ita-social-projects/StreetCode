using AutoMapper;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Team.TeamMembersLinks.Create;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
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
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerService>();
            _mockLocalizerCannotCreate = new Mock<IStringLocalizer<CannotCreateSharedResource>>();
            _mockLocalizerFailedToCreate = new Mock<IStringLocalizer<FailedToCreateSharedResource>>();
            _mockLocalizerConvertNull = new Mock<IStringLocalizer<CannotConvertNullSharedResource>>();
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_WhenTypeIsCorrect()
        {
            //Arrange
            var testTeamMemberLink = GetTeamMemberLink();

            SetupMapMethod(testTeamMemberLink);
            SetupCreateMethod(testTeamMemberLink);
            SetupSaveChangesMethod();

            var handler = new CreateTeamLinkHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object, _mockLocalizerCannotCreate.Object, _mockLocalizerFailedToCreate.Object, _mockLocalizerConvertNull.Object);

            //Act
            var result = await handler.Handle(new CreateTeamLinkQuery(GetTeamMemberLinkDTO()), CancellationToken.None);

            //Assert
            Assert.IsType<TeamMemberLinkDTO>(result.Value);
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_WhenPartnerAdded()
        {
            //Arrange
            var testTeamMemberLink = GetTeamMemberLink();

            SetupMapMethod(testTeamMemberLink);
            SetupCreateMethod(testTeamMemberLink);
            SetupSaveChangesMethod();

            var handler = new CreateTeamLinkHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object, _mockLocalizerCannotCreate.Object, _mockLocalizerFailedToCreate.Object, _mockLocalizerConvertNull.Object);

            //Act
            var result = await handler.Handle(new CreateTeamLinkQuery(GetTeamMemberLinkDTO()), CancellationToken.None);

            //Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task ShouldThrowExeption_WhenSaveChangesIsNotSuccessful()
        {
            string expectedErrorMessage = "Failed to create a team";
            _mockLocalizerFailedToCreate.Setup(x => x["FailedToCreateTeam"])
                .Returns(new LocalizedString("FailedToCreateTeam", expectedErrorMessage));
            //Arrange
            var testTeamMemberLink = GetTeamMemberLink();

            SetupMapMethod(testTeamMemberLink);
            SetupCreateMethod(testTeamMemberLink);
            SetupSaveChangesMethodWithErrorThrow(expectedErrorMessage);

            var handler = new CreateTeamLinkHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object, _mockLocalizerCannotCreate.Object, _mockLocalizerFailedToCreate.Object, _mockLocalizerConvertNull.Object);

            //Act
            var result = await handler.Handle(new CreateTeamLinkQuery(null), CancellationToken.None);

            //Assert
            Assert.Equal(expectedErrorMessage, result.Errors.First().Message);

            _mockRepository.Verify(x => x.StreetcodeRepository.GetAllAsync(It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), null), Times.Never);
        }

        [Fact]
        public async Task ShouldThrowExeption_WhenCreateNotSuccessful()
        {
            string expectedErrorMessage = "Cannot create team link";
            _mockLocalizerCannotCreate.Setup(x => x["CannotCreateTeamLink"])
                .Returns(new LocalizedString("CannotCreateTeamLink", expectedErrorMessage));

            //Arrange
            var testTeamMemberLink = GetTeamMemberLink();

            SetupMapMethod(testTeamMemberLink);

            //The specific setup of the 'Create' method returned null, causing an error.
            _mockRepository.Setup(x => x.TeamLinkRepository.Create(It.Is<TeamMemberLink>(y => y.Id == testTeamMemberLink.Id)))
                .Returns((TeamMemberLink)null);

            var handler = new CreateTeamLinkHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object, _mockLocalizerCannotCreate.Object, _mockLocalizerFailedToCreate.Object, _mockLocalizerConvertNull.Object);

            //Act
            var result = await handler.Handle(new CreateTeamLinkQuery(null), CancellationToken.None);

            //Assert
            Assert.Equal(expectedErrorMessage, result.Errors.First().Message);

            _mockRepository.Verify(x => x.StreetcodeRepository.GetAllAsync(It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), null), Times.Never);
        }

        [Fact]
        public async Task ShouldThrowExeption_WhenMapNotSuccessful()
        {
            string expectedErrorMessage = "Cannot convert null to team link";
            _mockLocalizerConvertNull.Setup(x => x["CannotConvertNullToTeamLink"])
                .Returns(new LocalizedString("CannotConvertNullToTeamLink", expectedErrorMessage));
            //Arrange
            //The specific setup of the 'Map' method returned null, causing an error.
            _mockMapper.Setup(x => x.Map<TeamMemberLink>(It.IsAny<TeamMemberLinkCreateDTO>()))
                .Returns((TeamMemberLink)null);

            var handler = new CreateTeamLinkHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object, _mockLocalizerCannotCreate.Object, _mockLocalizerFailedToCreate.Object, _mockLocalizerConvertNull.Object);

            //Act
            var result = await handler.Handle(new CreateTeamLinkQuery(null), CancellationToken.None);

            //Assert
            Assert.Equal(expectedErrorMessage, result.Errors.First().Message);

            _mockRepository.Verify(x => x.StreetcodeRepository.GetAllAsync(It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), null), Times.Never);
        }

        private void SetupMapMethod(TeamMemberLink teamMemberLink)
        {
            _mockMapper.Setup(x => x.Map<TeamMemberLink>(It.IsAny<TeamMemberLinkCreateDTO>()))
                .Returns(teamMemberLink);
            _mockMapper.Setup(x => x.Map<TeamMemberLinkDTO>(It.IsAny<TeamMemberLink>()))
                .Returns(new TeamMemberLinkDTO());
        }

        private void SetupCreateMethod(TeamMemberLink teamMemberLink)
        {
            _mockRepository.Setup(x => x.TeamLinkRepository.Create(It.Is<TeamMemberLink>(y => y.Id == teamMemberLink.Id)))
                .Returns(teamMemberLink);
        }

        private void SetupSaveChangesMethod()
        {
            _mockRepository.Setup(x => x.SaveChangesAsync())
                .ReturnsAsync(1);
        }

        private void SetupSaveChangesMethodWithErrorThrow(string expectedError)
        {
            _mockRepository.Setup(x => x.SaveChanges())
                .Throws(new Exception(expectedError));
        }

        private static TeamMemberLink GetTeamMemberLink()
        {
            return new TeamMemberLink()
            {
                Id = 0
            };
        }

        private static TeamMemberLinkCreateDTO GetTeamMemberLinkDTO()
        {
            return new TeamMemberLinkCreateDTO();
        }
    }
}
