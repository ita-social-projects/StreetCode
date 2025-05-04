using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Team.GetNamePosition;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Team.TickerString
{
    public class GetTickerStringHandlerTest
    {
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<ILoggerService> _mockLogger;

        public GetTickerStringHandlerTest()
        {
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockLogger = new Mock<ILoggerService>();
        }

        [Fact]
        public async Task ShouldReturnSuccess()
        {
            // Arrange
            SetupGetAllAsync(GetPositionsList());

            var handler = new GetTickerStringHandler(_mockRepository.Object, _mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetTickerStringQuery(), CancellationToken.None);

            // Assert
            Assert.Multiple(
                 () => Assert.NotNull(result),
                 () => Assert.True(result.IsSuccess));
        }

        [Fact]
        public async Task Handle_EmptyPositionsList_ReturnsEmptyString()
        {
            // Arrange
            SetupGetAllAsync(new List<Positions>());
            var handler = new GetTickerStringHandler(_mockRepository.Object, _mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetTickerStringQuery(), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.True(result.IsSuccess),
                () => Assert.Empty(result.Value));
        }

        private static IEnumerable<Positions> GetPositionsList()
        {
            var partners = new List<Positions>()
            {
                new ()
                {
                    Id = 1,
                    TeamMembers = new List<TeamMember>
                    {
                        new () { Id = 2 },
                    },
                },
                new () { Id = 2 },
            };
            return partners;
        }

        private void SetupGetAllAsync(IEnumerable<Positions> positions)
        {
            _mockRepository
                .Setup(x => x.PositionRepository
                    .GetAllAsync(
                        null,
                        It.IsAny<Func<IQueryable<Positions>,
                        IIncludableQueryable<Positions, object>>>()))
                .ReturnsAsync(positions);
        }
    }
}
