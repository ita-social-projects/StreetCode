using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Newss.Update;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.News
{
    public class UpdateNewsTest
    {
        private readonly Mock<IRepositoryWrapper> mockRepository;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IBlobService> blobService;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly Mock<IStringLocalizer<FailedToUpdateSharedResource>> mockLocalizerFailedToUpdate;
        private readonly Mock<IStringLocalizer<CannotConvertNullSharedResource>> mockLocalizerConvertNull;

        public UpdateNewsTest()
        {
            this.mockRepository = new ();
            this.mockMapper = new ();
            this.blobService = new Mock<IBlobService>();
            this.mockLogger = new Mock<ILoggerService>();
            this.mockLocalizerConvertNull = new Mock<IStringLocalizer<CannotConvertNullSharedResource>>();
            this.mockLocalizerFailedToUpdate = new Mock<IStringLocalizer<FailedToUpdateSharedResource>>();
        }

        [Theory]
        [InlineData(1)]
        public async Task ShouldReturnSuccessfully_WhenUpdated(int returnNumber)
        {
            // Arrange
            var testNews = GetNews();
            var testNewsDTO = GetNewsDTO();

            this.SetupUpdateRepository(returnNumber);
            this.SetupMapper(testNews, testNewsDTO);
            this.SetupImageRepository();

            var handler = new UpdateNewsHandler(this.mockRepository.Object, this.mockMapper.Object, this.blobService.Object, this.mockLogger.Object, this.mockLocalizerFailedToUpdate.Object, this.mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new UpdateNewsCommand(testNewsDTO), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.IsType<UpdateNewsDTO>(result.Value);
        }

        [Theory]
        [InlineData(1)]
        public async Task ShouldThrowException_TryMapNullRequest(int returnNumber)
        {
            // Arrange
            var expectedError = "Cannot convert null to news";
            this.mockLocalizerConvertNull.Setup(x => x["CannotConvertNullToNews"])
                .Returns(new LocalizedString("CannotConvertNullToNews", expectedError));
            this.SetupUpdateRepository(returnNumber);
            this.SetupMapperWithNullNews();

            var handler = new UpdateNewsHandler(this.mockRepository.Object, this.mockMapper.Object, this.blobService.Object, this.mockLogger.Object, this.mockLocalizerFailedToUpdate.Object, this.mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new UpdateNewsCommand(GetNewsDTOWithNotExistId() !), CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(expectedError, result.Errors[0].Message);
        }

        [Fact]
        public async Task ShouldReturnFail_CreationDateIsRequired()
        {
            // Arrange
            string expectedErrorMessage = "CreationDate field is required";
            var testNews = GetNewsWithDefaultCreationDate();
            var testNewsDto = GetNewsDTOWithDefaultCreationDate();
            this.SetupMapper(testNews, testNewsDto);
            var handler = new UpdateNewsHandler(this.mockRepository.Object, this.mockMapper.Object, this.blobService.Object, this.mockLogger.Object, this.mockLocalizerFailedToUpdate.Object, this.mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new UpdateNewsCommand(testNewsDto), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.True(result.IsFailed),
                () => Assert.Equal(expectedErrorMessage, result.Errors[0].Message));
        }

        [Theory]
        [InlineData(-1)]
        public async Task ShouldThrowException_SaveChangesAsyncIsNotSuccessful(int returnNumber)
        {
            // Arrange
            var testNews = GetNews();
            var testNewsDTO = GetNewsDTO();

            this.SetupUpdateRepository(returnNumber);
            this.SetupMapper(testNews, testNewsDTO);
            this.SetupImageRepository();

            var expectedError = "Failed to update news";
            this.mockLocalizerFailedToUpdate.Setup(x => x["FailedToUpdateNews"])
                .Returns(new LocalizedString("FailedToUpdateNews", expectedError));
            var handler = new UpdateNewsHandler(this.mockRepository.Object, this.mockMapper.Object, this.blobService.Object, this.mockLogger.Object, this.mockLocalizerFailedToUpdate.Object, this.mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new UpdateNewsCommand(testNewsDTO), CancellationToken.None);

            // Assert
            Assert.True(result.IsFailed);
            Assert.Equal(expectedError, result.Errors[0].Message);
        }

        [Theory]
        [InlineData(1)]
        public async Task ShouldReturnSuccessfully_TypeIsCorrect(int returnNumber)
        {
            // Arrange
            this.SetupCreateRepository(returnNumber);
            this.SetupMapper(GetNews(), GetNewsDTO());
            this.SetupImageRepository();

            var handler = new UpdateNewsHandler(this.mockRepository.Object, this.mockMapper.Object, this.blobService.Object, this.mockLogger.Object, this.mockLocalizerFailedToUpdate.Object, this.mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new UpdateNewsCommand(GetNewsDTO()), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task ShouldReturnFail_NewsWithSameTitleExists()
        {
            // Arrange
            var testNews = GetNews();
            var testNewsDTO = GetNewsDTO();
            var expectedError = "A news with the same title already exists.";
            this.SetupMapper(testNews, testNewsDTO);
            this.SetupRepositoryGetFirstOrDefaultAsyncWithExistingTitle(testNews.Title);
            var handler = new UpdateNewsHandler(this.mockRepository.Object, this.mockMapper.Object, this.blobService.Object, this.mockLogger.Object, this.mockLocalizerFailedToUpdate.Object, this.mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new UpdateNewsCommand(GetNewsDTO()), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.True(result.IsFailed),
                () => Assert.Equal(expectedError, result.Errors[0].Message));
        }

        [Fact]
        public async Task ShouldThrowException_NewsWithSameTextExists()
        {
            // Arrange
            var testNews = GetNews();
            var testNewsDTO = GetNewsDTO();
            var expectedError = "A news with the same text already exists.";
            this.SetupMapper(testNews, testNewsDTO);
            this.SetupMockRepositoryGetSingleOrDefaultAsyncWithExistingText(testNews.Text);
            var handler = new UpdateNewsHandler(this.mockRepository.Object, this.mockMapper.Object, this.blobService.Object, this.mockLogger.Object, this.mockLocalizerFailedToUpdate.Object, this.mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new UpdateNewsCommand(GetNewsDTO()), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.True(result.IsFailed),
                () => Assert.Equal(expectedError, result.Errors[0].Message));
        }

        private static DAL.Entities.News.News GetNews()
        {
            return new DAL.Entities.News.News()
            {
                Id = 1,
                ImageId = 1,
                Title = "Title",
                Text = "Text",
                URL = "URL",
                CreationDate = new DateTime(2015, 12, 25, 0, 0, 0, DateTimeKind.Utc),
            };
        }

        private static DAL.Entities.News.News GetNewsWithDefaultCreationDate()
        {
            return new DAL.Entities.News.News()
            {
                Id = 1,
                ImageId = 1,
                Title = "Title",
                Text = "Text",
                URL = "URL",
            };
        }

        private static UpdateNewsDTO GetNewsDTO()
        {
            return new UpdateNewsDTO()
            {
                Id = 1,
                ImageId = 1,
                Title = "Title",
                Text = "Text",
                URL = "URL",
                CreationDate = new DateTime(2015, 12, 25, 0, 0, 0, DateTimeKind.Utc),
            };
        }

        private static UpdateNewsDTO GetNewsDTOWithDefaultCreationDate()
        {
            return new UpdateNewsDTO()
            {
                Id = 1,
                ImageId = 1,
                Title = "Title",
                Text = "Text",
                URL = "URL",
            };
        }

        private static DAL.Entities.News.News? GetNewsWithNotExistId() => null;

        private static UpdateNewsDTO? GetNewsDTOWithNotExistId() => null;

        private void SetupUpdateRepository(int returnNumber)
        {
            this.mockRepository.Setup(x => x.NewsRepository.Update(It.IsAny<DAL.Entities.News.News>()));
            this.mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(returnNumber);
        }

        private void SetupCreateRepository(int returnNumber)
        {
            this.mockRepository.Setup(x => x.NewsRepository.Create(GetNews()));
            this.mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(returnNumber);
        }

        private void SetupMapper(DAL.Entities.News.News testNews, UpdateNewsDTO testNewsDTO)
        {
            this.mockMapper.Setup(x => x.Map<DAL.Entities.News.News>(It.IsAny<UpdateNewsDTO>()))
                .Returns(testNews);
            this.mockMapper.Setup(x => x.Map<UpdateNewsDTO>(It.IsAny<DAL.Entities.News.News>()))
                .Returns(testNewsDTO);
        }

        private void SetupMapperWithNullNews()
        {
            this.mockMapper.Setup(x => x.Map<DAL.Entities.News.News?>(It.IsAny<UpdateNewsDTO>()))
                .Returns(GetNewsWithNotExistId());
        }

        private void SetupRepositoryGetFirstOrDefaultAsyncWithExistingTitle(string title)
        {
            this.mockRepository.Setup(x => x.NewsRepository.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<DAL.Entities.News.News, bool>>>(),
                    It.IsAny<Func<IQueryable<DAL.Entities.News.News>, IIncludableQueryable<DAL.Entities.News.News, object>>>()))
                .ReturnsAsync((Expression<Func<DAL.Entities.News.News, bool>> predicate, Func<IQueryable<DAL.Entities.News.News>, IIncludableQueryable<DAL.Entities.News.News, object>> include) =>
                {
                    var newsList = new List<DAL.Entities.News.News>
                    {
                        new DAL.Entities.News.News { Title = title },
                    };
                    return newsList.Find(new Predicate<DAL.Entities.News.News>(predicate.Compile()));
                });
        }

        private void SetupMockRepositoryGetSingleOrDefaultAsyncWithExistingText(string text)
        {
            this.mockRepository.Setup(x => x.NewsRepository.GetSingleOrDefaultAsync(
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

        private void SetupImageRepository()
        {
            this.mockRepository.Setup(x => x.ImageRepository.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Image, bool>>>(),
                    It.IsAny<Func<IQueryable<Image>, IIncludableQueryable<Image, object>>>()))
                .ReturnsAsync((Image?)null);

            this.blobService.Setup(x => x.FindFileInStorageAsBase64(It.IsAny<string>()))
                .Returns("base64Image");
        }
    }
}
