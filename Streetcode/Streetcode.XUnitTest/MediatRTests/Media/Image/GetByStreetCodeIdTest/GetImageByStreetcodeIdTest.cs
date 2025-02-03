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
        private readonly Mock<IRepositoryWrapper> mockRepo;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IBlobService> blobService;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> mockLocalizer;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly Mock<ICacheService> mockCache;

        public GetImageByStreetcodeIdTest()
        {
            this.mockRepo = new Mock<IRepositoryWrapper>();
            this.mockMapper = new Mock<IMapper>();
            this.blobService = new Mock<IBlobService>();
            this.mockLogger = new Mock<ILoggerService>();
            this.mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
            this.mockCache = new Mock<ICacheService>();
            this.mockCache
                .Setup(c => c.GetOrSetAsync(It.IsAny<string>(), It.IsAny<Func<Task<Result<IEnumerable<ImageDto>>>>>(), It.IsAny<TimeSpan>()))
                .Returns<string, Func<Task<Result<IEnumerable<ImageDto>>>>, TimeSpan>((key, func, timeSpan) =>
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
            var handler = new GetImageByStreetcodeIdHandler(this.mockRepo.Object, this.mockMapper.Object, this.blobService.Object, this.mockLogger.Object, this.mockLocalizer.Object, this.mockCache.Object);

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
            var handler = new GetImageByStreetcodeIdHandler(this.mockRepo.Object, this.mockMapper.Object, this.blobService.Object, this.mockLogger.Object, this.mockLocalizer.Object, this.mockCache.Object);

            // Act
            var result = await handler.Handle(new GetImageByStreetcodeIdQuery(streetcodeId), default);

            // Assert
            Assert.IsType<Result<IEnumerable<ImageDto>>>(result);
        }

        [Theory]
        [InlineData(-1)]
        public async Task Handle_WithNonExistentId_ReturnsError(int streetcodeId)
        {
            // Arrange
            this.MockRepositoryAndMapper(new List<DAL.Entities.Media.Images.Image>(), new List<ImageDto>());

            var expectedError = $"Cannot find an image with the corresponding streetcode id: {streetcodeId}";
            this.mockLocalizer.Setup(x => x["CannotFindAnImageWithTheCorrespondingStreetcodeId", streetcodeId])
                .Returns(new LocalizedString("CannotFindAnImageWithTheCorrespondingStreetcodeId", expectedError));

            var handler = new GetImageByStreetcodeIdHandler(this.mockRepo.Object, this.mockMapper.Object, this.blobService.Object, this.mockLogger.Object, this.mockLocalizer.Object, this.mockCache.Object);

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

        private List<ImageDto> GetImagesDTOList()
        {
            return new List<ImageDto>()
            {
                new ImageDto
                {
                    Id = 1,
                },
                new ImageDto
                {
                    Id = 2,
                },
            };
        }

        private void MockRepositoryAndMapper(List<DAL.Entities.Media.Images.Image> imageList, List<ImageDto> imageListDTO)
        {
            this.mockRepo
                .Setup(r => r.ImageRepository.GetAllAsync(
                    It.IsAny<Expression<Func<DAL.Entities.Media.Images.Image, bool>>>(),
                    It.IsAny<Func<IQueryable<DAL.Entities.Media.Images.Image>,
                    IIncludableQueryable<DAL.Entities.Media.Images.Image, object>>>()))
                .ReturnsAsync(imageList);

            this.mockMapper
                .Setup(x => x.Map<IEnumerable<ImageDto>>(It.IsAny<IEnumerable<object>>()))
                .Returns(imageListDTO);
        }
    }
}