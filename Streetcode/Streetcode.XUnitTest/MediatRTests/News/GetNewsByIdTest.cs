using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Newss.GetById;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

using Xunit;
using NewsModel = Streetcode.DAL.Entities.News.News;

namespace Streetcode.XUnitTest.MediatRTests.News
{
    public class GetNewsByIdTest
    {
        private readonly Mock<IRepositoryWrapper> mockRepository;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IBlobService> mockBlobService;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly Mock<IStringLocalizer<NoSharedResource>> mockLocalizer;

        public GetNewsByIdTest()
        {
            this.mockMapper = new Mock<IMapper>();
            this.mockRepository = new Mock<IRepositoryWrapper>();
            this.mockBlobService = new Mock<IBlobService>();
            this.mockLogger = new Mock<ILoggerService>();
            this.mockLocalizer = new Mock<IStringLocalizer<NoSharedResource>>();
        }

        [Theory]
        [InlineData(1, "base64-encoded-image")]
        public async Task ShouldReturnSuccessfully_ExistingId_ImageNotNull(int id, string expectedBase64)
        {
            // Arrange
            var testNewsDTO = GetNewsDTO(id);
            var testNews = GetNews(id);
            this.RepositorySetup(testNews);
            this.MapperSetup(testNewsDTO);
            this.BlobServiceSetup(expectedBase64);

            var handler = new GetNewsByIdHandler(this.mockMapper.Object, this.mockRepository.Object, this.mockBlobService.Object, this.mockLogger.Object, this.mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetNewsByIdQuery(id), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.True(result.IsSuccess),
                () => Assert.Equal(result.Value.Id, id),
                () => Assert.NotNull(result.Value.Image),
                () => Assert.Equal(expectedBase64, result.Value.Image?.Base64));
        }

        [Theory]
        [InlineData(1, "base64-encoded-image")]
        public async Task ShouldReturnSuccessfully_ExistingId_ImageNull(int id, string expectedBase64)
        {
            // Arrange
            var testNewsDTO = new NewsDto { Id = id };
            var testNews = GetNews(id);
            this.RepositorySetup(testNews);
            this.MapperSetup(testNewsDTO);
            this.BlobServiceSetup(expectedBase64);

            var handler = new GetNewsByIdHandler(this.mockMapper.Object, this.mockRepository.Object, this.mockBlobService.Object, this.mockLogger.Object, this.mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetNewsByIdQuery(id), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.True(result.IsSuccess),
                () => Assert.Null(result.Value.Image));
        }

        [Theory]
        [InlineData(1)]
        public async Task ShouldReturnErrorResponse_NotExistingId(int id)
        {
            // Arrange
            string expectedError = $"No news by entered Id - {id}";
            this.mockLocalizer.Setup(x => x[It.IsAny<string>(), It.IsAny<object>()]).Returns((string key, object[] args) =>
            {
                if (args != null && args.Length > 0 && args[0] is int id)
                {
                    return new LocalizedString(key, $"No news by entered Id - {id}");
                }

                return new LocalizedString(key, "Cannot find any news with unknown id");
            });
            this.RepositorySetup(null);
            this.MapperSetup(null);
            this.BlobServiceSetup(null);

            var handler = new GetNewsByIdHandler(this.mockMapper.Object, this.mockRepository.Object, this.mockBlobService.Object, this.mockLogger.Object, this.mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetNewsByIdQuery(id), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.True(result.IsFailed),
                () => Assert.Equal(expectedError, result.Errors[0].Message));
        }

        [Theory]
        [InlineData(1, "base64-encoded-image")]
        public async Task ShouldReturnSuccessfully_CorrectType(int id, string expectedBase64)
        {
            // Arrange
            var testNewsDTO = GetNewsDTO(id);
            var testNews = GetNews(id);
            this.RepositorySetup(testNews);
            this.MapperSetup(testNewsDTO);
            this.BlobServiceSetup(expectedBase64);

            var handler = new GetNewsByIdHandler(this.mockMapper.Object, this.mockRepository.Object, this.mockBlobService.Object, this.mockLogger.Object, this.mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetNewsByIdQuery(id), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotNull(result.ValueOrDefault),
                () => Assert.IsType<NewsDto>(result.ValueOrDefault));
        }

        private static ImageDto GetImageDTO()
        {
            return new ImageDto
            {
                BlobName = It.IsAny<string>(),
            };
        }

        private static NewsModel GetNews(int id)
        {
            return new NewsModel
            {
                Id = id,
            };
        }

        private static NewsDto GetNewsDTO(int id)
        {
            return new NewsDto
            {
                Id = id,
                Image = GetImageDTO(),
            };
        }

        private void RepositorySetup(NewsModel? news)
        {
            this.mockRepository.Setup(repo => repo.NewsRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<NewsModel, bool>>>(), It.IsAny<Func<IQueryable<NewsModel>,
                IIncludableQueryable<NewsModel, object>>>())).ReturnsAsync(news);
        }

        private void MapperSetup(NewsDto? news)
        {
            this.mockMapper.Setup(mapper => mapper.Map<NewsDto?>(It.IsAny<NewsModel>()))
                .Returns(news);
        }

        private void BlobServiceSetup(string? expectedBase64)
        {
            this.mockBlobService.Setup<string?>(blob => blob.FindFileInStorageAsBase64(It.IsAny<string>()))
                .Returns(expectedBase64);
        }
    }
}
