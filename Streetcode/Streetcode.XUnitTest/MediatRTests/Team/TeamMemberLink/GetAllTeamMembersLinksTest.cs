using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.MediatR.Team.TeamMembersLinks.GetAll;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Team.TeamLink
{
    public class GetAllTeamMembersLinksTest
    {
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;

        public GetAllTeamMembersLinksTest()
        {
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_WhenTypeIsCorrect()
        {
            //Arrange
            SetupMapMethod(GetListTeamMemberLinkDTO());
            SetupGetAllAsyncMethod(GetTeamMemberLinksList());

            var handler = new GetAllTeamLinkHandler(_mockRepository.Object, _mockMapper.Object);

            //Act
            var result = await handler.Handle(new GetAllTeamLinkQuery(), CancellationToken.None);

            //Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.IsType<List<TeamMemberLinkDTO>>(result.ValueOrDefault)
            );
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_WhenCountMatch()
        {
            //Arrange
            SetupMapMethod(GetListTeamMemberLinkDTO());
            SetupGetAllAsyncMethod(GetTeamMemberLinksList());

            var handler = new GetAllTeamLinkHandler(_mockRepository.Object, _mockMapper.Object);

            //Act
            var result = await handler.Handle(new GetAllTeamLinkQuery(), CancellationToken.None);

            //Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.Equal(GetTeamMemberLinksList().Count(), result.Value.Count())
            );
        }

        [Fact]
        public async Task ShouldThrowExeption_WhenIdNotExist()
        {
            //Arrange
            const string expectedError = "Cannot find any team links";
            SetupMapMethod(GetTeamMemberLinksListWithNotExistingId());

            var handler = new GetAllTeamLinkHandler(_mockRepository.Object, _mockMapper.Object);

            //Act
            var result = await handler.Handle(new GetAllTeamLinkQuery(), CancellationToken.None);

            //Assert
            Assert.Equal(expectedError, result.Errors.First().Message);

            _mockMapper.Verify(x => x.Map<IEnumerable<TeamMemberLinkDTO>>(It.IsAny<IEnumerable<TeamMemberLink>>()), Times.Never);
        }

        private void SetupMapMethod(IEnumerable<TeamMemberLinkDTO> teamMemberLinksDTO)
        {
            _mockMapper.Setup(x => x.Map<IEnumerable<TeamMemberLinkDTO>>(It.IsAny<IEnumerable<TeamMemberLink>>()))
                .Returns(teamMemberLinksDTO);
        }

        private void SetupMapMethod(IEnumerable<TeamMemberLink> teamMemberLinks)
        {
            _mockRepository.Setup(x => x.TeamLinkRepository.GetAllAsync(
                null,
                It.IsAny<Func<IQueryable<TeamMemberLink>, IIncludableQueryable<TeamMemberLink, object>>>()))
                .ReturnsAsync(teamMemberLinks);
        }

        private void SetupGetAllAsyncMethod(IEnumerable<TeamMemberLink> teamMemberLinks)
        {
            _mockRepository.Setup(x => x.TeamLinkRepository.GetAllAsync(
                null,
                It.IsAny<Func<IQueryable<TeamMemberLink>, IIncludableQueryable<TeamMemberLink, object>>>()))
                .ReturnsAsync(teamMemberLinks);
        }

        private static IEnumerable<TeamMemberLink> GetTeamMemberLinksList()
        {
            var partners = new List<TeamMemberLink>{
                new TeamMemberLink
                {
                    Id = 1
                },
                new TeamMemberLink
                {
                    Id = 2
                }
            };
            return partners;
        }

        private static List<TeamMemberLink> GetTeamMemberLinksListWithNotExistingId()
        {
            return null;
        }

        private static List<TeamMemberLinkDTO> GetListTeamMemberLinkDTO()
        {
            var partnersDTO = new List<TeamMemberLinkDTO>{
                new TeamMemberLinkDTO
                {
                    Id = 1
                },
                new TeamMemberLinkDTO
                {
                    Id = 2,
                }
            };
            return partnersDTO;
        }
    }
}
