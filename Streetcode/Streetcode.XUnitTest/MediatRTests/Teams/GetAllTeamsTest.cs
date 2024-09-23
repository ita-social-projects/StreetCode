using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.MediatR.Team.GetAll;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.SharedResource;
using Streetcode.BLL.Interfaces.Logging;
using System.Linq.Expressions;
using Streetcode.DAL.Helpers;
using Streetcode.BLL.MediatR.Team.Position.GetAll;

namespace Streetcode.XUnitTest.MediatRTests.Teams
{
    public class GetAllTeamTest
    {
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizerCannotFind;

        public GetAllTeamTest()
        {
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerService>();
            _mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_CorrectType()
        {
            // Arrange
            var teamList = GetTeamList();
            var teamDTOList = GetListTeamDTO();

            SetupPaginatedRepository(teamList);
            SetupMapper(teamDTOList);

            var handler = new GetAllTeamHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new GetAllTeamQuery(), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.NotNull(result.Value),
                () => Assert.IsType<List<TeamMemberDTO>>(result.ValueOrDefault.TeamMembers)
            );
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_CountMatch()
        {
            // Arrange
            var teamList = GetTeamList();
            var teamDTOList = GetListTeamDTO();

            SetupPaginatedRepository(teamList);
            SetupMapper(teamDTOList);

            var handler = new GetAllTeamHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new GetAllTeamQuery(), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.NotNull(result.Value),
                () => Assert.Equal(GetTeamList().Count(), result.Value.TeamMembers.Count())
            );
        }

        [Fact]
        public async Task Handler_Returns_Correct_PageSize()
        {
            //Arrange
            ushort pageSize = 3;
            SetupPaginatedRepository(GetTeamList().Take(pageSize));
            SetupMapper(GetListTeamDTO().Take(pageSize).ToList());

            var handler = new GetAllTeamHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);

            //Act
            var result = await handler.Handle(new GetAllTeamQuery(page: 1, pageSize: pageSize), CancellationToken.None);

            //Assert
            Assert.Multiple(
                () => Assert.IsType<List<TeamMemberDTO>>(result.Value.TeamMembers),
                () => Assert.Equal(pageSize, result.Value.TeamMembers.Count()));
        }

        private void SetupPaginatedRepository(IEnumerable<TeamMember> returnList)
        {
            _mockRepository.Setup(repo => repo.TeamRepository.GetAllPaginated(
                It.IsAny<ushort?>(),
                It.IsAny<ushort?>(),
                It.IsAny<Expression<Func<TeamMember, TeamMember>>?>(),
                It.IsAny<Expression<Func<TeamMember, bool>>?>(),
                It.IsAny<Func<IQueryable<TeamMember>, IIncludableQueryable<TeamMember, object>>?>(),
                It.IsAny<Expression<Func<TeamMember, object>>?>(),
                It.IsAny<Expression<Func<TeamMember, object>>?>()))
            .Returns(PaginationResponse<TeamMember>.Create(returnList.AsQueryable()));
        }

        private void SetupMapper(IEnumerable<TeamMemberDTO> teamDTOList)
        {
            _mockMapper.Setup(x => x.Map<IEnumerable<TeamMemberDTO>>(It.IsAny<IEnumerable<TeamMember>>()))
                .Returns(teamDTOList);
        }

        private static List<TeamMember> GetTeamList()
        {
            return new List<TeamMember>
            {
                new TeamMember
                {
                    Id = 1,
                },
                new TeamMember
                {
                    Id = 2,
                },
                new TeamMember
                {
                    Id = 3,
                },
                new TeamMember
                {
                    Id = 4,
                },
                new TeamMember
                {
                    Id = 5,
                },
            };
        }

        private static List<TeamMemberDTO> GetListTeamDTO()
        {
            return new List<TeamMemberDTO>
            {
                new TeamMemberDTO
                {
                    Id = 1,
                },
                new TeamMemberDTO
                {
                    Id = 2,
                },
                new TeamMemberDTO
                {
                    Id = 3,
                },
                new TeamMemberDTO
                {
                    Id = 4,
                },
                new TeamMemberDTO
                {
                    Id = 5,
                },
            };
        }
    }
}
