using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Newss.GetAll;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Newss
{
    public class GetAllNewsTest
    {
        private Mock<IRepositoryWrapper> _mockRepository;
        private Mock<IMapper> _mockMapper;
        private readonly Mock<IBlobService> _blobService;
        private readonly Mock<ILoggerService> _mockLogger;

        public GetAllNewsTest()
        {
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _blobService = new Mock<IBlobService>();
            _mockLogger = new Mock<ILoggerService>();
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_CorrectType()
        {
            // Arrange
            SetupMockRepositoryGetAllAsync(GetNewsList());

            var handler = new GetAllNewsHandler(_mockRepository.Object, _mockMapper.Object, _blobService.Object, _mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetAllNewsQuery(), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.IsType<List<NewsDTO>>(result.ValueOrDefault)
            );
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_CountMatch()
        {
            // Arrange
            SetupMockRepositoryGetAllAsync(GetNewsList());

            var handler = new GetAllNewsHandler(_mockRepository.Object, _mockMapper.Object, _blobService.Object, _mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetAllNewsQuery(), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.Equal(GetNewsList().Count(), result.Value.Count())
            );
        }

        [Fact]
        public async Task ShouldThrowException_IdNotExist()
        {
            // Arrange
            var expectedError = "There are no news in the database";
            SetupMockRepositoryGetAllAsync(GetNewsListWithNotExistingId());

            var handler = new GetAllNewsHandler(_mockRepository.Object, _mockMapper.Object, _blobService.Object, _mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetAllNewsQuery(), CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors.First().Message);
            _mockMapper.Verify(x => x.Map<IEnumerable<NewsDTO>>(It.IsAny<IEnumerable<DAL.Entities.News.News>>()), Times.Never);
        }

        private void SetupMockRepositoryGetAllAsync(IEnumerable<DAL.Entities.News.News> newsList)
        {
            _mockRepository.Setup(x => x.NewsRepository.GetAllAsync(
                    null,
                    It.IsAny<Func<IQueryable<DAL.Entities.News.News>, IIncludableQueryable<DAL.Entities.News.News, object>>>()))
                .ReturnsAsync(newsList);
            _mockMapper.Setup(x => x.Map<IEnumerable<NewsDTO>>(It.IsAny<IEnumerable<DAL.Entities.News.News>>()))
               .Returns(GetListNewsDTO());
        }

        private static IEnumerable<DAL.Entities.News.News> GetNewsList()
        {
            var news = new List<DAL.Entities.News.News>
            {
                new DAL.Entities.News.News
                {
                    Id = 1
                },
                new DAL.Entities.News.News
                {
                    Id = 2
                }
            };

            return news;
        }

        private static List<DAL.Entities.News.News>? GetNewsListWithNotExistingId()
        {
            return null;
        }

        private static List<NewsDTO> GetListNewsDTO()
        {
            var newsDTO = new List<NewsDTO>
            {
                new NewsDTO
                {
                    Id = 1
                },
                new NewsDTO
                {
                    Id = 2,
                }
            };

            return newsDTO;
        }
    }
}
