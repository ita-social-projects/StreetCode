using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Newss.Create;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.News
{
    public class CreateNewsTest
    {
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<FailedToCreateSharedResource>> _mockLocalizerFail;
        private readonly Mock<IStringLocalizer<CannotConvertNullSharedResource>> _mockLocalizerConvertNull;

        public CreateNewsTest()
        {
            this._mockRepository = new Mock<IRepositoryWrapper>();
            this._mockMapper = new Mock<IMapper>();
            this._mockLogger = new Mock<ILoggerService>();
            this._mockLocalizerFail = new Mock<IStringLocalizer<FailedToCreateSharedResource>>();
            this._mockLocalizerConvertNull = new Mock<IStringLocalizer<CannotConvertNullSharedResource>>();
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_TypeIsCorrect()
        {
            // Arrange
            var testNews = GetNews(1, "test");
            this.SetupMockMapping(testNews);
            this.SetupMockRepositoryCreate(testNews);
            this.SetupMockRepositorySaveChangesReturns(1);

            var handler = new CreateNewsHandler(this._mockMapper.Object, this._mockRepository.Object, this._mockLogger.Object, this._mockLocalizerFail.Object, this._mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new CreateNewsCommand(GetNewsCreateDTO()), CancellationToken.None);

            // Assert
            Assert.IsType<NewsDTO>(result.Value);
        }

        [Fact]
        public async Task ShouldThrowException_TryMapNullRequest()
        {
            // Arrange
            var expectedError = "Cannot convert null to news";
            this._mockLocalizerConvertNull.Setup(x => x["CannotConvertNullToNews"])
            .Returns(new LocalizedString("CannotConvertNullToNews", expectedError));
            this.SetupMapperWithNullNews();

            var handler = new CreateNewsHandler(this._mockMapper.Object, this._mockRepository.Object, this._mockLogger.Object, this._mockLocalizerFail.Object, this._mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new CreateNewsCommand(GetNewsDTOWithNotExistId() !), CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(expectedError, result.Errors[0].Message);
        }

        [Fact]
        public async Task ShouldReturnFail_ImageIdIsZero()
        {
            // Arrange
            string expectedErrorMessage = "Invalid ImageId Value";
            var testNews = GetNews();
            this.SetupMockMapping(testNews);
            var handler = new CreateNewsHandler(this._mockMapper.Object, this._mockRepository.Object, this._mockLogger.Object, this._mockLocalizerFail.Object, this._mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new CreateNewsCommand(new NewsCreateDTO()), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.True(result.IsFailed),
                () => Assert.Equal(expectedErrorMessage, result.Errors[0].Message));
        }

        [Fact]
        public async Task ShouldReturnFail_UrlIsInvalid()
        {
            // Arrange
            string expectedErrorMessage = "Url Is Invalid";
            var testNews = GetNews(1);
            this.SetupMockMapping(testNews);
            var handler = new CreateNewsHandler(this._mockMapper.Object, this._mockRepository.Object, this._mockLogger.Object, this._mockLocalizerFail.Object, this._mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new CreateNewsCommand(new NewsCreateDTO()), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.True(result.IsFailed),
                () => Assert.Equal(expectedErrorMessage, result.Errors[0].Message));
        }

        [Fact]
        public async Task ShouldReturnFail_CreationDateIsRequired()
        {
            // Arrange
            string expectedErrorMessage = "CreationDate field is required";
            var testNews = GetNewsWithDefaultCreationDate();
            this.SetupMockMapping(testNews);
            var handler = new CreateNewsHandler(this._mockMapper.Object, this._mockRepository.Object, this._mockLogger.Object, this._mockLocalizerFail.Object, this._mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new CreateNewsCommand(new NewsCreateDTO()), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.True(result.IsFailed),
                () => Assert.Equal(expectedErrorMessage, result.Errors[0].Message));
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_WhenNewsAdded()
        {
            // Arrange
            var testNews = GetNews(1, "test");
            this.SetupMockMapping(testNews);
            this.SetupMockRepositoryCreate(testNews);
            this.SetupMockRepositorySaveChangesReturns(1);

            var handler = new CreateNewsHandler(this._mockMapper.Object, this._mockRepository.Object, this._mockLogger.Object, this._mockLocalizerFail.Object, this._mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new CreateNewsCommand(GetNewsCreateDTO()), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task ShouldThrowException_SaveChangesIsNotSuccessful()
        {
            // Arrange
            var testNews = GetNews(1, "test");
            var expectedError = "Failed to create a news";
            this._mockLocalizerFail.Setup(x => x["FailedToCreateNews"]).Returns(new LocalizedString("FailedToCreateNews", expectedError));
            this.SetupMockMapping(testNews);
            this.SetupMockRepositoryCreate(testNews);
            this.SetupMockRepositorySaveChangesException(expectedError);

            var handler = new CreateNewsHandler(this._mockMapper.Object, this._mockRepository.Object, this._mockLogger.Object, this._mockLocalizerFail.Object, this._mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new CreateNewsCommand(GetNewsCreateDTO()), CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors[0].Message);
        }

        [Fact]
        public async Task ShouldReturnFail_NewsWithSameTitleExists()
        {
            // Arrange
            var testNews = GetNews(1, "test");
            var expectedError = "A news with the same title already exists.";
            this.SetupMockMapping(testNews);
            this.SetupMockRepositoryGetFirstOrDefaultAsyncWithExistingTitle(testNews.Title);
            var handler = new CreateNewsHandler(this._mockMapper.Object, this._mockRepository.Object, this._mockLogger.Object, this._mockLocalizerFail.Object, this._mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new CreateNewsCommand(GetNewsCreateDTO()), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.True(result.IsFailed),
                () => Assert.Equal(expectedError, result.Errors[0].Message));
        }

        [Fact]
        public async Task ShouldThrowException_NewsWithSameTextExists()
        {
            // Arrange
            var testNews = GetNews(1, "test");
            var expectedError = "A news with the same text already exists.";
            this.SetupMockMapping(testNews);
            this.SetupMockRepositoryGetSingleOrDefaultAsyncWithExistingText(testNews.Text);
            var handler = new CreateNewsHandler(this._mockMapper.Object, this._mockRepository.Object, this._mockLogger.Object, this._mockLocalizerFail.Object, this._mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new CreateNewsCommand(GetNewsCreateDTO()), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.True(result.IsFailed),
                () => Assert.Equal(expectedError, result.Errors[0].Message));
        }

        private static DAL.Entities.News.News GetNews(int imageId = 0, string url = "/test")
        {
            return new DAL.Entities.News.News()
            {
                Id = 1,
                ImageId = imageId,
                Title = "Title",
                Text = "test",
                URL = url,
                CreationDate = new DateTime(2015, 12, 25, 0, 0, 0, DateTimeKind.Utc),
            };
        }

        private static NewsCreateDTO GetNewsCreateDTO()
        {
            return new NewsCreateDTO()
            {
                ImageId = 1,
                Title = "Title",
                Text = "test",
                URL = "test",
                CreationDate = new DateTime(2015, 12, 25, 0, 0, 0, DateTimeKind.Utc),
            };
        }

        private static DAL.Entities.News.News GetNewsWithDefaultCreationDate()
        {
            return new DAL.Entities.News.News()
            {
                ImageId = 1,
                Title = "Title",
                Text = "test",
                URL = "test",
            };
        }

        private static NewsDTO GetNewsDTO()
        {
            return new NewsDTO()
            {
                Id = 1,
                ImageId = 1,
                Title = "Title",
                Text = "test",
                URL = "test",
                CreationDate = new DateTime(2015, 12, 25, 0, 0, 0, DateTimeKind.Utc),
            };
        }

        private static DAL.Entities.News.News? GetNewsWithNotExistId() => null;

        private static NewsCreateDTO? GetNewsDTOWithNotExistId() => null;

        private void SetupMockMapping(DAL.Entities.News.News testNews)
        {
            this._mockMapper.Setup(x => x.Map<DAL.Entities.News.News>(It.IsAny<NewsCreateDTO>()))
                .Returns(testNews);
            this._mockMapper.Setup(x => x.Map<NewsDTO>(It.IsAny<DAL.Entities.News.News>()))
                .Returns(GetNewsDTO());
        }

        private void SetupMapperWithNullNews()
        {
            this._mockMapper.Setup(x => x.Map<DAL.Entities.News.News?>(It.IsAny<NewsDTO>()))
                .Returns(GetNewsWithNotExistId());
        }

        private void SetupMockRepositoryCreate(DAL.Entities.News.News testNews)
        {
            this._mockRepository.Setup(x => x.NewsRepository.Create(It.Is<DAL.Entities.News.News>(y => y.Id == testNews.Id)))
                .Returns(testNews);
        }

        private void SetupMockRepositorySaveChangesException(string expectedError)
        {
            this._mockRepository.Setup(x => x.SaveChanges()).Throws(new Exception(expectedError));
        }

        private void SetupMockRepositoryGetFirstOrDefaultAsyncWithExistingTitle(string title)
        {
            this._mockRepository.Setup(x => x.NewsRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<DAL.Entities.News.News, bool>>>(),
                It.IsAny<Func<IQueryable<DAL.Entities.News.News>, IIncludableQueryable<DAL.Entities.News.News, object>>>()))
            .ReturnsAsync(() =>
            {
                 var newsList = new List<DAL.Entities.News.News>
                 {
                     new DAL.Entities.News.News { Title = title },
                 };
                 return newsList.FirstOrDefault();
            });
        }

        private void SetupMockRepositoryGetSingleOrDefaultAsyncWithExistingText(string text)
        {
            this._mockRepository.Setup(x => x.NewsRepository.GetSingleOrDefaultAsync(
                It.IsAny<Expression<Func<DAL.Entities.News.News, bool>>>(),
                It.IsAny<Func<IQueryable<DAL.Entities.News.News>, IIncludableQueryable<DAL.Entities.News.News, object>>>()))
            .ReturnsAsync(() =>
            {
                 var newsList = new List<DAL.Entities.News.News>
                 {
                     new DAL.Entities.News.News { Text = text },
                 };
                 return newsList.FirstOrDefault();
            });
        }

        private void SetupMockRepositorySaveChangesReturns(int number)
        {
            this._mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(number);
        }
    }
}
