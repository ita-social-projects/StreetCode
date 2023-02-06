using Moq;
using Streetcode.BLL.MediatR.Media.Image.GetAll;
using Streetcode.BLL.DTO.Media.Images;
using AutoMapper;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using FluentResults;

namespace Streetcode.XUnitTest.MediatRTests.Media.Image.GetAllImageTests
{

    public class GetAllImageTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;

        public GetAllImageTests()
        {
            _mockRepo= new Mock<IRepositoryWrapper>();
           _mockMapper= new Mock<IMapper>();
        }

        [Fact]
        public async Task GetAllImageTests_ShouldReturnAllImages()
        {
            var allImagesList = new List<DAL.Entities.Media.Images.Image>()
            {
                new DAL.Entities.Media.Images.Image
                {
                    Id = 1,
                    Alt = "Alt1"
                },

                new DAL.Entities.Media.Images.Image
                {
                   Id = 2,
                   Alt = "Alt2"
                }
            };

            var allImagesDTOList = new List<ImageDTO>()
            {
                new ImageDTO
                {
                   Id = 1,
                   Alt = "Alt1"
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

            var handler = new GetAllImagesHandler(_mockRepo.Object, _mockMapper.Object);
            
            var result = await handler.Handle(new GetAllImagesQuery(), default);

            Assert.Equal(allImagesDTOList.Count(), result.Value.Count());
        }


        [Fact]
        public async Task GetAllImageTests_ShouldBeNotNull()
        {
            var emptyListOfImages = (List<DAL.Entities.Media.Images.Image>)null;
            var emptyListOfImagesDTO = (List<ImageDTO>)null;

            _mockRepo.Setup(r => r.ImageRepository.GetAllAsync(
            It.IsAny<Expression<Func<DAL.Entities.Media.Images.Image, bool>>>(),
            It.IsAny<Func<IQueryable<DAL.Entities.Media.Images.Image>,
            IIncludableQueryable<DAL.Entities.Media.Images.Image, object>>>()))
             .ReturnsAsync(emptyListOfImages);

            _mockMapper.Setup(x => x.Map<IEnumerable<ImageDTO>>(It.IsAny<IEnumerable<object>>()))
             .Returns(emptyListOfImagesDTO);

            var handler = new GetAllImagesHandler(_mockRepo.Object, _mockMapper.Object);

            var result = await handler.Handle(new GetAllImagesQuery(), default);

            Assert.NotNull(result.Value);
        }


        [Fact]
        public async Task GetAllImageTests_ShouldReturnTypeResultIEnumerableImageDTO()
        {
            var allImagesList = new List<DAL.Entities.Media.Images.Image>()
            {
                new DAL.Entities.Media.Images.Image
                {
                    Id = 1,
                    Alt = "Alt1"
                },

                new DAL.Entities.Media.Images.Image
                {
                   Id = 2,
                   Alt = "Alt2"
                }
            };
            var allImagesDTOList = new List<ImageDTO>()
            {
                new ImageDTO
                {
                   Id = 1,
                   Alt = "Alt1"
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

            var handler = new GetAllImagesHandler(_mockRepo.Object, _mockMapper.Object);

            var result = await handler.Handle(new GetAllImagesQuery(), default);

            Assert.IsType<Result<IEnumerable<ImageDTO>>>(result);
        }


    }


}
