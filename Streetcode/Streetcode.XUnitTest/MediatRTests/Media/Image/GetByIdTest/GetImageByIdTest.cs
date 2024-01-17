using Moq;
using Xunit;
using Streetcode.BLL.MediatR.Media.Image.GetById;
using Streetcode.BLL.DTO.Media.Images;
using AutoMapper;
using Streetcode.DAL.Repositories.Interfaces.Base;
using FluentResults;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.SharedResource;

namespace Streetcode.XUnitTest.MediatRTests.Media.Images
{
    public class GetImageByIdTest
    {
        private Mock<IRepositoryWrapper> _mockRepo;
        private Mock<IMapper> _mockMapper;
        private Mock<IBlobService> _blobService;
        private readonly Mock<ILoggerService> _mockLogger;

        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizer;
        public GetImageByIdTest()
        {
            _mockRepo = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _blobService = new Mock<IBlobService>();
            _mockLogger = new Mock<ILoggerService>();
            _mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ReturnsImage(int id)
        {
            // Arrange
            GetMockRepositoryAndMapper(GetImage(), GetImageDTO());
            var handler = new GetImageByIdHandler(_mockRepo.Object, _mockMapper.Object, _blobService.Object, _mockLogger.Object, _mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetImageByIdQuery(id), CancellationToken.None);

            // Assert
            Assert.Equal(id, result.Value.Id);
        }

        [Theory]
        [InlineData(1)]

        public async Task Handle_ReturnsType(int id)
        {
            // Arrange
            GetMockRepositoryAndMapper(GetImage(), GetImageDTO());
            var handler = new GetImageByIdHandler(_mockRepo.Object, _mockMapper.Object, _blobService.Object, _mockLogger.Object, _mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetImageByIdQuery(id), CancellationToken.None);

            // Assert
            Assert.IsType<Result<ImageDTO>>(result);
        }

        [Theory]
        [InlineData(-1)]
        public async Task Handle_WithNonExistentId_ReturnsError(int id)
        {
            // Arrange
            GetMockRepositoryAndMapper(null, null);
            var handler = new GetImageByIdHandler(_mockRepo.Object, _mockMapper.Object, _blobService.Object, _mockLogger.Object, _mockLocalizer.Object);
            var expectedError = $"Cannot find a image with corresponding id: {id}";

            // Act
            var result = await handler.Handle(new GetImageByIdQuery(id), CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors.First().Message);

        }

        private Image GetImage()
        {
            return new Image()
            {
                Id = 2,
                BlobName = "https://",
                MimeType = ""
            };
        }

        private ImageDTO GetImageDTO()
        {
            return new ImageDTO
            {
                Id = 1,
            };
        }

        private void GetMockRepositoryAndMapper(Image Image, ImageDTO ImageDTO)
        {
            _mockRepo.Setup(r => r.ImageRepository.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<Image, bool>>>(),
            It.IsAny<Func<IQueryable<Image>,
            IIncludableQueryable<Image, object>>>()))
            .ReturnsAsync(Image);

            _mockMapper.Setup(x => x.Map<ImageDTO>(It.IsAny<object>()))
            .Returns(ImageDTO);

            _mockLocalizer.Setup(x => x[It.IsAny<string>(), It.IsAny<object>()])
            .Returns((string key, object[] args) =>
            {
                if (args != null && args.Length > 0 && args[0] is int id)
                {
                    return new LocalizedString(key, $"Cannot find a image with corresponding id: {id}");
                }

                return new LocalizedString(key, "Cannot find an image with unknown Id");
            });
        }
    }
}