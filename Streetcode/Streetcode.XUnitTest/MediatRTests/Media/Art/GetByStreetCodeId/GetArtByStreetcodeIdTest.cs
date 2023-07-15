using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Media.Art.GetByStreetcodeId;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Media.Arts
{
  public class GetArtByStreetcodeIdTest
    {
        private readonly Mock<IRepositoryWrapper> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IBlobService> _blobService;
        private readonly Mock<ILoggerService> _mockLogger;
        public GetArtByStreetcodeIdTest()
        {
            _mockRepo = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _blobService = new Mock<IBlobService>();
            _mockLogger = new Mock<ILoggerService>();
        }

        [Theory]
        [InlineData(1)]

        public async Task Handle_ReturnsArt(int streetcodeId)
        {
            // Arrange
            MockRepositoryAndMapper(GetArtsList(), GetArtsDTOList());
            var handler = new GetArtsByStreetcodeIdHandler(_mockRepo.Object, _mockMapper.Object, _blobService.Object, _mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetArtsByStreetcodeIdQuery(streetcodeId), CancellationToken.None);

            //Assert
            Assert.Equal(streetcodeId, result.Value.First().Id);

        }

        [Theory]
        [InlineData(1)]

        public async Task Handle_ReturnsType(int streetcodeId)
        {
            // Arrange
            MockRepositoryAndMapper(GetArtsList(), GetArtsDTOList());
            var handler = new GetArtsByStreetcodeIdHandler(_mockRepo.Object, _mockMapper.Object, _blobService.Object, _mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetArtsByStreetcodeIdQuery(streetcodeId), CancellationToken.None);

            // Assert
            Assert.IsType<Result<IEnumerable<ArtDTO>>>(result);
        }


        [Theory]
        [InlineData(-1)]
        public async Task Handle_WithNonExistentId_ReturnsError(int streetcodeId)
        {
            // Arrange
            MockRepositoryAndMapper(null, null);
            var handler = new GetArtsByStreetcodeIdHandler(_mockRepo.Object, _mockMapper.Object, _blobService.Object, _mockLogger.Object);
            var expectedError = $"Cannot find any art with corresponding streetcode id: {streetcodeId}";

            // Act
            var result = await handler.Handle(new GetArtsByStreetcodeIdQuery(streetcodeId), CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors.First().Message);
        }

        private List<Art> GetArtsList()
        {
            return new List<Art>()
            {
                new Art()
                {
                    Id = 1,
                    Image = new Image()
                },
                new Art()
                {
                    Id = 2,
                    Image = new Image()
                },
            };
        }

        private List<ArtDTO> GetArtsDTOList()
        {
            return new List<ArtDTO>()
            {
                new ArtDTO
                {
                    Id = 1,
                    Image = new ImageDTO()
                },
                new ArtDTO
                {
                    Id = 2,
                    Image = new ImageDTO()
                },
            };
        }

        private void MockRepositoryAndMapper(List<Art> artList, List<ArtDTO> artListDTO)
        {
            _mockRepo.Setup(r => r.ArtRepository.GetAllAsync(
            It.IsAny<Expression<Func<Art, bool>>>(),
            It.IsAny<Func<IQueryable<Art>,
            IIncludableQueryable<Art, object>>>()))
            .ReturnsAsync(artList);

            _mockMapper.Setup(x => x.Map<IEnumerable<ArtDTO>>(It.IsAny<IEnumerable<object>>()))
            .Returns(artListDTO);
        }
    }
}
