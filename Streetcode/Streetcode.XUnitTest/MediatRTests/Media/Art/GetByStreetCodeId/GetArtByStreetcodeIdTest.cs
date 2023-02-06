using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.MediatR.Media.Art.GetByStreetcodeId;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;

namespace Streetcode.XUnitTest.MediatR.Media.Art.GetByStreetCodeId
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

        public async Task GetArtByStreetCodeId_ReturnsSuccessfullyArtById(int streetcodeId)
        {
            var allArtsList = new List<DAL.Entities.Media.Images.Art>()
            {
                new DAL.Entities.Media.Images.Art()
                {
                    Id = 1,
                    ImageId = 1,
                    Description = "Test text 1"
                },
                new DAL.Entities.Media.Images.Art()
                {
                    Id = 2,
                    ImageId = 2,
                    Description = "Test text 2"
                }
            };
            var allArtsDTOList = new List<ArtDTO>()
            {
                new ArtDTO
                {
                   Id = 1,
                   ImageId = 1,
                   Description = "Test text 1"

                },
                new ArtDTO
                {
                   Id = 2,
                   ImageId = 2,
                   Description = "Test text 2"
                }
            };

             
            _mockRepo.Setup(r => r.ArtRepository.GetAllAsync(
            It.IsAny<Expression<Func<DAL.Entities.Media.Images.Art, bool>>>(),
            It.IsAny<Func<IQueryable<DAL.Entities.Media.Images.Art>,
            IIncludableQueryable<DAL.Entities.Media.Images.Art, object>>>()))
            .ReturnsAsync(allArtsList);

            _mockMapper.Setup(x => x.Map<IEnumerable<ArtDTO>>(It.IsAny<IEnumerable<object>>()))
             .Returns(allArtsDTOList);
                
            var handler = new GetArtByStreetcodeIdQueryHandler(_mockRepo.Object, _mockMapper.Object);

            var result = await handler.Handle(new GetArtByStreetcodeIdQuery(streetcodeId), CancellationToken.None);
            Assert.Equal(streetcodeId, result.Value.First().Id);

        }

        [Theory]
        [InlineData(1)]

        public async Task GetArtByStreetCodeId_ShouldReturnTypeResultIEnumerableArtDTO(int streetcodeId)
        {
            var allArtsList = new List<DAL.Entities.Media.Images.Art>()
            {
                new DAL.Entities.Media.Images.Art()
                {
                    Id = 1,
                    ImageId = 1,
                    Description = "Test text 1"

                },
                new DAL.Entities.Media.Images.Art()
                {
                    Id = 2,
                    ImageId = 2,
                    Description = "Test text 2"

                }
            };
            var allArtsDTOList = new List<ArtDTO>()
            {
                new ArtDTO
                {
                   Id = 1,
                   ImageId = 1,
                   Description = "Test text 1"

                },
                new ArtDTO
                {
                   Id = 2,
                   ImageId = 2,
                   Description = "Test text 3"

                }
            };


            _mockRepo.Setup(r => r.ArtRepository.GetAllAsync(
            It.IsAny<Expression<Func<DAL.Entities.Media.Images.Art, bool>>>(),
            It.IsAny<Func<IQueryable<DAL.Entities.Media.Images.Art>,
            IIncludableQueryable<DAL.Entities.Media.Images.Art, object>>>()))
             .ReturnsAsync(allArtsList);

            _mockMapper.Setup(x => x.Map<IEnumerable<ArtDTO>>(It.IsAny<IEnumerable<object>>()))
             .Returns(allArtsDTOList);

            var handler = new GetArtByStreetcodeIdQueryHandler(_mockRepo.Object, _mockMapper.Object);

            var result = await handler.Handle(new GetArtByStreetcodeIdQuery(streetcodeId), CancellationToken.None);

            Assert.IsType<Result<IEnumerable<ArtDTO>>>(result);

        }


        [Theory]
        [InlineData(-1)]
        public async Task GetArtByStreetCodeId_WithNonExistentValue_ShouldReturnError(int streetcodeId)
        {
            var emptyArts = (List<DAL.Entities.Media.Images.Art>)null;
            var emptyArtsDTO = (List<ArtDTO>)null;

            _mockRepo.Setup(r => r.ArtRepository.GetAllAsync(
            It.IsAny<Expression<Func<DAL.Entities.Media.Images.Art, bool>>>(),
            It.IsAny<Func<IQueryable<DAL.Entities.Media.Images.Art>,
            IIncludableQueryable<DAL.Entities.Media.Images.Art, object>>>()))
            .ReturnsAsync(emptyArts);

            _mockMapper.Setup(x => x.Map<IEnumerable<ArtDTO>>(It.IsAny<IEnumerable<object>>()))
             .Returns(emptyArtsDTO);
            var error = $"Cannot find an art with corresponding streetcode id: {streetcodeId}";

            var handler = new GetArtByStreetcodeIdQueryHandler(_mockRepo.Object, _mockMapper.Object);

            var result = await handler.Handle(new GetArtByStreetcodeIdQuery(streetcodeId), CancellationToken.None);
          
            Assert.Equal(error, result.Errors.First().Message);
          

        }
    }
}
