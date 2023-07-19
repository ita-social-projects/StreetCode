using Moq;
using Streetcode.BLL.MediatR.Media.Image.GetAll;
using Streetcode.BLL.DTO.Media.Images;
using AutoMapper;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using FluentResults;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.SharedResource;

namespace Streetcode.XUnitTest.MediatRTests.Media.Images
{
    public class GetAllImagesTest
    {
        private readonly Mock<IRepositoryWrapper> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IBlobService> _blobService;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizer;

        public GetAllImagesTest()
        {
            _mockRepo = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _blobService = new Mock<IBlobService>();
            _mockLogger = new Mock<ILoggerService>();
        }

        [Fact]
        public async Task Handle_ReturnsAllImages()
        {
            // Arrange
            MockRepositoryAndMapper(GetImagesList(), GetImagesDTOList());
            var handler = new GetAllImagesHandler(_mockRepo.Object, _mockMapper.Object, _blobService.Object, _mockLogger.Object, _mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetAllImagesQuery(), default);

            // Assert
            Assert.Equal(GetImagesList().Count(), result.Value.Count());
        }


        [Fact]
        public async Task Handle_ReturnsZero()
        {
            //Arrange
            MockRepositoryAndMapper(new List<Image>() { }, new List<ImageDTO>() { });
            var handler = new GetAllImagesHandler(_mockRepo.Object, _mockMapper.Object, _blobService.Object, _mockLocalizer.Object, _mockLogger.Object);
            int expectedResult = 0;

            //Act
            var result = await handler.Handle(new GetAllImagesQuery(), default);

            //Assert
            Assert.Equal(expectedResult, result.Value.Count());

        }

        [Fact]
        public async Task Handle_ReturnsType()
        {
            //Arrange
            MockRepositoryAndMapper(GetImagesList(), GetImagesDTOList());
            var handler = new GetAllImagesHandler(_mockRepo.Object, _mockMapper.Object, _blobService.Object, _mockLogger.Object, _mockLocalizer.Object);

            //Act
            var result = await handler.Handle(new GetAllImagesQuery(), default);

            //Assert
            Assert.IsType<Result<IEnumerable<ImageDTO>>>(result);
        }


        private List<Image> GetImagesList()
        {
            return new List<Image>()
            {
                new Image()
                {
                    Id = 1,
                    BlobName = "https://",
                    MimeType = ""

                },
                new Image()
                {
                    Id = 2,
                    BlobName = "https://",
                    MimeType = ""
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

        private void MockRepositoryAndMapper(List<Image> ImageList, List<ImageDTO> ImageListDTO)
        {
            _mockRepo.Setup(r => r.ImageRepository.GetAllAsync(
            It.IsAny<Expression<Func<Image, bool>>>(),
            It.IsAny<Func<IQueryable<Image>,
            IIncludableQueryable<Image, object>>>()))
            .ReturnsAsync(ImageList);

            _mockMapper.Setup(x => x.Map<IEnumerable<ImageDTO>>(It.IsAny<IEnumerable<object>>()))
            .Returns(ImageListDTO);
        }
    }
}