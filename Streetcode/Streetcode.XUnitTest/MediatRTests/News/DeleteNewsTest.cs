using System.Linq.Expressions;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Newss.Delete;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.News
{
    public class DeleteNewsTest
    {
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<NoSharedResource>> _mockLocalizerNoShared;
        private readonly Mock<IStringLocalizer<FailedToDeleteSharedResource>> _mockLocalizerFailed;

        public DeleteNewsTest()
        {
            this._mockRepository = new Mock<IRepositoryWrapper>();
            this._mockLogger = new Mock<ILoggerService>();
            this._mockLocalizerFailed = new Mock<IStringLocalizer<FailedToDeleteSharedResource>>();
            this._mockLocalizerNoShared = new Mock<IStringLocalizer<NoSharedResource>>();
        }

        [Fact]
        public async Task ShouldDeleteSuccessfully()
        {
            // Arrange
            var testNews = GetNews();
            this.SetupMockRepositoryGetFirstOrDefault(testNews);
            this.SetupMockRepositorySaveChangesReturns(1);

            var handler = new DeleteNewsHandler(this._mockRepository.Object, this._mockLogger.Object, this._mockLocalizerNoShared.Object, this._mockLocalizerFailed.Object);

            // Act
            var result = await handler.Handle(new DeleteNewsCommand(testNews.Id), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.True(result.IsSuccess));

            this._mockRepository.Verify(x => x.NewsRepository.Delete(It.Is<DAL.Entities.News.News>(x => x.Id == testNews.Id)), Times.Once);
            this._mockRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowException_IdNotExisting()
        {
            // Arrange
            var testNews = GetNews();
            var expectedError = $"No news found by entered Id - {testNews.Id}";
            this._mockLocalizerNoShared.Setup(x => x[It.IsAny<string>(), It.IsAny<object>()]).Returns((string key, object[] args) =>
           {
               if (args != null && args.Length > 0 && args[0] is int)
               {
                   return new LocalizedString(key, $"No news found by entered Id - {testNews.Id}");
               }

               return new LocalizedString(key, "Cannot find any news with unknown id");
           });

            this.SetupMockRepositoryGetFirstOrDefault(null);

            var handler = new DeleteNewsHandler(this._mockRepository.Object, this._mockLogger.Object, this._mockLocalizerNoShared.Object, this._mockLocalizerFailed.Object);

            // Act
            var result = await handler.Handle(new DeleteNewsCommand(testNews.Id), CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors[0].Message);
            this._mockRepository.Verify(x => x.NewsRepository.Delete(It.IsAny<DAL.Entities.News.News>()), Times.Never);
        }

        [Fact]
        public async Task ShouldThrowException_SaveChangesAsyncIsNotSuccessful()
        {
            // Arrange
            var testNews = GetNews();
            var expectedError = "Failed to delete news";
            this._mockLocalizerFailed.Setup(x => x["FailedToDeleteNews"])
            .Returns(new LocalizedString("FailedToDeleteNews", expectedError));
            this.SetupMockRepositoryGetFirstOrDefault(testNews);
            this.SetupMockRepositorySaveChangesException(expectedError);

            var handler = new DeleteNewsHandler(this._mockRepository.Object, this._mockLogger.Object, this._mockLocalizerNoShared.Object, this._mockLocalizerFailed.Object);

            // Act
            var result = await handler.Handle(new DeleteNewsCommand(testNews.Id), CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors[0].Message);
        }

        private static DAL.Entities.News.News GetNews()
        {
            return new DAL.Entities.News.News()
            {
                Id = 1,
            };
        }

        private void SetupMockRepositoryGetFirstOrDefault(DAL.Entities.News.News? news)
        {
            this._mockRepository.Setup(x => x.NewsRepository.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<DAL.Entities.News.News, bool>>>(), null))
                .ReturnsAsync(news);
        }

        private void SetupMockRepositorySaveChangesException(string expectedError)
        {
            this._mockRepository.Setup(x => x.SaveChanges()).Throws(new Exception(expectedError));
        }

        private void SetupMockRepositorySaveChangesReturns(int number)
        {
            this._mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(number);
        }
    }
}
