using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.MediatR.Newss.GetByUrl;
using Streetcode.DAL.Entities.News;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;
using NewsModel = Streetcode.DAL.Entities.News.News;


namespace Streetcode.XUnitTest.MediatRTests.Newss
{
    public class GetNewsByUrlTest
    {
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IBlobService> _mockBlobService;

        public GetNewsByUrlTest()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockBlobService = new Mock<IBlobService>();
        }

        [Theory]
        [InlineData("example.url", "base64-encoded-image")]
        public async Task ShouldReturnSuccessfully_ExistingId_ImageNotNull(string url, string expectedBase64)
        {
            // Arrange
            var testNewsDTO = GetNewsDTO(url);
            var testNews = GetNews(url);

            RepositorySetup(testNews);
            MapperSetup(testNewsDTO);
            BlobServiceSetup(expectedBase64);

            var handler = new GetNewsByUrlHandler(_mockMapper.Object, _mockRepository.Object, _mockBlobService.Object);
            // Act
            var result = await handler.Handle(new GetNewsByUrlQuery(url), CancellationToken.None);
            // Assert
            Assert.Multiple(
                () => Assert.True(result.IsSuccess),
                () => Assert.Equal(result.Value.URL, url),
                () => Assert.NotNull(result.Value.Image),
                () => Assert.Equal(expectedBase64, result.Value.Image.Base64)
            );
        }

        [Theory]
        [InlineData("example.url", "base64-encoded-image")]
        public async Task ShouldReturnSuccessfully_ExistingId_ImageNull(string url, string expectedBase64)
        {
            // Arrange
            var testNewsDTO = new NewsDTO { URL = url };
            var testNews = GetNews(url);

            RepositorySetup(testNews);
            MapperSetup(testNewsDTO);
            BlobServiceSetup(expectedBase64);

            var handler = new GetNewsByUrlHandler(_mockMapper.Object, _mockRepository.Object, _mockBlobService.Object);
            // Act
            var result = await handler.Handle(new GetNewsByUrlQuery(url), CancellationToken.None);
            // Assert
            Assert.Multiple(
                () => Assert.True(result.IsSuccess),
                () => Assert.Null(result.Value.Image)
            );
        }

        [Theory]
        [InlineData("example.url")]
        public async Task ShouldReturnErrorResponse_NotExistingId(string url)
        {
            // Arrange
            string expectedError = $"No news by entered Url - {url}";
            RepositorySetup(null);
            MapperSetup(null);
            BlobServiceSetup(null);

            var handler = new GetNewsByUrlHandler(_mockMapper.Object, _mockRepository.Object, _mockBlobService.Object);
            // Act
            var result = await handler.Handle(new GetNewsByUrlQuery(url), CancellationToken.None);
            // Assert
            Assert.Multiple(
                () => Assert.True(result.IsFailed),
                () => Assert.Equal(expectedError, result.Errors.First().Message)
            );
        }

        [Theory]
        [InlineData("example.url", "base64-encoded-image")]
        public async Task ShouldReturnSuccessfully_CorrectType(string url, string expectedBase64)
        {
            //Arrange
            var testNewsDTO = GetNewsDTO(url);
            var testNews = GetNews(url);

            RepositorySetup(testNews);
            MapperSetup(testNewsDTO);
            BlobServiceSetup(expectedBase64);

            var handler = new GetNewsByUrlHandler(_mockMapper.Object, _mockRepository.Object, _mockBlobService.Object);
            //Act
            var result = await handler.Handle(new GetNewsByUrlQuery(url), CancellationToken.None);
            //Assert
            Assert.Multiple(
                () => Assert.NotNull(result.ValueOrDefault),
                () => Assert.IsType<NewsDTO>(result.ValueOrDefault)
            );
        }

        private void RepositorySetup(NewsModel? news)
        {
            _mockRepository.Setup(repo => repo.NewsRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<NewsModel, bool>>>(), It.IsAny<Func<IQueryable<NewsModel>, IIncludableQueryable<NewsModel, object>>>()))
                .ReturnsAsync(news);
        }

        private void MapperSetup(NewsDTO? news)
        {
            _mockMapper.Setup(mapper => mapper.Map<NewsDTO>(It.IsAny<object>()))
                .Returns(news);
        }

        private void BlobServiceSetup(string? expectedBase64)
        {
            _mockBlobService.Setup(blob => blob.FindFileInStorageAsBase64(It.IsAny<string>()))
                .Returns(expectedBase64);
        }

        private static ImageDTO GetImageDTO()
        {
            return new ImageDTO
            {
                BlobName = It.IsAny<string>(),
                Base64 = It.IsAny<string>()
            };
        }
        private static NewsModel GetNews(string url)
        {
            return new NewsModel
            {
                URL = url
            };
        }
        private static NewsDTO GetNewsDTO(string url)
        {
            return new NewsDTO
            {
                URL = url,
                Image = GetImageDTO()
            };
        }
    }
}