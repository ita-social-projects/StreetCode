using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Newss.Create;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.News
{
    public class CreateNewsTest
    {
        private readonly Mock<IRepositoryWrapper> mockRepository;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly Mock<IStringLocalizer<FailedToCreateSharedResource>> mockLocalizerFail;
        private readonly Mock<IStringLocalizer<CannotConvertNullSharedResource>> mockLocalizerConvertNull;

        public CreateNewsTest()
        {
            this.mockRepository = new Mock<IRepositoryWrapper>();
            this.mockMapper = new Mock<IMapper>();
            this.mockLogger = new Mock<ILoggerService>();
            this.mockLocalizerFail = new Mock<IStringLocalizer<FailedToCreateSharedResource>>();
            this.mockLocalizerConvertNull = new Mock<IStringLocalizer<CannotConvertNullSharedResource>>();
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_TypeIsCorrect()
        {
            // Arrange
            var testNews = GetNews(1, "test");
            this.SetupMockMapping(testNews);
            this.SetupMockRepositoryCreate(testNews);
            this.SetupMockRepositorySaveChangesReturns(1);
            this.SetupMockImageRepositoryGetFirstOrDefaultAsync();

            var handler = new CreateNewsHandler(this.mockMapper.Object, this.mockRepository.Object, this.mockLogger.Object, this.mockLocalizerFail.Object, this.mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new CreateNewsCommand(GetNewsCreateDTO()), CancellationToken.None);

            // Assert
            Assert.IsType<NewsDto>(result.Value);
        }

        [Fact]
        public async Task ShouldThrowException_TryMapNullRequest()
        {
            // Arrange
            var expectedError = "Cannot convert null to news";
            this.mockLocalizerConvertNull.Setup(x => x["CannotConvertNullToNews"])
            .Returns(new LocalizedString("CannotConvertNullToNews", expectedError));
            this.SetupMapperWithNullNews();

            var handler = new CreateNewsHandler(this.mockMapper.Object, this.mockRepository.Object, this.mockLogger.Object, this.mockLocalizerFail.Object, this.mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new CreateNewsCommand(GetNewsDTOWithNotExistId() !), CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(expectedError, result.Errors[0].Message);
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_WhenNewsAdded()
        {
            // Arrange
            var testNews = GetNews(1, "test");
            this.SetupMockMapping(testNews);
            this.SetupMockRepositoryCreate(testNews);
            this.SetupMockRepositorySaveChangesReturns(1);
            this.SetupMockImageRepositoryGetFirstOrDefaultAsync();

            var handler = new CreateNewsHandler(this.mockMapper.Object, this.mockRepository.Object, this.mockLogger.Object, this.mockLocalizerFail.Object, this.mockLocalizerConvertNull.Object);

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
            this.mockLocalizerFail.Setup(x => x["FailedToCreateNews"]).Returns(new LocalizedString("FailedToCreateNews", expectedError));
            this.SetupMockMapping(testNews);
            this.SetupMockRepositoryCreate(testNews);
            this.SetupMockRepositorySaveChangesException(expectedError);
            this.SetupMockImageRepositoryGetFirstOrDefaultAsync();

            var handler = new CreateNewsHandler(this.mockMapper.Object, this.mockRepository.Object, this.mockLogger.Object, this.mockLocalizerFail.Object, this.mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new CreateNewsCommand(GetNewsCreateDTO()), CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors[0].Message);
        }

        private void SetupMockImageRepositoryGetFirstOrDefaultAsync()
        {
            this.mockRepository.Setup(x => x.ImageRepository.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Image, bool>>>(),
                    It.IsAny<Func<IQueryable<Image>, IIncludableQueryable<Image, object>>>()))
                .ReturnsAsync(new Image { Id = 1 });
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

        private static NewsCreateDto GetNewsCreateDTO()
        {
            return new NewsCreateDto()
            {
                ImageId = 1,
                Title = "Title",
                Text = "test",
                URL = "test",
                CreationDate = new DateTime(2015, 12, 25, 0, 0, 0, DateTimeKind.Utc),
            };
        }

        private static NewsDto GetNewsDTO()
        {
            return new NewsDto()
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

        private static NewsCreateDto? GetNewsDTOWithNotExistId() => null;

        private void SetupMockMapping(DAL.Entities.News.News testNews)
        {
            this.mockMapper.Setup(x => x.Map<DAL.Entities.News.News>(It.IsAny<NewsCreateDto>()))
                .Returns(testNews);
            this.mockMapper.Setup(x => x.Map<NewsDto>(It.IsAny<DAL.Entities.News.News>()))
                .Returns(GetNewsDTO());
        }

        private void SetupMapperWithNullNews()
        {
            this.mockMapper.Setup(x => x.Map<DAL.Entities.News.News?>(It.IsAny<NewsDto>()))
                .Returns(GetNewsWithNotExistId());
        }

        private void SetupMockRepositoryCreate(DAL.Entities.News.News testNews)
        {
            this.mockRepository.Setup(x => x.NewsRepository.Create(It.Is<DAL.Entities.News.News>(y => y.Id == testNews.Id)))
                .Returns(testNews);
        }

        private void SetupMockRepositorySaveChangesException(string expectedError)
        {
            this.mockRepository.Setup(x => x.SaveChanges()).Throws(new Exception(expectedError));
        }

        private void SetupMockRepositorySaveChangesReturns(int number)
        {
            this.mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(number);
        }
    }
}
