using Moq;
using Xunit;
using Streetcode.BLL.MediatR.Media.Art.GetById;
using Streetcode.BLL.DTO.Media.Images;
using AutoMapper;
using Streetcode.DAL.Repositories.Interfaces.Base;
using FluentResults;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using Streetcode.DAL.Entities.Media.Images;

namespace Streetcode.XUnitTest.MediatRTests.Media.Arts
{
    public class GetArtByIdTest
    {
        private Mock<IRepositoryWrapper> _mockRepo;
        private Mock<IMapper> _mockMapper;

        public GetArtByIdTest()
        {
            _mockRepo = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ReturnsArt(int id)
        {
            // Arrange
            GetMockRepositoryAndMapper(GetArt(), GetArtDTO());
            var handler = new GetArtByIdHandler(_mockRepo.Object, _mockMapper.Object);

            // Act
            var result = await handler.Handle(new GetArtByIdQuery(id), CancellationToken.None);

            // Assert
            Assert.Equal(id, result.Value.Id);
        }

        [Theory]
        [InlineData(1)]

        public async Task Handle_ReturnsType(int id)
        {
            // Arrange
            GetMockRepositoryAndMapper(GetArt(), GetArtDTO());
            var handler = new GetArtByIdHandler(_mockRepo.Object, _mockMapper.Object);

            // Act
            var result = await handler.Handle(new GetArtByIdQuery(id), CancellationToken.None);

            // Assert
            Assert.IsType<Result<ArtDTO>>(result);
        }

        [Theory]
        [InlineData(-1)]
        public async Task Handle_WithNonExistentId_ReturnsError(int id)
        {
            // Arrange
            GetMockRepositoryAndMapper(null, null);
            var handler = new GetArtByIdHandler(_mockRepo.Object, _mockMapper.Object);
            var expectedError = $"Cannot find an art with corresponding id: {id}";

            // Act
            var result = await handler.Handle(new GetArtByIdQuery(id), CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors.First().Message);

        }

        private Art GetArt()
        {
            return new Art
            {
                Id = 1
            };
        }

        private ArtDTO GetArtDTO()
        {
            return new ArtDTO
            {
                Id = 1
            };
        }

        private void GetMockRepositoryAndMapper(Art art, ArtDTO artDTO)
        {
            _mockRepo.Setup(r => r.ArtRepository.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<Art, bool>>>(),
            It.IsAny<Func<IQueryable<Art>,
            IIncludableQueryable<Art, object>>>()))
            .ReturnsAsync(art);

            _mockMapper.Setup(x => x.Map<ArtDTO>(It.IsAny<object>()))
            .Returns(artDTO);
        }
    }
}
