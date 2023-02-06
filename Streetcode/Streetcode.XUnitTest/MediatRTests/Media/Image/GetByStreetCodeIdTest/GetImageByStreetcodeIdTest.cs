using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.MediatR.Media.Image.GetByStreetcodeId;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;

namespace Streetcode.XUnitTest.MediatR.Media.Image.GetByStreetCodeId
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

        public async Task GetImageByStreetCodeId_ReturnsSuccessfullyImageById(int streetcodeId)
        {
            var allImagesList = new List<DAL.Entities.Media.Images.Image>()
            {
                new DAL.Entities.Media.Images.Image()
                {
                    Id = 1,
                    Alt = "Alt1",
                },
                new DAL.Entities.Media.Images.Image()
                {
                    Id = 2,
                    Alt = "Alt2",
                }
            };
            var allImagesDTOList = new List<ImageDTO>()
            {
                new ImageDTO
                {
                    Id = 1,
                    Alt = "Alt2",
                },
                new ImageDTO
                {
                    Id = 2,
                    Alt = "Alt2"
                }
            };

             
            _mockRepo.Setup(r => r.ImageRepository.GetAllAsync(
            It.IsAny<Expression<Func<DAL.Entities.Media.Images.Image, bool>>>(),
            It.IsAny<Func<IQueryable<DAL.Entities.Media.Images.Image>,
            IIncludableQueryable<DAL.Entities.Media.Images.Image, object>>>()))
            .ReturnsAsync(allImagesList);

            _mockMapper.Setup(x => x.Map<IEnumerable<ImageDTO>>(It.IsAny<IEnumerable<object>>()))
             .Returns(allImagesDTOList);
                

            var handler = new GetImageByStreetcodeIdQueryHandler(_mockRepo.Object, _mockMapper.Object);

            var result = await handler.Handle(new GetImageByStreetcodeIdQuery(streetcodeId), CancellationToken.None);
            
            Assert.Equal(allImagesDTOList.Count(), result.Value.Count());

        }

        [Theory]
        [InlineData(1)]

        public async Task GetImageByStreetCodeId_ShouldReturnTypeResultIEnumerableImageDTO(int streetcodeId)
        {
            var allImagesList = new List<DAL.Entities.Media.Images.Image>()
            {
                new DAL.Entities.Media.Images.Image()
                {
                    Id = 1,
                    Alt = "Alt1",
                },
                new DAL.Entities.Media.Images.Image()
                {
                    Id = 2,
                    Alt = "Alt2",
                }
            };
            var allImagesDTOList = new List<ImageDTO>()
            {
                new ImageDTO
                {
                    Id = 1,
                    Alt = "Alt2",
                },
                new ImageDTO
                {
                    Id = 2,
                    Alt = "Alt2"
                }
            };


            _mockRepo.Setup(r => r.ImageRepository.GetAllAsync(
            It.IsAny<Expression<Func<DAL.Entities.Media.Images.Image, bool>>>(),
            It.IsAny<Func<IQueryable<DAL.Entities.Media.Images.Image>,
            IIncludableQueryable<DAL.Entities.Media.Images.Image, object>>>()))
            .ReturnsAsync(allImagesList);

            _mockMapper.Setup(x => x.Map<IEnumerable<ImageDTO>>(It.IsAny<IEnumerable<object>>()))
             .Returns(allImagesDTOList);


            var handler = new GetImageByStreetcodeIdQueryHandler(_mockRepo.Object, _mockMapper.Object);

            var result = await handler.Handle(new GetImageByStreetcodeIdQuery(streetcodeId), CancellationToken.None);

            Assert.IsType<Result<IEnumerable<ImageDTO>>>(result);

        }

        [Theory]
        [InlineData(-1)]
        public async Task GetImageByStreetCodeId_WithNonExistentValue_ShouldReturnError(int streetcodeId)
        {
            var allImagesList = (List<DAL.Entities.Media.Images.Image>)null;
            var allImagesDTOList = (List<ImageDTO>)null;

            _mockRepo.Setup(r => r.ImageRepository.GetAllAsync(
            It.IsAny<Expression<Func<DAL.Entities.Media.Images.Image, bool>>>(),
            It.IsAny<Func<IQueryable<DAL.Entities.Media.Images.Image>,
            IIncludableQueryable<DAL.Entities.Media.Images.Image, object>>>()))
            .ReturnsAsync(allImagesList);

            _mockMapper.Setup(x => x.Map<IEnumerable<ImageDTO>>(It.IsAny<IEnumerable<object>>()))
             .Returns(allImagesDTOList);
            var error = $"Cannot find an image with the corresponding streetcode id: {streetcodeId}";

            var handler = new GetImageByStreetcodeIdQueryHandler(_mockRepo.Object, _mockMapper.Object);

            var result = await handler.Handle(new GetImageByStreetcodeIdQuery(streetcodeId), CancellationToken.None);

            Assert.Equal(error, result.Errors.First().Message);
        }
    }
}
