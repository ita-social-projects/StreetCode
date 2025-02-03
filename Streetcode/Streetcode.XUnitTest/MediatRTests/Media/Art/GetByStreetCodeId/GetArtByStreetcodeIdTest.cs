using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Media.Art.GetByStreetcodeId;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Media.Arts
{
    public class GetArtByStreetcodeIdTest
    {
        private readonly Mock<IRepositoryWrapper> mockRepo;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IBlobService> blobService;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> mockLocalizer;

        public GetArtByStreetcodeIdTest()
        {
            this.mockRepo = new Mock<IRepositoryWrapper>();
            this.mockMapper = new Mock<IMapper>();
            this.blobService = new Mock<IBlobService>();
            this.mockLogger = new Mock<ILoggerService>();
            this.mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Theory]
        [InlineData(1)]

        public async Task Handle_ReturnsArt(int streetcodeId)
        {
            // Arrange
            this.MockRepositoryAndMapper(this.GetArtsList(), this.GetArtsDTOList());
            var handler = new GetArtsByStreetcodeIdHandler(this.mockRepo.Object, this.mockMapper.Object, this.blobService.Object, this.mockLogger.Object, this.mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetArtsByStreetcodeIdQuery(streetcodeId), CancellationToken.None);

            // Assert
            Assert.Equal(streetcodeId, result.Value.First().Id);
        }

        [Theory]
        [InlineData(1)]

        public async Task Handle_ReturnsEmptyArray(int streetcodeId)
        {
            // Arrange
            this.MockRepositoryAndMapper(new List<Art>(), new List<ArtDto>());
            var handler = new GetArtsByStreetcodeIdHandler(this.mockRepo.Object, this.mockMapper.Object, this.blobService.Object, this.mockLogger.Object, this.mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetArtsByStreetcodeIdQuery(streetcodeId), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<Result<IEnumerable<ArtDto>>>(result),
                () => Assert.IsAssignableFrom<IEnumerable<ArtDto>>(result.Value),
                () => Assert.Empty(result.Value));
        }

        [Theory]
        [InlineData(1)]

        public async Task Handle_ReturnsType(int streetcodeId)
        {
            // Arrange
            this.MockRepositoryAndMapper(this.GetArtsList(), this.GetArtsDTOList());
            var handler = new GetArtsByStreetcodeIdHandler(this.mockRepo.Object, this.mockMapper.Object, this.blobService.Object, this.mockLogger.Object, this.mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetArtsByStreetcodeIdQuery(streetcodeId), CancellationToken.None);

            // Assert
            Assert.IsType<Result<IEnumerable<ArtDto>>>(result);
        }

        private List<Art> GetArtsList()
        {
            return new List<Art>()
            {
                new Art()
                {
                    Id = 1,
                    Image = new DAL.Entities.Media.Images.Image(),
                },
                new Art()
                {
                    Id = 2,
                    Image = new DAL.Entities.Media.Images.Image(),
                },
            };
        }

        private List<ArtDto> GetArtsDTOList()
        {
            return new List<ArtDto>()
            {
                new ArtDto
                {
                    Id = 1,
                    Image = new ImageDto(),
                },
                new ArtDto
                {
                    Id = 2,
                    Image = new ImageDto(),
                },
            };
        }

        private void MockRepositoryAndMapper(List<Art> artList, List<ArtDto> artListDTO)
        {
            this.mockRepo
                .Setup(r => r.ArtRepository
                    .GetAllAsync(
                        It.IsAny<Expression<Func<Art, bool>>>(),
                        It.IsAny<Func<IQueryable<Art>,
                        IIncludableQueryable<Art, object>>>()))
                .ReturnsAsync(artList);

            this.mockMapper.Setup(x => x.Map<IEnumerable<ArtDto>>(It.IsAny<IEnumerable<object>>()))
            .Returns(artListDTO);

            this.mockLocalizer.Setup(x => x[It.IsAny<string>(), It.IsAny<object>()])
            .Returns((string key, object[] args) =>
            {
                if (args != null && args.Length > 0 && args[0] is int streetcodeId)
                {
                    return new LocalizedString(key, $"Cannot find any art with corresponding streetcode id: {streetcodeId}");
                }

                return new LocalizedString(key, "Cannot find any art with corresponding streetcode id");
            });
        }
    }
}
