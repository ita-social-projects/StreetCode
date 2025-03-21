using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Media.Art.GetAll;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Repositories.Interfaces.Base;

using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Media.Arts
{
    public class GetAllArtsTest
    {
        private readonly Mock<IRepositoryWrapper> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizer;
        private readonly Mock<ILoggerService> _mockLogger;

        public GetAllArtsTest()
        {
            _mockRepo = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerService>();
            _mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Fact]
        public async Task Handle_ReturnsAllArts()
        {
            // Arrange
            MockRepositoryAndMapper(GetArtsList(), GetArtsDtoList());
            var handler = new GetAllArtsHandler(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetAllArtsQuery(), default);

            // Assert
            Assert.Equal(GetArtsList().Count, result.Value.Count());
        }

        [Fact]
        public async Task Handle_ReturnsError()
        {
            // Arrange
            MockRepositoryAndMapper(null, null);
            var expectedError = $"Cannot find any arts";
            _mockLocalizer.Setup(localizer => localizer["CannotFindAnyArts"])
                .Returns(new LocalizedString("CannotFindAnyArts", expectedError));
            var handler = new GetAllArtsHandler(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetAllArtsQuery(), default);

            // Assert
            Assert.Equal(expectedError, result.Errors.Single().Message);
        }

        [Fact]
        public async Task Handle_ReturnsType()
        {
            // Arrange
            MockRepositoryAndMapper(GetArtsList(), GetArtsDtoList());
            var handler = new GetAllArtsHandler(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetAllArtsQuery(), default);

            // Assert
            Assert.IsType<Result<IEnumerable<ArtDTO>>>(result);
        }

        private static List<ArtDTO> GetArtsDtoList()
        {
            return new List<ArtDTO>()
            {
                new ArtDTO
                {
                    Id = 1,
                },
                new ArtDTO
                {
                    Id = 2,
                },
            };
        }

        private static List<Art> GetArtsList()
        {
             return new List<Art>()
             {
                new Art()
                {
                    Id = 1,
                },
                new Art()
                {
                    Id = 2,
                },
             };
        }

        private void MockRepositoryAndMapper(List<Art> artList, List<ArtDTO> artListDto)
        {
           _mockRepo.Setup(r => r.ArtRepository.GetAllAsync(
           It.IsAny<Expression<Func<Art, bool>>>(),
           It.IsAny<Func<IQueryable<Art>,
           IIncludableQueryable<Art, object>>>()))
           .ReturnsAsync(artList);

           _mockMapper.Setup(x => x.Map<IEnumerable<ArtDTO>>(It.IsAny<IEnumerable<object>>()))
           .Returns(artListDto);
        }
    }
}
