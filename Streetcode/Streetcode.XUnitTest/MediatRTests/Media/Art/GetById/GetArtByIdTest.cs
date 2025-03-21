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
        private readonly Mock<IRepositoryWrapper> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizerCannotFind;

        public GetArtByIdTest()
        {
            _mockRepo = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerService>();
            _mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ReturnsArt(int id)
        {
            // Arrange
            GetMockRepositoryAndMapper(GetArt(), GetArtDto());
            var handler = new GetArtByIdHandler(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);

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
            GetMockRepositoryAndMapper(GetArt(), GetArtDto());
            var handler = new GetArtByIdHandler(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);

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
            var handler = new GetArtByIdHandler(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);
            var expectedError = $"Cannot find an art with corresponding id: {id}";

            // Act
            var result = await handler.Handle(new GetArtByIdQuery(id), CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors[0].Message);
        }

        private static Art GetArt()
        {
            return new Art
            {
                Id = 1,
            };
        }

        private static ArtDTO GetArtDto()
        {
            return new ArtDTO
            {
                Id = 1,
            };
        }

        private void GetMockRepositoryAndMapper(Art? art, ArtDTO? artDto)
        {
            _mockRepo
                .Setup(r => r.ArtRepository
                    .GetFirstOrDefaultAsync(
                        It.IsAny<Expression<Func<Art, bool>>>(),
                        It.IsAny<Func<IQueryable<Art>,
                        IIncludableQueryable<Art, object>>>()))
                .ReturnsAsync(art);

            _mockMapper
                .Setup(x => x.Map<ArtDTO?>(It.IsAny<object>()))
                .Returns(artDto);

            _mockLocalizerCannotFind.Setup(x => x[It.IsAny<string>(), It.IsAny<object>()])
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
