using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Team.GetById;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Team
{
    public class GetTeamByIdTest
    {
        private readonly Mock<IRepositoryWrapper> mockRepository;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizerCannotFind;

        public GetTeamByIdTest()
        {
            this.mockMapper = new Mock<IMapper>();
            this.mockRepository = new Mock<IRepositoryWrapper>();
            this.mockLogger = new Mock<ILoggerService>();
            this._mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_ExistingId()
        {
            // Arrange
            var testTeam = GetTeam();
            var teamDTO = GetTeamDTO();

            this.SetupGetTeamById(testTeam);
            this.SetupMapTeamMember(teamDTO);

            var handler = new GetByIdTeamHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this._mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new GetByIdTeamQuery(testTeam.Id), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.True(result.IsSuccess),
                () => Assert.Equal(result.Value.Id, testTeam.Id));
        }

        [Fact]
        public async Task ShouldReturnErrorResponse_NotExistingId()
        {
            // Arrange
            var testTeam = GetTeam();
            var expectedError = $"Cannot find any team with corresponding id: {testTeam.Id}";
            this._mockLocalizerCannotFind.Setup(x => x[It.IsAny<string>(), It.IsAny<object>()])
               .Returns((string key, object[] args) =>
               {
                   if (args != null && args.Length > 0 && args[0] is int)
                   {
                       return new LocalizedString(key, expectedError);
                   }

                   return new LocalizedString(key, "Cannot find any team with unknown Id");
               });

            var teamDTOWithNotExistingId = GetTeamDTOWithNotExistingId();

            this.SetupGetTeamById(GetTeamWithNotExistingId());
            this.SetupMapTeamMember(teamDTOWithNotExistingId);

            var handler = new GetByIdTeamHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this._mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new GetByIdTeamQuery(testTeam.Id), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.True(result.IsFailed),
                () => Assert.Equal(expectedError, result.Errors[0].Message));
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_CorrectType()
        {
            // Arrange
            var testTeam = GetTeam();
            var teamDTO = GetTeamDTO();
            this.SetupGetTeamById(testTeam);
            this.SetupMapTeamMember(teamDTO);

            var handler = new GetByIdTeamHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this._mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new GetByIdTeamQuery(testTeam.Id), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotNull(result.ValueOrDefault),
                () => Assert.IsType<TeamMemberDTO>(result.ValueOrDefault));
        }

        private static TeamMember GetTeam()
        {
            return new TeamMember
            {
                Id = 1,
            };
        }

        private static TeamMember? GetTeamWithNotExistingId()
        {
            return null;
        }

        private static TeamMemberDTO GetTeamDTO()
        {
            return new TeamMemberDTO
            {
                Id = 1,
            };
        }

        private static TeamMemberDTO? GetTeamDTOWithNotExistingId()
        {
            return null;
        }

        private void SetupGetTeamById(TeamMember? testTeam)
        {
            this.mockRepository.Setup(x => x.TeamRepository
                .GetSingleOrDefaultAsync(
                    It.IsAny<Expression<Func<TeamMember, bool>>>(),
                    It.IsAny<Func<IQueryable<TeamMember>, IIncludableQueryable<TeamMember, object>>>()))
                .ReturnsAsync(testTeam);
        }

        private void SetupMapTeamMember(TeamMemberDTO? teamMemberDTO)
        {
            this.mockMapper.Setup(x => x
                .Map<TeamMemberDTO?>(It.IsAny<TeamMember>()))
                .Returns(teamMemberDTO);
        }
    }
}
