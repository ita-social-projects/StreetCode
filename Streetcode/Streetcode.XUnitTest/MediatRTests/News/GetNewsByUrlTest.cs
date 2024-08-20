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
            this._mockMapper = new Mock<IMapper>();
            this._mockRepository = new Mock<IRepositoryWrapper>();
            this._mockBlobService = new Mock<IBlobService>();
            this._mockLogger = new Mock<ILoggerService>();
            this._mockLocalizer = new Mock<IStringLocalizer<NoSharedResource>>();
        }

        [Theory]
        [InlineData("example.url", "base64-encoded-image")]
        public async Task ShouldReturnSuccessfully_ExistingId_ImageNotNull(string url, string expectedBase64)
        {
            // Arrange
            var testNewsDTO = GetNewsDTO(url);
            var testNews = GetNews(url);

            this.RepositorySetup(testNews);
            this.MapperSetup(testNewsDTO);
            this.BlobServiceSetup(expectedBase64);

            var handler = new GetNewsByUrlHandler(this._mockMapper.Object, this._mockRepository.Object, this._mockBlobService.Object, this._mockLogger.Object, this._mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetNewsByUrlQuery(url), CancellationToken.None);

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
            var testNewsDTO = new NewsDTO { URL = url };
            var testNews = GetNews(url);

            this.RepositorySetup(testNews);
            this.MapperSetup(testNewsDTO);
            this.BlobServiceSetup(expectedBase64);

            var handler = new GetNewsByUrlHandler(this._mockMapper.Object, this._mockRepository.Object, this._mockBlobService.Object, this._mockLogger.Object, this._mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetNewsByUrlQuery(url), CancellationToken.None);

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
            this._mockLocalizer.Setup(x => x[It.IsAny<string>(), It.IsAny<object>()]).Returns((string key, object[] args) =>
            {
                if (args != null && args.Length > 0 && args[0] is string url)
                {
                    return new LocalizedString(key, $"No news by entered Url - {url}");
                }

                return new LocalizedString(key, "Cannot find any news with unknown Url");
            });
            this.RepositorySetup(null);
            this.MapperSetup(null);
            this.BlobServiceSetup(null);

            var handler = new GetNewsByUrlHandler(this._mockMapper.Object, this._mockRepository.Object, this._mockBlobService.Object, this._mockLogger.Object, this._mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetNewsByUrlQuery(url), CancellationToken.None);

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
            var testNewsDTO = GetNewsDTO(url);
            var testNews = GetNews(url);

            this.RepositorySetup(testNews);
            this.MapperSetup(testNewsDTO);
            this.BlobServiceSetup(expectedBase64);

            var handler = new GetNewsByUrlHandler(this._mockMapper.Object, this._mockRepository.Object, this._mockBlobService.Object, this._mockLogger.Object, this._mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetNewsByUrlQuery(url), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotNull(result.ValueOrDefault),
                () => Assert.IsType<NewsDTO>(result.ValueOrDefault));
        }

        private static ImageDTO GetImageDTO()
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

        private static NewsDTO GetNewsDTO(string url)
        {
            return new NewsDTO
            {
                URL = url,
                Image = GetImageDTO(),
            };
        }

        private void RepositorySetup(NewsModel? news)
        {
            this._mockRepository.Setup(repo => repo.NewsRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<NewsModel, bool>>>(), It.IsAny<Func<IQueryable<NewsModel>, IIncludableQueryable<NewsModel, object>>>()))
                .ReturnsAsync(news);
        }

        private void MapperSetup(NewsDTO? news)
        {
            this._mockMapper.Setup(mapper => mapper.Map<NewsDTO?>(It.IsAny<object>()))
                .Returns(news);
        }

        private void BlobServiceSetup(string? expectedBase64)
        {
            this._mockBlobService.Setup(blob => blob.FindFileInStorageAsBase64(It.IsAny<string>()))
                .Returns(expectedBase64!);
        }
    }
}