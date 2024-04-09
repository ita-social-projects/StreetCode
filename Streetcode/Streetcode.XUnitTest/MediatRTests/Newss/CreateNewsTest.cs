using AutoMapper;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Newss.Create;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
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
            var testNews = GetNews();
            SetupMockMapping(testNews);
            SetupMockRepositoryCreate(testNews);
            SetupMockRepositorySaveChangesReturns(1);

            var handler = new CreateNewsHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object, _mockLocalizerFail.Object, _mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new CreateNewsCommand(GetNewsDTO()), CancellationToken.None);

            // Assert
            Assert.IsType<CreateNewsDTO>(result.Value);
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_WhenNewsAdded()
        {
            // Arrange
            var testNews = GetNews();
            SetupMockMapping(testNews);
            SetupMockRepositoryCreate(testNews);
            SetupMockRepositorySaveChangesReturns(1);

            var handler = new CreateNewsHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object, _mockLocalizerFail.Object, _mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new CreateNewsCommand(GetNewsDTO()), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task ShouldThrowException_SaveChangesIsNotSuccessful()
        {
            // Arrange
            var testNews = GetNews();
            var expectedError = "Failed to create a news";
            _mockLocalizerFail.Setup(x => x["FailedToCreateNews"]).Returns(new LocalizedString("FailedToCreateNews", expectedError));
            SetupMockMapping(testNews);
            SetupMockRepositoryCreate(testNews);
            SetupMockRepositorySaveChangesException(expectedError);

            var handler = new CreateNewsHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object, _mockLocalizerFail.Object, _mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new CreateNewsCommand(GetNewsDTO()), CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors.First().Message);
        }

        private void SetupMockMapping(DAL.Entities.News.News testNews)
        {
            _mockMapper.Setup(x => x.Map<DAL.Entities.News.News>(It.IsAny<CreateNewsDTO>()))
                .Returns(testNews);
            _mockMapper.Setup(x => x.Map<CreateNewsDTO>(It.IsAny<DAL.Entities.News.News>()))
                .Returns(GetNewsDTO());
        }

        private void SetupMockRepositoryCreate(DAL.Entities.News.News testNews)
        {
            _mockRepository.Setup(x => x.NewsRepository.Create(It.Is<DAL.Entities.News.News>(y => y.Id == testNews.Id)))
                .Returns(testNews);
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
                Id = 1,
                ImageId = 1,
                Title = "Title",
                Text = "test",
                URL = "test",
                CreationDate = new DateTime(2015, 12, 25)
            };
        }

        private static CreateNewsDTO GetNewsDTO()
        {
            return new CreateNewsDTO()
            {
                ImageId = 1,
                Title = "Title",
                Text = "test",
                URL = "test",
                CreationDate = new DateTime(2015, 12, 25)
            };
        }
    }
}
