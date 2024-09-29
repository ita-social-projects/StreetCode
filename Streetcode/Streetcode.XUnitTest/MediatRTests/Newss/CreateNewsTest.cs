using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Newss.Create;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.News;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Streetcode.DAL.Entities.Media.Images;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Newss
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
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerService>();
            _mockLocalizerFail = new Mock<IStringLocalizer<FailedToCreateSharedResource>>();
            _mockLocalizerConvertNull = new Mock<IStringLocalizer<CannotConvertNullSharedResource>>();
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_TypeIsCorrect()
        {
            // Arrange
            var testNews = GetNews(1, "test");
            SetupMockMapping(testNews);
            SetupMockRepositoryCreate(testNews);
            SetupMockRepositorySaveChangesReturns(1);
            SetupMockImageRepositoryGetFirstOrDefaultAsync();

            var handler = new CreateNewsHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object, _mockLocalizerFail.Object, _mockLocalizerConvertNull.Object);

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
            _mockLocalizerConvertNull.Setup(x => x["CannotConvertNullToNews"])
            .Returns(new LocalizedString("CannotConvertNullToNews", expectedError));
            var testNews = GetNews(1, "test");
            SetupMapperWithNullNews();

            var handler = new CreateNewsHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object, _mockLocalizerFail.Object, _mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new CreateNewsCommand(GetNewsDTOWithNotExistId()), CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(expectedError, result.Errors.First().Message);
        }

        [Fact]
        public async Task ShouldReturnFail_ImageIdIsZero()
        {
            // Arrange
            string expectedErrorMessage = "Invalid ImageId Value";
            var testNews = GetNews();
            SetupMockMapping(testNews);
            var handler = new CreateNewsHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object, _mockLocalizerFail.Object, _mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new CreateNewsCommand(new NewsCreateDTO()), CancellationToken.None);
            // Assert
            Assert.Multiple(
                () => Assert.True(result.IsFailed),
                () => Assert.Equal(expectedErrorMessage, result.Errors.First().Message));
        }

        [Fact]
        public async Task ShouldReturnFail_UrlIsInvalid()
        {
            // Arrange
            string expectedErrorMessage = "Url Is Invalid";
            var testNews = GetNews(1);
            SetupMockMapping(testNews);
            SetupMockImageRepositoryGetFirstOrDefaultAsync();
            var handler = new CreateNewsHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object, _mockLocalizerFail.Object, _mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new CreateNewsCommand(new NewsCreateDTO()), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.True(result.IsFailed),
                () => Assert.Equal(expectedErrorMessage, result.Errors.First().Message));
        }

        [Fact]
        public async Task ShouldReturnFail_CreationDateIsRequired()
        {
            // Arrange
            string expectedErrorMessage = "CreationDate field is required";
            var testNews = GetNewsWithDefaultCreationDate();
            SetupMockMapping(testNews);
            SetupMockImageRepositoryGetFirstOrDefaultAsync();
            var handler = new CreateNewsHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object, _mockLocalizerFail.Object, _mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new CreateNewsCommand(new NewsCreateDTO()), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.True(result.IsFailed),
                () => Assert.Equal(expectedErrorMessage, result.Errors.First().Message));
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_WhenNewsAdded()
        {
            // Arrange
            var testNews = GetNews(1, "test");
            SetupMockMapping(testNews);
            SetupMockRepositoryCreate(testNews);
            SetupMockRepositorySaveChangesReturns(1);
            SetupMockImageRepositoryGetFirstOrDefaultAsync();

            var handler = new CreateNewsHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object, _mockLocalizerFail.Object, _mockLocalizerConvertNull.Object);

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
            _mockLocalizerFail.Setup(x => x["FailedToCreateNews"]).Returns(new LocalizedString("FailedToCreateNews", expectedError));
            SetupMockMapping(testNews);
            SetupMockRepositoryCreate(testNews);
            SetupMockRepositorySaveChangesException(expectedError);
            SetupMockImageRepositoryGetFirstOrDefaultAsync();

            var handler = new CreateNewsHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object, _mockLocalizerFail.Object, _mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new CreateNewsCommand(GetNewsCreateDTO()), CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors.First().Message);
        }

        [Fact]
        public async Task ShouldReturnFail_NewsWithSameTitleExists()
        {
            // Arrange
            var testNews = GetNews(1, "test");
            var expectedError = "A news with the same title already exists.";
            SetupMockMapping(testNews);
            SetupMockRepositoryGetFirstOrDefaultAsyncWithExistingTitle(testNews.Title);
            SetupMockImageRepositoryGetFirstOrDefaultAsync();
            var handler = new CreateNewsHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object, _mockLocalizerFail.Object, _mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new CreateNewsCommand(GetNewsCreateDTO()), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.True(result.IsFailed),
                () => Assert.Equal(expectedError, result.Errors.First().Message));
        }

        [Fact]
        public async Task ShouldThrowException_NewsWithSameTextExists()
        {
            // Arrange
            var testNews = GetNews(1, "test");
            var expectedError = "A news with the same text already exists.";
            SetupMockMapping(testNews);
            SetupMockRepositoryGetSingleOrDefaultAsyncWithExistingText(testNews.Text);
            SetupMockImageRepositoryGetFirstOrDefaultAsync();
            var handler = new CreateNewsHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object, _mockLocalizerFail.Object, _mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new CreateNewsCommand(GetNewsCreateDTO()), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.True(result.IsFailed),
                () => Assert.Equal(expectedError, result.Errors.First().Message));
        }

        [Fact]
        public async Task ShouldReturnFail_WhenImageDoesNotExist()
        {
            // Arrange
            var testNewsCreateDTO = GetNewsCreateDTO();
            var testNews = GetNews(testNewsCreateDTO.ImageId, testNewsCreateDTO.URL);

            SetupMockMapping(testNews);
            SetupMockImageRepositoryGetFirstOrDefaultAsyncNonExistentImage();

            var handler = new CreateNewsHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object, _mockLocalizerFail.Object, _mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new CreateNewsCommand(testNewsCreateDTO), CancellationToken.None);

            // Assert
            Assert.True(result.IsFailed);
            Assert.Equal("Image with provided ImageId does not exist", result.Errors.First().Message);
        }

        private void SetupMockMapping(News testNews)
        {
            _mockMapper.Setup(x => x.Map<News>(It.IsAny<NewsCreateDTO>()))
                .Returns(testNews);
            _mockMapper.Setup(x => x.Map<NewsDTO>(It.IsAny<News>()))
                .Returns(GetNewsDTO());
        }

        private void SetupMapperWithNullNews()
        {
            _mockMapper.Setup(x => x.Map<News>(It.IsAny<NewsDTO>()))
                .Returns(GetNewsWithNotExistId());
        }

        private static News GetNewsWithNotExistId() => null;

        private static NewsCreateDTO GetNewsDTOWithNotExistId() => null;

        private void SetupMockRepositoryCreate(News testNews)
        {
            _mockRepository.Setup(x => x.NewsRepository.Create(It.Is<News>(y => y.Id == testNews.Id)))
                .Returns(testNews);
        }

        private void SetupMockRepositorySaveChangesException(string expectedError)
        {
            _mockRepository.Setup(x => x.SaveChanges()).Throws(new Exception(expectedError));
        }

        private void SetupMockRepositoryGetFirstOrDefaultAsyncWithExistingTitle(string title)
        {
            _mockRepository.Setup(x => x.NewsRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<News, bool>>>(),
                It.IsAny<Func<IQueryable<News>, IIncludableQueryable<News, object>>>()))
                           .ReturnsAsync(() =>
                           {
                               var newsList = new List<News>
                               {
                                    new () { Title = title },
                               };

                               return newsList.FirstOrDefault();
                           });
        }

        private void SetupMockRepositoryGetSingleOrDefaultAsyncWithExistingText(string text)
        {
            _mockRepository.Setup(x => x.NewsRepository.GetSingleOrDefaultAsync(
                It.IsAny<Expression<Func<News, bool>>>(),
                It.IsAny<Func<IQueryable<News>, IIncludableQueryable<News, object>>>()))
                           .ReturnsAsync(() =>
                           {
                               var newsList = new List<News>
                               {
                                    new () { Text = text },
                               };

                               return newsList.FirstOrDefault();
                           });
        }

        private void SetupMockRepositorySaveChangesReturns(int number)
        {
            _mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(number);
        }

        private void SetupMockImageRepositoryGetFirstOrDefaultAsync()
        {
            _mockRepository.Setup(x => x.ImageRepository.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Image, bool>>>(),
                    It.IsAny<Func<IQueryable<Image>, IIncludableQueryable<Image, object>>>()))
                .ReturnsAsync(new Image { Id = 1 });
        }

        private void SetupMockImageRepositoryGetFirstOrDefaultAsyncNonExistentImage()
        {
            _mockRepository.Setup(x => x.ImageRepository.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Image, bool>>>(),
                    It.IsAny<Func<IQueryable<Image>, IIncludableQueryable<Image, object>>>()))
                .ReturnsAsync((Image)null!);
        }

        private static News GetNews(int imageId = 0, string url = "/test")
        {
            return new News()
            {
                Id = 1,
                ImageId = imageId,
                Title = "Title",
                Text = "test",
                URL = url,
                CreationDate = new DateTime(2015, 12, 25)
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
                CreationDate = new DateTime(2015, 12, 25)
            };
        }

        private static News GetNewsWithDefaultCreationDate()
        {
            return new News()
            {
                ImageId = 1,
                Title = "Title",
                Text = "test",
                URL = "test"
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
                CreationDate = new DateTime(2015, 12, 25)
            };
        }
    }
}
