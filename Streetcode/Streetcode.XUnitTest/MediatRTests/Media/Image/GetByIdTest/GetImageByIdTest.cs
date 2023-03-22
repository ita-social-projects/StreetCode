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

namespace Streetcode.XUnitTest.MediatRTests.Media.Images
{
    public class GetImageByIdTest
    {
        private Mock<IRepositoryWrapper> _mockRepo;
        private Mock<IMapper> _mockMapper;

        public GetImageByIdTest()
        {
            _mockRepo = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ReturnsImage(int id)
        {
            // Arrange
            GetMockRepositoryAndMapper(GetImage(), GetImageDTO());
            var handler = new GetImageByIdHandler(_mockRepo.Object, _mockMapper.Object);

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
            var handler = new GetImageByIdHandler(_mockRepo.Object, _mockMapper.Object);

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
            var handler = new GetImageByIdHandler(_mockRepo.Object, _mockMapper.Object);
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
                Title = "Title2",
                Alt = "Alt2",
                BlobName = "https://",
                MimeType = ""
            };
        }

        private ImageDTO GetImageDTO()
        {
            return new ImageDTO
            {
                Id = 1,
                Alt = "Alt2",
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
        }
    }
}