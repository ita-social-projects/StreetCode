using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Team.GetAll;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Helpers;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Team
{
    public class GetAllTeamsTest
    {
        private readonly Mock<IRepositoryWrapper> mockRepository;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> mockLocalizerCannotFind;

        public GetAllTeamsTest()
        {
            this.mockRepository = new Mock<IRepositoryWrapper>();
            this.mockMapper = new Mock<IMapper>();
            this.mockLogger = new Mock<ILoggerService>();
            this.mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_CorrectType()
        {
            // Arrange
            var teamList = GetTeamList();
            var teamDTOList = GetListTeamDTO();

            this.SetupPaginatedRepository(teamList);
            this.SetupMapper(teamDTOList);

            var handler = new GetAllTeamHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object);

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

            this.SetupPaginatedRepository(teamList);
            this.SetupMapper(teamDTOList);

            var handler = new GetAllTeamHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object);

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
            // Arrange
            ushort pageSize = 3;
            this.SetupPaginatedRepository(GetTeamList().Take(pageSize));
            this.SetupMapper(GetListTeamDTO().Take(pageSize).ToList());

            var handler = new GetAllTeamHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new GetAllTeamQuery(page: 1, pageSize: pageSize), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<List<TeamMemberDTO>>(result.Value.TeamMembers),
                () => Assert.Equal(pageSize, result.Value.TeamMembers.Count()));
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

        private void SetupPaginatedRepository(IEnumerable<TeamMember> returnList)
        {
            this.mockRepository.Setup(repo => repo.TeamRepository.GetAllPaginated(
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
            this.mockMapper.Setup(x => x.Map<IEnumerable<TeamMemberDTO>>(It.IsAny<IEnumerable<TeamMember>>()))
                .Returns(teamDTOList);
        }
    }
}
