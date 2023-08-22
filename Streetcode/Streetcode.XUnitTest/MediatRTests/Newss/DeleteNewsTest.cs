using Moq;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Newss.Delete;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Newss
{
    public class DeleteNewsTest
    {
        private Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<ILoggerService> _mockLogger;

        public DeleteNewsTest()
        {
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockLogger = new Mock<ILoggerService>();
        }

        [Fact]
        public async Task ShouldDeleteSuccessfully()
        {
            // Arrange
            var testNews = GetNews();
            SetupMockRepositoryGetFirstOrDefault(testNews);
            SetupMockRepositorySaveChangesReturns(1);

            var handler = new DeleteNewsHandler(_mockRepository.Object, _mockLogger.Object);

            // Act
            var result = await handler.Handle(new DeleteNewsCommand(testNews.Id), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.True(result.IsSuccess)
            );

            _mockRepository.Verify(x => x.NewsRepository.Delete(It.Is<DAL.Entities.News.News>(x => x.Id == testNews.Id)), Times.Once);
            _mockRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowException_IdNotExisting()
        {
            // Arrange
            var testNews = GetNews();
            var expectedError = $"No news found by entered Id - {testNews.Id}";
            SetupMockRepositoryGetFirstOrDefault(null);

            var handler = new DeleteNewsHandler(_mockRepository.Object, _mockLogger.Object);

            // Act
            var result = await handler.Handle(new DeleteNewsCommand(testNews.Id), CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors.First().Message);
            _mockRepository.Verify(x => x.NewsRepository.Delete(It.IsAny<DAL.Entities.News.News>()), Times.Never);
        }

        [Fact]
        public async Task ShouldThrowException_SaveChangesAsyncIsNotSuccessful()
        {
            // Arrange
            var testNews = GetNews();
            var expectedError = "Failed to delete news";
            SetupMockRepositoryGetFirstOrDefault(testNews);
            SetupMockRepositorySaveChangesException(expectedError);

            var handler = new DeleteNewsHandler(_mockRepository.Object, _mockLogger.Object);

            // Act
            var result = await handler.Handle(new DeleteNewsCommand(testNews.Id), CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors.First().Message);
        }

        private void SetupMockRepositoryGetFirstOrDefault(DAL.Entities.News.News? news)
        {
            _mockRepository.Setup(x => x.NewsRepository.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<DAL.Entities.News.News, bool>>>(), null))
                .ReturnsAsync(news);
        }
        private void SetupMockRepositorySaveChangesException(string expectedError)
        {
            _mockRepository.Setup(x => x.SaveChanges()).Throws(new Exception(expectedError));
        }
        private void SetupMockRepositorySaveChangesReturns(int number)
        {
            _mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(number);
        }

        private static DAL.Entities.News.News GetNews()
        {
            return new DAL.Entities.News.News()
            {
                Id = 1
            };
        }
    }
}
