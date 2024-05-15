using System.Linq.Expressions;
using Moq;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Timeline.HistoricalContext.Delete;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Timeline.HistoricalContextTests
{
    public class DeleteHistoricalContextTest
    {
        private Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<ILoggerService> _mockLogger;

        public DeleteHistoricalContextTest()
        {
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockLogger = new Mock<ILoggerService>();
        }

        [Fact]
        public async Task ShouldDeleteSuccessfully()
        {
            // Arrange
            var testContexts = DeleteContext();
            SetupMockRepositoryGetFirstOrDefault(testContexts);
            SetupMockRepositorySaveChangesReturns(1);

            var handler = new DeleteHistoricalContextHandler(_mockRepository.Object, _mockLogger.Object);

            // Act
            var result = await handler.Handle(new DeleteHistoricalContextCommand(testContexts.Id),
                CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.True(result.IsSuccess)
            );

            _mockRepository.Verify(
                x => x.HistoricalContextRepository.Delete(It.Is<HistoricalContext>(x => x.Id == testContexts.Id)),
                Times.Once);
            _mockRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowException_IdNotExisting()
        {
            // Arrange
            var testContexts = DeleteContext();
            var expectedError = $"No context found by entered Id - {testContexts.Id}";
            SetupMockRepositoryGetFirstOrDefault(null);

            var handler = new DeleteHistoricalContextHandler(_mockRepository.Object, _mockLogger.Object);

            // Act
            var result = await handler.Handle(new DeleteHistoricalContextCommand(testContexts.Id), CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors.First().Message);
            _mockRepository.Verify(x => x.HistoricalContextRepository.Delete(It.IsAny<HistoricalContext>()), Times.Never);
        }

        private void SetupMockRepositoryGetFirstOrDefault(HistoricalContext context)
        {
            _mockRepository.Setup(x => x.HistoricalContextRepository.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<HistoricalContext, bool>>>(), null))
                .ReturnsAsync(context);
        }

        private void SetupMockRepositorySaveChangesException(string expectedError)
        {
            _mockRepository.Setup(x => x.SaveChanges()).Throws(new Exception(expectedError));
        }

        private void SetupMockRepositorySaveChangesReturns(int number)
        {
            _mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(number);
        }

        private static HistoricalContext DeleteContext()
        {
            return new HistoricalContext
            {
                Id = 1
            };
        }
    }
}