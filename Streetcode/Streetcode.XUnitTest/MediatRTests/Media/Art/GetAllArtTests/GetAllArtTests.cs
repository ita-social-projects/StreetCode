using Moq;
using Streetcode.BLL.MediatR.Media.Art.GetAll;
using Streetcode.BLL.DTO.Media.Images;
using AutoMapper;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using FluentResults;

namespace Streetcode.XUnitTest.MediatRTests.Media.Art.GetAllArtTests
{

    public class GetAllImageTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;

        public GetAllImageTests()
        {
            _mockRepo = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
        }

        [Fact]
        public async Task GetAllArtTests_ShouldReturnAllArts()
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
                    Description = "Test text 3"

                }
           };
            var allArtsDTOlist = new List<ArtDTO>()
            {
                new ArtDTO
                {
                    Id = 3,
                    ImageId = 3,
                    Description = "Test text 3"

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
            .Returns(allArtsDTOlist);

            var handler = new GetAllArtsHandler(_mockRepo.Object, _mockMapper.Object);

            var result = await handler.Handle(new GetAllArtsQuery(), default);

            Assert.Equal(allArtsDTOlist.Count(), result.Value.Count());
        }


        [Fact]
        public async Task GetAllArtTests_ShouldReturnNotNull()
        {
            var emptyListOfArts = (List<DAL.Entities.Media.Images.Art>)null;
            var emptyListOfArtsDTO = (List<ArtDTO>)null;

            _mockRepo.Setup(r => r.ArtRepository.GetAllAsync(
            It.IsAny<Expression<Func<DAL.Entities.Media.Images.Art, bool>>>(),
            It.IsAny<Func<IQueryable<DAL.Entities.Media.Images.Art>,
            IIncludableQueryable<DAL.Entities.Media.Images.Art, object>>>()))
            .ReturnsAsync(emptyListOfArts);

            _mockMapper.Setup(x => x.Map<IEnumerable<ArtDTO>>(It.IsAny<IEnumerable<object>>()))
             .Returns(emptyListOfArtsDTO);


            var handler = new GetAllArtsHandler(_mockRepo.Object, _mockMapper.Object);

            var result = await handler.Handle(new GetAllArtsQuery(), default);

            Assert.NotNull(result.Value);

        }


        [Fact]
        public async Task GetAllArtTests_ShouldReturnTypeResultIEnumerableArtDTO()
        {
            var allArtsList = new List<DAL.Entities.Media.Images.Art>()
            {
                new DAL.Entities.Media.Images.Art()
                {
                    Id = 1,
                    ImageId = 1,
                    Description = "Test text 1"

                }
            };
            var allArtsDTOlist = new List<ArtDTO>()
            {
                new ArtDTO
                {
                    Id = 3,
                    ImageId = 3,
                    Description = "Test text 3"

                }
            };

           _mockRepo.Setup(r => r.ArtRepository.GetAllAsync(
           It.IsAny<Expression<Func<DAL.Entities.Media.Images.Art, bool>>>(),
           It.IsAny<Func<IQueryable<DAL.Entities.Media.Images.Art>,
           IIncludableQueryable<DAL.Entities.Media.Images.Art, object>>>()))
           .ReturnsAsync(allArtsList);

           _mockMapper.Setup(x => x.Map<IEnumerable<ArtDTO>>(It.IsAny<IEnumerable<object>>()))
             .Returns(allArtsDTOlist);

            var handler = new GetAllArtsHandler(_mockRepo.Object, _mockMapper.Object);

            var result = await handler.Handle(new GetAllArtsQuery(), default);


            Assert.IsType<Result<IEnumerable<ArtDTO>>>(result);
        }


    }


}
