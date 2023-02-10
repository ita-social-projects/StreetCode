using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.MediatR.Media.Image.GetByStreetcodeId;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Media.Images
{ 
    public class GetImageByStreetcodeIdTest
    {
        private Mock<IRepositoryWrapper> _mockRepo;
        private Mock<IMapper> _mockMapper;

        public GetImageByStreetcodeIdTest()
        {
            _mockRepo = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
        }

        [Theory]
        [InlineData(1)]

        public async Task Handle_ReturnsImage(int streetcodeId)
        {
            // Arrange
            MockRepositoryAndMapper(GetImagesList(), GetImagesDTOList());
            var handler = new GetImageByStreetcodeIdQueryHandler(_mockRepo.Object, _mockMapper.Object);

            // Act
            var result = await handler.Handle(new GetImageByStreetcodeIdQuery(streetcodeId), CancellationToken.None);

            //Assert
            Assert.Equal(streetcodeId, result.Value.First().Id);

        }

        [Theory]
        [InlineData(1)]

        public async Task Handle_ReturnsType(int streetcodeId)
        {
            // Arrange
            MockRepositoryAndMapper(GetImagesList(), GetImagesDTOList());
            var handler = new GetImageByStreetcodeIdQueryHandler(_mockRepo.Object, _mockMapper.Object);

            // Act
            var result = await handler.Handle(new GetImageByStreetcodeIdQuery(streetcodeId), CancellationToken.None);

            // Assert
            Assert.IsType<Result<IEnumerable<ImageDTO>>>(result);
        }


        [Theory]
        [InlineData(-1)]
        public async Task Handle_WithNonExistentId_ReturnsError(int streetcodeId)
        {
            // Arrange
            MockRepositoryAndMapper(null, null);
            var handler = new GetImageByStreetcodeIdQueryHandler(_mockRepo.Object, _mockMapper.Object);
            var expectedError = $"Cannot find an image with the corresponding streetcode id: {streetcodeId}";

            // Act
            var result = await handler.Handle(new GetImageByStreetcodeIdQuery(streetcodeId), CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors.First().Message);
        }

        private List<Image> GetImagesList()
        {
            return new List<Image>()
            {
                new Image()
                {
                    Id = 1,
                    Title= "Title1",
                    Alt = "Alt1",
                    Url = "https://",

                },
                new Image()
                {
                    Id = 2,
                    Title= "Title2",
                    Alt = "Alt2",
                    Url = "https://",
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
                    Alt = "Alt1",
                },
                new ImageDTO
                {
                    Id = 2,
                    Alt = "Alt2",
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