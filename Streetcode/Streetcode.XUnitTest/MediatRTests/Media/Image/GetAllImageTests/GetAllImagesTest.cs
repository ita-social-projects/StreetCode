using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Media.Image.GetAll;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Media.Images
{
    using ImageEntity = DAL.Entities.Media.Images;

    public class GetAllImagesTest
    {
        private readonly Mock<IRepositoryWrapper> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IBlobService> _blobService;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizer;

        public GetAllImagesTest()
        {
            this._mockRepo = new Mock<IRepositoryWrapper>();
            this._mockMapper = new Mock<IMapper>();
            this._blobService = new Mock<IBlobService>();
            this._mockLogger = new Mock<ILoggerService>();
            this._mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Fact]
        public async Task Handle_ReturnsAllImages()
        {
            // Arrange
            this.MockRepositoryAndMapper(this.GetImagesList(), this.GetImagesDTOList());
            var handler = new GetAllImagesHandler(this._mockRepo.Object, this._mockMapper.Object, this._blobService.Object, this._mockLogger.Object, this._mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetAllImagesQuery(), default);

            // Assert
            Assert.Equal(this.GetImagesList().Count, result.Value.Count());
        }

        [Fact]
        public async Task Handle_ReturnsError()
        {
            // Arrange
            this.MockRepositoryAndMapper(new List<ImageEntity.Image>() { }, new List<ImageDTO>() { });
            var expectedError = $"Cannot find any image";
            this._mockLocalizer.Setup(localizer => localizer["CannotFindAnyImage"])
                .Returns(new LocalizedString("CannotFindAnyImage", expectedError));
            var handler = new GetAllImagesHandler(this._mockRepo.Object, this._mockMapper.Object, this._blobService.Object, this._mockLogger.Object, this._mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetAllImagesQuery(), default);

            // Assert
            Assert.Equal(expectedError, result.Errors.Single().Message);
        }

        [Fact]
        public async Task Handle_ReturnsType()
        {
            // Arrange
            this.MockRepositoryAndMapper(this.GetImagesList(), this.GetImagesDTOList());
            var handler = new GetAllImagesHandler(this._mockRepo.Object, this._mockMapper.Object, this._blobService.Object, this._mockLogger.Object, this._mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetAllImagesQuery(), default);

            // Assert
            Assert.IsType<Result<IEnumerable<ImageDTO>>>(result);
        }

        private List<ImageEntity.Image> GetImagesList()
        {
            return new List<ImageEntity.Image>()
            {
                new ImageEntity.Image()
                {
                    Id = 1,
                    BlobName = "https://",
                    MimeType = string.Empty,
                },
                new ImageEntity.Image()
                {
                    Id = 2,
                    BlobName = "https://",
                    MimeType = string.Empty,
                },
            };
        }

        private List<ImageDTO> GetImagesDTOList()
        {
            return new List<ImageDTO>()
            {
                new ImageDTO
                {
                    Id = 1,
                },
                new ImageDTO
                {
                    Id = 2,
                },
            };
        }

        private void MockRepositoryAndMapper(List<ImageEntity.Image> imageList, List<ImageDTO> imageListDTO)
        {
            this._mockRepo.Setup(r => r.ImageRepository.GetAllAsync(
            It.IsAny<Expression<Func<ImageEntity.Image, bool>>>(),
            It.IsAny<Func<IQueryable<ImageEntity.Image>,
            IIncludableQueryable<ImageEntity.Image, object>>>()))
            .ReturnsAsync(imageList);

            this._mockMapper.Setup(x => x.Map<IEnumerable<ImageDTO>>(It.IsAny<IEnumerable<object>>()))
            .Returns(imageListDTO);
        }
    }
}