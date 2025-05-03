using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Newss.GetByUrl;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;
using NewsModel = Streetcode.DAL.Entities.News.News;

namespace Streetcode.XUnitTest.MediatRTests.News
{
    public class GetNewsByUrlTest
    {
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IBlobService> _mockBlobService;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<NoSharedResource>> _mockLocalizer;

        public GetNewsByUrlTest()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockBlobService = new Mock<IBlobService>();
            _mockLogger = new Mock<ILoggerService>();
            _mockLocalizer = new Mock<IStringLocalizer<NoSharedResource>>();
        }

        [Theory]
        [InlineData("example.url", "base64-encoded-image")]
        public async Task ShouldReturnSuccessfully_ExistingId_ImageNotNull(string url, string expectedBase64)
        {
            // Arrange
            var testNewsDto = GetNewsDto(url);
            var testNews = GetNews(url);

            RepositorySetup(testNews);
            MapperSetup(testNewsDto);
            BlobServiceSetup(expectedBase64);

            var handler = new GetNewsByUrlHandler(_mockMapper.Object, _mockRepository.Object, _mockBlobService.Object, _mockLogger.Object, _mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetNewsByUrlQuery(url, UserRole.User), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.True(result.IsSuccess),
                () => Assert.Equal(result.Value.URL, url),
                () => Assert.NotNull(result.Value.Image),
                () => Assert.Equal(expectedBase64, result.Value.Image?.Base64));
        }

        [Theory]
        [InlineData("example.url", "base64-encoded-image")]
        public async Task ShouldReturnSuccessfully_ExistingId_ImageNull(string url, string expectedBase64)
        {
            // Arrange
            var testNewsDto = new NewsDTO { URL = url };
            var testNews = GetNews(url);

            RepositorySetup(testNews);
            MapperSetup(testNewsDto);
            BlobServiceSetup(expectedBase64);

            var handler = new GetNewsByUrlHandler(_mockMapper.Object, _mockRepository.Object, _mockBlobService.Object, _mockLogger.Object, _mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetNewsByUrlQuery(url, UserRole.User), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.True(result.IsSuccess),
                () => Assert.Null(result.Value.Image));
        }

        [Theory]
        [InlineData("example.url")]
        public async Task ShouldReturnErrorResponse_NotExistingId(string url)
        {
            // Arrange
            string expectedError = $"No news by entered Url - {url}";
            _mockLocalizer.Setup(x => x[It.IsAny<string>(), It.IsAny<object>()]).Returns((string key, object[] args) =>
            {
                if (args != null && args.Length > 0 && args[0] is string url)
                {
                    return new LocalizedString(key, $"No news by entered Url - {url}");
                }

                return new LocalizedString(key, "Cannot find any news with unknown Url");
            });
            RepositorySetup(null);
            MapperSetup(null);
            BlobServiceSetup(null);

            var handler = new GetNewsByUrlHandler(_mockMapper.Object, _mockRepository.Object, _mockBlobService.Object, _mockLogger.Object, _mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetNewsByUrlQuery(url, UserRole.User), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.True(result.IsFailed),
                () => Assert.Equal(expectedError, result.Errors[0].Message));
        }

        [Theory]
        [InlineData("example.url", "base64-encoded-image")]
        public async Task ShouldReturnSuccessfully_CorrectType(string url, string expectedBase64)
        {
            // Arrange
            var testNewsDto = GetNewsDto(url);
            var testNews = GetNews(url);

            RepositorySetup(testNews);
            MapperSetup(testNewsDto);
            BlobServiceSetup(expectedBase64);

            var handler = new GetNewsByUrlHandler(_mockMapper.Object, _mockRepository.Object, _mockBlobService.Object, _mockLogger.Object, _mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetNewsByUrlQuery(url, UserRole.User), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotNull(result.ValueOrDefault),
                () => Assert.IsType<NewsDTO>(result.ValueOrDefault));
        }

        private static ImageDTO GetImageDto()
        {
            return new ImageDTO
            {
                BlobName = It.IsAny<string>(),
                Base64 = It.IsAny<string>(),
            };
        }

        private static NewsModel GetNews(string url)
        {
            return new NewsModel
            {
                URL = url,
            };
        }

        private static NewsDTO GetNewsDto(string url)
        {
            return new NewsDTO
            {
                URL = url,
                Image = GetImageDto(),
            };
        }

        private void RepositorySetup(NewsModel? news)
        {
            _mockRepository.Setup(repo => repo.NewsRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<NewsModel, bool>>>(), It.IsAny<Func<IQueryable<NewsModel>, IIncludableQueryable<NewsModel, object>>>()))
                .ReturnsAsync(news);
        }

        private void MapperSetup(NewsDTO? news)
        {
            _mockMapper.Setup(mapper => mapper.Map<NewsDTO?>(It.IsAny<object>()))
                .Returns(news);
        }

        private void BlobServiceSetup(string? expectedBase64)
        {
            _mockBlobService.Setup(blob => blob.FindFileInStorageAsBase64(It.IsAny<string>()))
                .Returns(expectedBase64!);
        }
    }
}