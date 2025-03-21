﻿using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Media.Image.GetById;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Media.Images
{
    public class GetImageByIdTest
    {
        private readonly Mock<IRepositoryWrapper> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IBlobService> _blobService;
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
            GetMockRepositoryAndMapper(GetImage(), GetImageDto());
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
            GetMockRepositoryAndMapper(GetImage(), GetImageDto());
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
            Assert.Equal(expectedError, result.Errors[0].Message);
        }

        private static DAL.Entities.Media.Images.Image GetImage()
        {
            return new DAL.Entities.Media.Images.Image()
            {
                Id = 2,
                BlobName = "https://",
                MimeType = string.Empty,
            };
        }

        private static ImageDTO GetImageDto()
        {
            return new ImageDTO
            {
                Id = 1,
            };
        }

        private void GetMockRepositoryAndMapper(DAL.Entities.Media.Images.Image? image, ImageDTO? imageDto)
        {
            _mockRepo
                .Setup(r => r.ImageRepository.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<DAL.Entities.Media.Images.Image, bool>>>(),
                    It.IsAny<Func<IQueryable<DAL.Entities.Media.Images.Image>,
                    IIncludableQueryable<DAL.Entities.Media.Images.Image, object>>>()))
                .ReturnsAsync(image);

            _mockMapper
                .Setup(x => x.Map<ImageDTO?>(It.IsAny<object>()))
                .Returns(imageDto);

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