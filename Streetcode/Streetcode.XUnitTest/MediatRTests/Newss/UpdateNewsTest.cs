using AutoMapper;
using Moq;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.MediatR.Newss.Update;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;
using Streetcode.DAL.Entities.Media.Images;
using Microsoft.EntityFrameworkCore.Query;
using System.Threading.Tasks;
using System.Threading;
using Streetcode.BLL.Interfaces.Logging;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.SharedResource;
using Streetcode.BLL.MediatR.Newss.Create;

namespace Streetcode.XUnitTest.MediatRTests.Newss
{
    public class UpdateNewsTests
    {
        private Mock<IRepositoryWrapper> _mockRepository;
        private Mock<IMapper> _mockMapper;
        private readonly Mock<IBlobService> _blobService;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<FailedToUpdateSharedResource>> _mockLocalizerFailedToUpdate;
        private readonly Mock<IStringLocalizer<CannotConvertNullSharedResource>> _mockLocalizerConvertNull;

        public UpdateNewsTests()
        {
            _mockRepository = new();
            _mockMapper = new();
            _blobService = new Mock<IBlobService>();
            _mockLogger = new Mock<ILoggerService>();
            _mockLocalizerConvertNull = new Mock<IStringLocalizer<CannotConvertNullSharedResource>>();
            _mockLocalizerFailedToUpdate = new Mock<IStringLocalizer<FailedToUpdateSharedResource>>();
        }

        [Theory]
        [InlineData(1)]
        public async Task ShouldReturnSuccessfully_WhenUpdated(int returnNumber)
        {
            // Arrange
            var testNews = GetNews();
            var testNewsDTO = GetNewsDTO();

            SetupUpdateRepository(returnNumber);
            SetupMapper(testNews, testNewsDTO);
            SetupImageRepository();

            var handler = new UpdateNewsHandler(_mockRepository.Object, _mockMapper.Object, _blobService.Object, _mockLogger.Object, _mockLocalizerFailedToUpdate.Object, _mockLocalizerConvertNull.Object);

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
            _mockLocalizerConvertNull.Setup(x => x["CannotConvertNullToNews"])
            .Returns(new LocalizedString("CannotConvertNullToNews", expectedError));
            SetupUpdateRepository(returnNumber);
            SetupMapperWithNullNews();

            var handler = new UpdateNewsHandler(_mockRepository.Object, _mockMapper.Object, _blobService.Object, _mockLogger.Object, _mockLocalizerFailedToUpdate.Object, _mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new UpdateNewsCommand(GetNewsDTOWithNotExistId()), CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(expectedError, result.Errors.First().Message);
        }


        [Fact]
        public async Task ShouldReturnFail_CreationDateIsRequired()
        {
            // Arrange
            string expectedErrorMessage = "CreationDate field is required";
            var testNews = GetNewsWithDefaultCreationDate();
            var testNewsDto = GetNewsDTOWithDefaultCreationDate();
            SetupMapper(testNews, testNewsDto);
            var handler = new UpdateNewsHandler(_mockRepository.Object, _mockMapper.Object, _blobService.Object, _mockLogger.Object, _mockLocalizerFailedToUpdate.Object, _mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new UpdateNewsCommand(testNewsDto), CancellationToken.None);
            // Assert
            Assert.Multiple(
                () => Assert.True(result.IsFailed),
                () => Assert.Equal(expectedErrorMessage, result.Errors.First().Message)
            );

        }

        [Theory]
        [InlineData(-1)]
        public async Task ShouldThrowException_SaveChangesAsyncIsNotSuccessful(int returnNumber)
        {
            // Arrange
            var testNews = GetNews();
            var testNewsDTO = GetNewsDTO();

            SetupUpdateRepository(returnNumber);
            SetupMapper(testNews, testNewsDTO);
            SetupImageRepository();

            var expectedError = "Failed to update news";
            _mockLocalizerFailedToUpdate.Setup(x => x["FailedToUpdateNews"])
            .Returns(new LocalizedString("FailedToUpdateNews", expectedError));
            var handler = new UpdateNewsHandler(_mockRepository.Object, _mockMapper.Object, _blobService.Object, _mockLogger.Object, _mockLocalizerFailedToUpdate.Object, _mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new UpdateNewsCommand(testNewsDTO), CancellationToken.None);

            // Assert
            Assert.True(result.IsFailed);
            Assert.Equal(expectedError, result.Errors.First().Message);
        }

        [Theory]
        [InlineData(1)]
        public async Task ShouldReturnSuccessfully_TypeIsCorrect(int returnNumber)
        {
            // Arrange
            SetupCreateRepository(returnNumber);
            SetupMapper(GetNews(), GetNewsDTO());
            SetupImageRepository();

            var handler = new UpdateNewsHandler(_mockRepository.Object, _mockMapper.Object, _blobService.Object, _mockLogger.Object, _mockLocalizerFailedToUpdate.Object, _mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new UpdateNewsCommand(GetNewsDTO()), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
        }

        private void SetupUpdateRepository(int returnNumber)
        {
            _mockRepository.Setup(x => x.NewsRepository.Update(It.IsAny<DAL.Entities.News.News>()));
            _mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(returnNumber);
        }

        private void SetupCreateRepository(int returnNumber)
        {
            _mockRepository.Setup(x => x.NewsRepository.Create(GetNews()));
            _mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(returnNumber);
        }

        private void SetupMapper(DAL.Entities.News.News testNews, UpdateNewsDTO testNewsDTO)
        {
            _mockMapper.Setup(x => x.Map<DAL.Entities.News.News>(It.IsAny<UpdateNewsDTO>()))
                .Returns(testNews);
            _mockMapper.Setup(x => x.Map<UpdateNewsDTO>(It.IsAny<DAL.Entities.News.News>()))
                .Returns(testNewsDTO);
        }

        private void SetupMapperWithNullNews()
        {
            _mockMapper.Setup(x => x.Map<DAL.Entities.News.News>(It.IsAny<UpdateNewsDTO>()))
                .Returns(GetNewsWithNotExistId());
        }

        private void SetupImageRepository()
        {
            _mockRepository.Setup(x => x.ImageRepository.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Image, bool>>>(),
                    It.IsAny<Func<IQueryable<Image>, IIncludableQueryable<Image, object>>>()
                )).ReturnsAsync((Image)null);

            _blobService.Setup(x => x.FindFileInStorageAsBase64(It.IsAny<string>()))
                .Returns("base64Image");
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
                CreationDate = new DateTime(2015, 12, 25)
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
                CreationDate = new DateTime(2015, 12, 25)
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

        private static DAL.Entities.News.News GetNewsWithNotExistId() => null;

        private static UpdateNewsDTO GetNewsDTOWithNotExistId() => null;
    }
}
