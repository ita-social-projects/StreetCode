using AutoMapper;
using Moq;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.MediatR.Team.TeamMembersLinks.Create;
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

        public CreateTeamMembersLinkTest()
        {
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_WhenTypeIsCorrect()
        {
            //Arrange
            var testTeamMemberLink = GetTeamMemberLink();

            _mockMapper.Setup(x => x.Map<TeamMemberLink>(It.IsAny<TeamMemberLinkDTO>()))
                .Returns(testTeamMemberLink);
            _mockMapper.Setup(x => x.Map<TeamMemberLinkDTO>(It.IsAny<TeamMemberLink>()))
                .Returns(GetTeamMemberLinkDTO());

            _mockRepository.Setup(x => x.TeamLinkRepository.Create(It.Is<TeamMemberLink>(y => y.Id == testTeamMemberLink.Id)))
                .Returns(testTeamMemberLink);

            _mockRepository.Setup(x => x.SaveChangesAsync())
                .ReturnsAsync(1);

            var handler = new CreateTeamLinkHandler(_mockMapper.Object, _mockRepository.Object);

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

            _mockMapper.Setup(x => x.Map<TeamMemberLink>(It.IsAny<TeamMemberLinkDTO>()))
                .Returns(testTeamMemberLink);
            _mockMapper.Setup(x => x.Map<TeamMemberLinkDTO>(It.IsAny<TeamMemberLink>()))
                .Returns(GetTeamMemberLinkDTO());

            _mockRepository.Setup(x => x.TeamLinkRepository.Create(It.Is<TeamMemberLink>(y => y.Id == testTeamMemberLink.Id)))
                .Returns(testTeamMemberLink);

            _mockRepository.Setup(x => x.SaveChangesAsync())
                .ReturnsAsync(1);

            var handler = new CreateTeamLinkHandler(_mockMapper.Object, _mockRepository.Object);

            //Act
            var result = await handler.Handle(new CreateTeamLinkQuery(GetTeamMemberLinkDTO()), CancellationToken.None);

            //Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task ShouldThrowExeption_WhenSaveChangesIsNotSuccessful()
        {
            const string expectedErrorMessage = "Failed to create a team";
            //Arrange
            var testTeamMemberLink = GetTeamMemberLink();

            _mockMapper.Setup(x => x.Map<TeamMemberLink>(It.IsAny<TeamMemberLinkDTO>()))
                .Returns(testTeamMemberLink);

            _mockRepository.Setup(x => x.TeamLinkRepository.Create(It.Is<TeamMemberLink>(y => y.Id == testTeamMemberLink.Id)))
                .Returns(testTeamMemberLink);

            _mockRepository.Setup(x => x.SaveChanges())
                .Throws(new Exception(expectedErrorMessage));

            var handler = new CreateTeamLinkHandler(_mockMapper.Object, _mockRepository.Object);

            //Act
            var result = await handler.Handle(new CreateTeamLinkQuery(null), CancellationToken.None);

            //Assert
            Assert.Equal(expectedErrorMessage, result.Errors.First().Message);

            _mockRepository.Verify(x => x.StreetcodeRepository.GetAllAsync(It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), null), Times.Never);
        }

        [Fact]
        public async Task ShouldThrowExeption_WhenCreateNotSuccessful()
        {
            const string expectedErrorMessage = "Cannot create team link";
            //Arrange
            var testTeamMemberLink = GetTeamMemberLink();

            _mockMapper.Setup(x => x.Map<TeamMemberLink>(It.IsAny<TeamMemberLinkDTO>()))
                .Returns(testTeamMemberLink);

            //The setup of the 'Map' method returned null, causing an error.
            _mockRepository.Setup(x => x.TeamLinkRepository.Create(It.Is<TeamMemberLink>(y => y.Id == testTeamMemberLink.Id)))
                .Returns((TeamMemberLink)null);

            var handler = new CreateTeamLinkHandler(_mockMapper.Object, _mockRepository.Object);

            //Act
            var result = await handler.Handle(new CreateTeamLinkQuery(null), CancellationToken.None);

            //Assert
            Assert.Equal(expectedErrorMessage, result.Errors.First().Message);

            _mockRepository.Verify(x => x.StreetcodeRepository.GetAllAsync(It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), null), Times.Never);
        }

        [Fact]
        public async Task ShouldThrowExeption_WhenMapNotSuccessful()
        {
            const string expectedErrorMessage = "Cannot convert null to team link";
            //Arrange
            //The setup of the 'Map' method returned null, causing an error.
            _mockMapper.Setup(x => x.Map<TeamMemberLink>(It.IsAny<TeamMemberLinkDTO>()))
                .Returns((TeamMemberLink)null);

            var handler = new CreateTeamLinkHandler(_mockMapper.Object, _mockRepository.Object);

            //Act
            var result = await handler.Handle(new CreateTeamLinkQuery(null), CancellationToken.None);

            //Assert
            Assert.Equal(expectedErrorMessage, result.Errors.First().Message);

            _mockRepository.Verify(x => x.StreetcodeRepository.GetAllAsync(It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), null), Times.Never);
        }

        private static TeamMemberLink GetTeamMemberLink()
        {
            return new TeamMemberLink()
            {
                Id = 0
            };
        }

        private static TeamMemberLinkDTO GetTeamMemberLinkDTO()
        {
            return new TeamMemberLinkDTO();
        }
    }
}
