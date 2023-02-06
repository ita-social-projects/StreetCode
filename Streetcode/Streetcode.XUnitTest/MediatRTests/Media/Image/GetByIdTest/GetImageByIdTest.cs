using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Streetcode.BLL.MediatR.Media.Image.GetById;
using Streetcode.BLL.DTO.Media.Images;
using AutoMapper;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.BLL.Mapping.Media.Images;
using Streetcode.BLL.MediatR.Media.Image.GetById;
using FluentResults;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Streetcode.XUnitTest.MediatR.Media.Image.GetById
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

        public async Task GetImageById_ReturnsSuccessfullyImageById(int id)
        {
            var image = new DAL.Entities.Media.Images.Image
            {
                Id = 1,
                Alt = "Alt1"

            };
            var imageDTO = new ImageDTO
            {
                Id = 1,
                Alt = "Alt1"

            };

            _mockRepo.Setup(r => r.ImageRepository.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<DAL.Entities.Media.Images.Image, bool>>>(),
            It.IsAny<Func<IQueryable<DAL.Entities.Media.Images.Image>,
            IIncludableQueryable<DAL.Entities.Media.Images.Image, object>>>()))
            .ReturnsAsync(image);

            _mockMapper.Setup(x => x.Map<ImageDTO>(It.IsAny<object>()))
            .Returns(imageDTO);

            var handler = new GetImageByIdHandler(_mockRepo.Object, _mockMapper.Object);

            var result = await handler.Handle(new GetImageByIdQuery(id), CancellationToken.None);
            Assert.Equal(id, result.Value.Id);

        }

        [Theory]
        [InlineData(1)]

        public async Task GetImageById_ShouldReturnTypeResultImageDTO(int id)
        {
            var image = new DAL.Entities.Media.Images.Image
            {
                Id = 1,
                Alt = "Alt1"

            };
            var imageDTO = new ImageDTO
            {
                Id = 1,
                Alt = "Alt1"

            };

            _mockRepo.Setup(r => r.ImageRepository.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<DAL.Entities.Media.Images.Image, bool>>>(),
            It.IsAny<Func<IQueryable<DAL.Entities.Media.Images.Image>,
            IIncludableQueryable<DAL.Entities.Media.Images.Image, object>>>()))
            .ReturnsAsync(image);

            _mockMapper.Setup(x => x.Map<ImageDTO>(It.IsAny<object>()))
            .Returns(imageDTO);

            var handler = new GetImageByIdHandler(_mockRepo.Object, _mockMapper.Object);

            var result = await handler.Handle(new GetImageByIdQuery(id), CancellationToken.None);

            Assert.IsType<Result<ImageDTO>>(result);

        }


        [Theory]
        [InlineData(-1)]
        public async Task GetImageById_WithNonExistentValue_ShouldReturnError(int id)
        {
            var emptyImage = (DAL.Entities.Media.Images.Image)null;
            var emptyImageDTO = (ImageDTO)null;
           
            _mockRepo.Setup(r => r.ImageRepository.GetFirstOrDefaultAsync(
             It.IsAny<Expression<Func<DAL.Entities.Media.Images.Image, bool>>>(),
             It.IsAny<Func<IQueryable<DAL.Entities.Media.Images.Image>,
             IIncludableQueryable<DAL.Entities.Media.Images.Image, object>>>()))
             .ReturnsAsync(emptyImage);

            _mockMapper.Setup(x => x.Map<ImageDTO>(It.IsAny<object>()))
            .Returns(emptyImageDTO);
            var error = $"Cannot find a image with corresponding id: {id}";

            var handler = new GetImageByIdHandler(_mockRepo.Object, _mockMapper.Object);

            var result = await handler.Handle(new GetImageByIdQuery(id), CancellationToken.None);

            Assert.Equal(error, result.Errors.First().Message);
        }
    }
}
