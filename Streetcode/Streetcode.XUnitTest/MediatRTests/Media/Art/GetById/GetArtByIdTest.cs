using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Media.Art.GetById;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Media.Arts
{
    public class GetArtByIdTest
    {
        private readonly Mock<IRepositoryWrapper> mockRepo;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> mockLocalizerCannotFind;

        public GetArtByIdTest()
        {
            this.mockRepo = new Mock<IRepositoryWrapper>();
            this.mockMapper = new Mock<IMapper>();
            this.mockLogger = new Mock<ILoggerService>();
            this.mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ReturnsArt(int id)
        {
            // Arrange
            this.GetMockRepositoryAndMapper(this.GetArt(), this.GetArtDTO());
            var handler = new GetArtByIdHandler(this.mockRepo.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object);

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
            this.GetMockRepositoryAndMapper(this.GetArt(), this.GetArtDTO());
            var handler = new GetArtByIdHandler(this.mockRepo.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new GetArtByIdQuery(id), CancellationToken.None);

            // Assert
            Assert.IsType<Result<ArtDto>>(result);
        }

        [Theory]
        [InlineData(-1)]
        public async Task Handle_WithNonExistentId_ReturnsError(int id)
        {
            // Arrange
            this.GetMockRepositoryAndMapper(null, null);
            var handler = new GetArtByIdHandler(this.mockRepo.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object);
            var expectedError = $"Cannot find an art with corresponding id: {id}";

            // Act
            var result = await handler.Handle(new GetArtByIdQuery(id), CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors[0].Message);
        }

        private Art GetArt()
        {
            return new Art
            {
                Id = 1,
            };
        }

        private ArtDto GetArtDTO()
        {
            return new ArtDto
            {
                Id = 1,
            };
        }

        private void GetMockRepositoryAndMapper(Art? art, ArtDto? artDTO)
        {
            this.mockRepo
                .Setup(r => r.ArtRepository
                    .GetFirstOrDefaultAsync(
                        It.IsAny<Expression<Func<Art, bool>>>(),
                        It.IsAny<Func<IQueryable<Art>,
                        IIncludableQueryable<Art, object>>>()))
                .ReturnsAsync(art);

            this.mockMapper
                .Setup(x => x.Map<ArtDto?>(It.IsAny<object>()))
                .Returns(artDTO);

            this.mockLocalizerCannotFind.Setup(x => x[It.IsAny<string>(), It.IsAny<object>()])
            .Returns((string key, object[] args) =>
            {
                if (args != null && args.Length > 0 && args[0] is int id)
                {
                    return new LocalizedString(key, $"Cannot find an art with corresponding id: {id}");
                }

                return new LocalizedString(key, "Cannot find an art with unknown id");
            });
        }
    }
}
