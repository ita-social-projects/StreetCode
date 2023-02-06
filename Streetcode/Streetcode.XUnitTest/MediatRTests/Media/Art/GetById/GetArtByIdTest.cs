using Moq;
using Xunit;
using Streetcode.BLL.MediatR.Media.Art.GetById;
using Streetcode.BLL.DTO.Media.Images;
using AutoMapper;
using Streetcode.DAL.Repositories.Interfaces.Base;
using FluentResults;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Streetcode.XUnitTest.MediatR.Media.Art.GetById
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

        public async Task GetArtById_ReturnsSuccessfullyArtById(int id)
        {
            var art = new DAL.Entities.Media.Images.Art
            {
                Id = 1,
                ImageId = 1,
                Description = "Test text 1"
            };
            var artDTO = new ArtDTO
            {
                Id = 1,
                ImageId = 1,
                Description = "Test text 1"

            };

            _mockRepo.Setup(r => r.ArtRepository.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<DAL.Entities.Media.Images.Art, bool>>>(),
            It.IsAny<Func<IQueryable<DAL.Entities.Media.Images.Art>,
            IIncludableQueryable<DAL.Entities.Media.Images.Art, object>>>()))
            .ReturnsAsync(art);

            _mockMapper.Setup(x => x.Map<ArtDTO>(It.IsAny<object>()))
            .Returns(artDTO);

            var handler = new GetArtByIdHandler(_mockRepo.Object, _mockMapper.Object);

            var result = await handler.Handle(new GetArtByIdQuery(id), CancellationToken.None);
            Assert.Equal(id, result.Value.Id);

        }

        [Theory]
        [InlineData(1)]

        public async Task GetArtById_ShouldReturnTypeResultArtDTO(int id)
        {
            var art = new DAL.Entities.Media.Images.Art
            {
                Id = 1,
                ImageId = 1,
                Description = "Test text 1"
            };
            var artDTO = new ArtDTO
            {
                Id = 1,
                ImageId = 1,
                Description = "Test text 1"

            };
            _mockRepo.Setup(r => r.ArtRepository.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<DAL.Entities.Media.Images.Art, bool>>>(),
            It.IsAny<Func<IQueryable<DAL.Entities.Media.Images.Art>,
            IIncludableQueryable<DAL.Entities.Media.Images.Art, object>>>()))
            .ReturnsAsync(art);

            _mockMapper.Setup(x => x.Map<ArtDTO>(It.IsAny<object>()))
            .Returns(artDTO);

            var handler = new GetArtByIdHandler(_mockRepo.Object, _mockMapper.Object);

            var result = await handler.Handle(new GetArtByIdQuery(id), CancellationToken.None);

            Assert.IsType<Result<ArtDTO>>(result);

        }


        [Theory]
        [InlineData(-1)]
        public async Task GetArtById_WithNonExistentValue_ShouldReturnError(int id)
        {
            var emptyArt = (DAL.Entities.Media.Images.Art)null;
            var emptyArtDTO = (ArtDTO)null;

            _mockRepo.Setup(r => r.ArtRepository.GetFirstOrDefaultAsync(
             It.IsAny<Expression<Func<DAL.Entities.Media.Images.Art, bool>>>(),
             It.IsAny<Func<IQueryable<DAL.Entities.Media.Images.Art>,
             IIncludableQueryable<DAL.Entities.Media.Images.Art, object>>>()))
             .ReturnsAsync(emptyArt);
            _mockMapper.Setup(x => x.Map<ArtDTO>(It.IsAny<object>()))
            .Returns(emptyArtDTO);
            var error = $"Cannot find an art with corresponding id: {id}";

            var handler = new GetArtByIdHandler(_mockRepo.Object, _mockMapper.Object);

            var result = await handler.Handle(new GetArtByIdQuery(id), CancellationToken.None);
           

            Assert.Equal(error, result.Errors.First().Message);

        }
    }
}
