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
        private readonly Mock<IRepositoryWrapper> mockRepo;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> mockLocalizer;
        private readonly Mock<ILoggerService> mockLogger;

        public GetAllArtsTest()
        {
            this.mockRepo = new Mock<IRepositoryWrapper>();
            this.mockMapper = new Mock<IMapper>();
            this.mockLogger = new Mock<ILoggerService>();
            this.mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Fact]
        public async Task Handle_ReturnsAllArts()
        {
            // Arrange
            this.MockRepositoryAndMapper(this.GetArtsList(), this.GetArtsDTOList());
            var handler = new GetAllArtsHandler(this.mockRepo.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetAllArtsQuery(), default);

            // Assert
            Assert.Equal(this.GetArtsList().Count, result.Value.Count());
        }

        [Fact]
        public async Task Handle_ReturnsError()
        {
            // Arrange
            this.MockRepositoryAndMapper(null, null);
            var expectedError = $"Cannot find any arts";
            this.mockLocalizer.Setup(localizer => localizer["CannotFindAnyArts"])
                .Returns(new LocalizedString("CannotFindAnyArts", expectedError));
            var handler = new GetAllArtsHandler(this.mockRepo.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetAllArtsQuery(), default);

            // Assert
            Assert.Equal(expectedError, result.Errors.Single().Message);
        }

        [Fact]
        public async Task Handle_ReturnsType()
        {
            // Arrange
            this.MockRepositoryAndMapper(this.GetArtsList(), this.GetArtsDTOList());
            var handler = new GetAllArtsHandler(this.mockRepo.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetAllArtsQuery(), default);

            // Assert
            Assert.IsType<Result<IEnumerable<ArtDTO>>>(result);
        }

        private List<DAL.Entities.Media.Images.Art> GetArtsList()
        {
             return new List<DAL.Entities.Media.Images.Art>()
             {
                new ()
                {
                    Id = 1,
                },
                new ()
                {
                    Id = 2,
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
                },
                new ArtDTO
                {
                    Id = 2,
                },
            };
        }

        private void MockRepositoryAndMapper(List<DAL.Entities.Media.Images.Art> artList, List<ArtDTO> artListDTO)
        {
           this.mockRepo.Setup(r => r.ArtRepository.GetAllAsync(
           It.IsAny<Expression<Func<DAL.Entities.Media.Images.Art, bool>>>(),
           It.IsAny<Func<IQueryable<DAL.Entities.Media.Images.Art>,
           IIncludableQueryable<DAL.Entities.Media.Images.Art, object>>>()))
           .ReturnsAsync(artList);

           this.mockMapper.Setup(x => x.Map<IEnumerable<ArtDTO>>(It.IsAny<IEnumerable<object>>()))
           .Returns(artListDTO);
        }
    }
}
