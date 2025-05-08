using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Team.GetById;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Team
{
    public class GetTeamByIdTest
    {
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizerCannotFind;

        public GetTeamByIdTest()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockLogger = new Mock<ILoggerService>();
            _mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_ExistingId()
        {
            // Arrange
            var testTeam = GetTeam();
            var teamDto = GetTeamDto();

            SetupGetTeamById(testTeam);
            SetupMapTeamMember(teamDto);

            var handler = new GetByIdTeamHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);

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
            _mockLocalizerCannotFind.Setup(x => x[It.IsAny<string>(), It.IsAny<object>()])
               .Returns((string key, object[] args) =>
               {
                   if (args != null && args.Length > 0 && args[0] is int)
                   {
                       return new LocalizedString(key, expectedError);
                   }

                   return new LocalizedString(key, "Cannot find any team with unknown Id");
               });

            var teamDtoWithNotExistingId = GetTeamDtoWithNotExistingId();

            SetupGetTeamById(GetTeamWithNotExistingId());
            SetupMapTeamMember(teamDtoWithNotExistingId);

            var handler = new GetByIdTeamHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);

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
            var teamDto = GetTeamDto();
            SetupGetTeamById(testTeam);
            SetupMapTeamMember(teamDto);

            var handler = new GetByIdTeamHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);

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

        private static TeamMemberDTO GetTeamDto()
        {
            return new TeamMemberDTO
            {
                Id = 1,
            };
        }

        private static TeamMemberDTO? GetTeamDtoWithNotExistingId()
        {
            return null;
        }

        private void SetupGetTeamById(TeamMember? testTeam)
        {
            _mockRepository.Setup(x => x.TeamRepository
                .GetSingleOrDefaultAsync(
                    It.IsAny<Expression<Func<TeamMember, bool>>>(),
                    It.IsAny<Func<IQueryable<TeamMember>, IIncludableQueryable<TeamMember, object>>>()))
                .ReturnsAsync(testTeam);
        }

        private void SetupMapTeamMember(TeamMemberDTO? teamMemberDto)
        {
            _mockMapper.Setup(x => x
                .Map<TeamMemberDTO?>(It.IsAny<TeamMember>()))
                .Returns(teamMemberDto);
        }
    }
}
