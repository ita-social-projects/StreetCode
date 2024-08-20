using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Cache;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Media.Image.GetByStreetcodeId;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Media.Images
{
    public class GetImageByStreetcodeIdTest
    {
        private readonly Mock<IRepositoryWrapper> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IBlobService> _blobService;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizer;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<ICacheService> _mockCache;

        public GetImageByStreetcodeIdTest()
        {
            this._mockRepo = new Mock<IRepositoryWrapper>();
            this._mockMapper = new Mock<IMapper>();
            this._blobService = new Mock<IBlobService>();
            this._mockLogger = new Mock<ILoggerService>();
            this._mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
            this._mockCache = new Mock<ICacheService>();
            this._mockCache
                .Setup(c => c.GetOrSetAsync(It.IsAny<string>(), It.IsAny<Func<Task<Result<IEnumerable<ImageDTO>>>>>(), It.IsAny<TimeSpan>()))
                .Returns<string, Func<Task<Result<IEnumerable<ImageDTO>>>>, TimeSpan>((key, func, timeSpan) =>
                {
                    return func();
                });
        }

        [Theory]
        [InlineData(1)]

        public async Task Handle_ReturnsImage(int streetcodeId)
        {
            // Arrange
            this.MockRepositoryAndMapper(this.GetImagesList(), this.GetImagesDTOList());
            var handler = new GetImageByStreetcodeIdHandler(this._mockRepo.Object, this._mockMapper.Object, this._blobService.Object, this._mockLogger.Object, this._mockLocalizer.Object, this._mockCache.Object);

            // Act
            var result = await handler.Handle(new GetImageByStreetcodeIdQuery(streetcodeId), CancellationToken.None);

            // Assert
            Assert.Equal(streetcodeId, result.Value.First().Id);
        }

        [Theory]
        [InlineData(1)]

        public async Task Handle_ReturnsType(int streetcodeId)
        {
            // Arrange
            this.MockRepositoryAndMapper(this.GetImagesList(), this.GetImagesDTOList());
            var handler = new GetImageByStreetcodeIdHandler(this._mockRepo.Object, this._mockMapper.Object, this._blobService.Object, this._mockLogger.Object, this._mockLocalizer.Object, this._mockCache.Object);

            // Act
            var result = await handler.Handle(new GetImageByStreetcodeIdQuery(streetcodeId), default);

            // Assert
            Assert.IsType<Result<IEnumerable<ImageDTO>>>(result);
        }

        [Theory]
        [InlineData(-1)]
        public async Task Handle_WithNonExistentId_ReturnsError(int streetcodeId)
        {
            // Arrange
            this.MockRepositoryAndMapper(new List<DAL.Entities.Media.Images.Image>(), new List<ImageDTO>());

            var expectedError = $"Cannot find an image with the corresponding streetcode id: {streetcodeId}";
            this._mockLocalizer.Setup(x => x["CannotFindAnImageWithTheCorrespondingStreetcodeId", streetcodeId])
                .Returns(new LocalizedString("CannotFindAnImageWithTheCorrespondingStreetcodeId", expectedError));

            var handler = new GetImageByStreetcodeIdHandler(this._mockRepo.Object, this._mockMapper.Object, this._blobService.Object, this._mockLogger.Object, this._mockLocalizer.Object, this._mockCache.Object);

            // Act
            var result = await handler.Handle(new GetImageByStreetcodeIdQuery(streetcodeId), CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors[0].Message);
        }

        private List<DAL.Entities.Media.Images.Image> GetImagesList()
        {
            return new List<DAL.Entities.Media.Images.Image>()
            {
                new DAL.Entities.Media.Images.Image()
                {
                    Id = 1,
                },
                new DAL.Entities.Media.Images.Image()
                {
                    Id = 2,
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

        private void MockRepositoryAndMapper(List<DAL.Entities.Media.Images.Image> imageList, List<ImageDTO> imageListDTO)
        {
            this._mockRepo
                .Setup(r => r.ImageRepository.GetAllAsync(
                    It.IsAny<Expression<Func<DAL.Entities.Media.Images.Image, bool>>>(),
                    It.IsAny<Func<IQueryable<DAL.Entities.Media.Images.Image>,
                    IIncludableQueryable<DAL.Entities.Media.Images.Image, object>>>()))
                .ReturnsAsync(imageList);

            this._mockMapper
                .Setup(x => x.Map<IEnumerable<ImageDTO>>(It.IsAny<IEnumerable<object>>()))
                .Returns(imageListDTO);
        }
    }
}