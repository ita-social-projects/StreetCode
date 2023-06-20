using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.MediatR.Newss.GetById;
using Streetcode.DAL.Entities.News;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;


namespace Streetcode.XUnitTest.MediatRTests.Newss
{
    public class GetNewsByIdTest
    {
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IBlobService> _mockBlobService;

        public GetNewsByIdTest()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockBlobService = new Mock<IBlobService>();
        }

        [Theory]
        [InlineData(1, "base64-encoded-image")]
        public async Task ShouldReturnSuccessfully_ExistingId_ImageNotNull(int id, string expectedBase64)
        {
            // Arrange
            var testNewsDTO = GetNewsDTO(id);
            var testNews = GetNews(id);
            RepositorySetup(testNews);
            MapperSetup(testNewsDTO);
            BlobServiceSetup(expectedBase64);

            var handler = new GetNewsByIdHandler(_mockMapper.Object, _mockRepository.Object, _mockBlobService.Object);
            // Act
            var result = await handler.Handle(new GetNewsByIdQuery(id), CancellationToken.None);
            // Assert
            Assert.Multiple(
                () => Assert.True(result.IsSuccess),
                () => Assert.Equal(result.Value.Id, id),
                () => Assert.NotNull(result.Value.Image),
                () => Assert.Equal(expectedBase64, result.Value.Image.Base64)
            );
        }

        [Theory]
        [InlineData(1, "base64-encoded-image")]
        public async Task ShouldReturnSuccessfully_ExistingId_ImageNull(int id, string expectedBase64)
        {
            // Arrange
            var testNewsDTO = new NewsDTO { Id = id };
            var testNews = GetNews(id);
            RepositorySetup(testNews);
            MapperSetup(testNewsDTO);
            BlobServiceSetup(expectedBase64);

            var handler = new GetNewsByIdHandler(_mockMapper.Object, _mockRepository.Object, _mockBlobService.Object);
            // Act
            var result = await handler.Handle(new GetNewsByIdQuery(id), CancellationToken.None);
            // Assert
            Assert.Multiple(
                () => Assert.True(result.IsSuccess),
                () => Assert.Null(result.Value.Image)
            );
        }

        [Theory]
        [InlineData(1)]
        public async Task ShouldReturnErrorResponse_NotExistingId(int id)
        {
            // Arrange
            string expectedError = $"No news by entered Id - {id}";
            RepositorySetup(null);
            MapperSetup(null);
            BlobServiceSetup(null);

            var handler = new GetNewsByIdHandler(_mockMapper.Object, _mockRepository.Object, _mockBlobService.Object);
            // Act
            var result = await handler.Handle(new GetNewsByIdQuery(id), CancellationToken.None);
            // Assert
            Assert.Multiple(
                () => Assert.True(result.IsFailed),
                () => Assert.Equal(expectedError, result.Errors.First().Message)
            );
        }

        [Theory]
        [InlineData(1, "base64-encoded-image")]
        public async Task ShouldReturnSuccessfully_CorrectType(int id, string expectedBase64)
        {
            //Arrange
            var testNewsDTO = GetNewsDTO(id);
            var testNews = GetNews(id);
            RepositorySetup(testNews);
            MapperSetup(testNewsDTO);
            BlobServiceSetup(expectedBase64);

            var handler = new GetNewsByIdHandler(_mockMapper.Object, _mockRepository.Object, _mockBlobService.Object);
            //Act
            var result = await handler.Handle(new GetNewsByIdQuery(id), CancellationToken.None);
            //Assert
            Assert.Multiple(
                () => Assert.NotNull(result.ValueOrDefault),
                () => Assert.IsType<NewsDTO>(result.ValueOrDefault)
            );
        }

        private void RepositorySetup(News? news)
        {
            _mockRepository.Setup(repo => repo.NewsRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<News, bool>>>(), It.IsAny<Func<IQueryable<News>,
                IIncludableQueryable<News, object>>>())).ReturnsAsync(news);
        }

        private void MapperSetup(NewsDTO? news)
        {
            _mockMapper.Setup(mapper => mapper.Map<NewsDTO>(It.IsAny<News>()))
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
                BlobName = It.IsAny<string>()
            };
        }

        private static News GetNews(int id)
        {
            return new News
            {
                Id = id
            };
        }

        private static NewsDTO GetNewsDTO(int id)
        {
            return new NewsDTO
            {
                Id = id,
                Image = GetImageDTO()
            };
        }
    }
}
