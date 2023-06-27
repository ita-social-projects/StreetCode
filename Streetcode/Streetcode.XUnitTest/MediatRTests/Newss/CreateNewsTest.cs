using AutoMapper;
using Moq;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.MediatR.Newss.Create;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Newss
{
    public class CreateNewsTest
    {
        private Mock<IRepositoryWrapper> _mockRepository;
        private Mock<IMapper> _mockMapper;
        public CreateNewsTest()
        {
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_TypeIsCorrect()
        {
            // Arrange
            var testNews = GetNews();
            SetupMockMapping(testNews);
            SetupMockRepositoryCreate(testNews);
            SetupMockRepositorySaveChangesReturns(1);

            var handler = new CreateNewsHandler(_mockMapper.Object, _mockRepository.Object);

            // Act
            var result = await handler.Handle(new CreateNewsCommand(GetNewsDTO()), CancellationToken.None);

            // Assert
            Assert.IsType<NewsDTO>(result.Value);
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_WhenNewsAdded()
        {
            // Arrange
            var testNews = GetNews();
            SetupMockMapping(testNews);
            SetupMockRepositoryCreate(testNews);
            SetupMockRepositorySaveChangesReturns(1);

            var handler = new CreateNewsHandler(_mockMapper.Object, _mockRepository.Object);

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
            SetupMockMapping(testNews);
            SetupMockRepositoryCreate(testNews);
            SetupMockRepositorySaveChangesException(expectedError);

            var handler = new CreateNewsHandler(_mockMapper.Object, _mockRepository.Object);

            // Act
            var result = await handler.Handle(new CreateNewsCommand(GetNewsDTO()), CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors.First().Message);
        }

        private void SetupMockMapping(DAL.Entities.News.News testNews)
        {
            _mockMapper.Setup(x => x.Map<DAL.Entities.News.News>(It.IsAny<NewsDTO>()))
                .Returns(testNews);
            _mockMapper.Setup(x => x.Map<NewsDTO>(It.IsAny<DAL.Entities.News.News>()))
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
                Id = 1
            };
        }

        private static NewsDTO GetNewsDTO()
        {
            return new NewsDTO();
        }
    }
}
