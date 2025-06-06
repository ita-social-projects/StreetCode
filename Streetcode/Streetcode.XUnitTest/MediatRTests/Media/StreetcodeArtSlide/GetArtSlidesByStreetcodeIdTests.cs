using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Query;
using MockQueryable;
using Moq;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.MediatR.Media.Art.GetByStreetcodeId;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.BLL.Tests.MediatR.Media.Art
{
    public class GetArtSlidesByStreetcodeIdHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
        private readonly Mock<IBlobService> _mockBlobService;
        private readonly Mock<IMapper> _mockMapper;

        public GetArtSlidesByStreetcodeIdHandlerTests()
        {
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _mockBlobService = new Mock<IBlobService>();
            _mockMapper = new Mock<IMapper>();
        }

        [Fact]
        public async Task Handle_ShouldReturnSlides_WhenStreetcodeIdExists()
        {
            // Arrange
            uint streetcodeId = 1;
            var artSlides = GetMockArtSlides(streetcodeId);
            SetupRepositoryWrapper(artSlides);
            SetupMapper();
            SetupBlobService();

            var handler = new GetArtSlidesByStreetcodeIdHandler(
                _mockRepositoryWrapper.Object,
                _mockMapper.Object,
                _mockBlobService.Object);

            // Act
            var response = await handler.Handle(
                new GetArtSlidesByStreetcodeIdQuery(streetcodeId, 1, 5, UserRole.Admin),
                CancellationToken.None);

            // Assert
            response.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenStreetcodeIdDoesNotExist()
        {
            // Arrange
            uint streetcodeId = 999;
            var artSlides = new List<StreetcodeArtSlide>();
            SetupRepositoryWrapper(artSlides);
            SetupMapper();
            SetupBlobService();

            var handler = new GetArtSlidesByStreetcodeIdHandler(
                _mockRepositoryWrapper.Object,
                _mockMapper.Object,
                _mockBlobService.Object);

            // Act
            var response = await handler.Handle(
                new GetArtSlidesByStreetcodeIdQuery(streetcodeId, 1, 5, UserRole.Admin),
                CancellationToken.None);

            // Assert
            response.Value.Should().BeEmpty();
        }

        private void SetupRepositoryWrapper(IEnumerable<StreetcodeArtSlide> artSlides)
        {
            var mockDbSet = artSlides.AsQueryable().BuildMock();

            var mock = new Mock<IQueryable<StreetcodeArtSlide>>();
            mock.As<IQueryable<StreetcodeArtSlide>>().Setup(m => m.Provider).Returns(mockDbSet.Provider);
            mock.As<IQueryable<StreetcodeArtSlide>>().Setup(m => m.Expression).Returns(mockDbSet.Expression);
            mock.As<IQueryable<StreetcodeArtSlide>>().Setup(m => m.ElementType).Returns(mockDbSet.ElementType);
            mock.As<IQueryable<StreetcodeArtSlide>>().Setup(m => m.GetEnumerator()).Returns(mockDbSet.GetEnumerator());

            _mockRepositoryWrapper.Setup(repo => repo.StreetcodeArtSlideRepository
                    .GetAllAsync(
                        It.IsAny<Expression<Func<StreetcodeArtSlide, bool>>>(),
                        It.IsAny<Func<IQueryable<StreetcodeArtSlide>, IIncludableQueryable<StreetcodeArtSlide, object>>?>()))
                .ReturnsAsync(mock.Object.ToList());

            _mockRepositoryWrapper.Setup(repo => repo.StreetcodeArtSlideRepository
                    .FindAll(
                        It.IsAny<Expression<Func<StreetcodeArtSlide, bool>>?>(),
                        It.IsAny<Func<IQueryable<StreetcodeArtSlide>, IIncludableQueryable<StreetcodeArtSlide, object>>?>()))
                .Returns(mockDbSet);
        }

        private void SetupMapper()
        {
            _mockMapper.Setup(mapper =>
                    mapper.Map<IEnumerable<StreetcodeArtSlideDTO>>(It.IsAny<IEnumerable<StreetcodeArtSlide>>()))
                .Returns(new List<StreetcodeArtSlideDTO>());
        }

        private void SetupBlobService()
        {
            _mockBlobService.Setup(blobService => blobService.FindFileInStorageAsBase64(It.IsAny<string>()))
                .Returns("base64image");
        }

        private static List<StreetcodeArtSlide> GetMockArtSlides(uint streetcodeId)
        {
            return new List<StreetcodeArtSlide>
            {
                new StreetcodeArtSlide
                {
                    StreetcodeId = (int)streetcodeId,
                    StreetcodeArts = new List<StreetcodeArt>
                    {
                        new StreetcodeArt
                        {
                            Art = new Streetcode.DAL.Entities.Media.Images.Art
                            {
                                Image = new Streetcode.DAL.Entities.Media.Images.Image
                                    { BlobName = "image1", MimeType = "image/png" },
                            },
                        },
                    },
                },
                new StreetcodeArtSlide
                {
                    StreetcodeId = (int)streetcodeId,
                    StreetcodeArts = new List<StreetcodeArt>
                    {
                        new StreetcodeArt
                        {
                            Art = new Streetcode.DAL.Entities.Media.Images.Art
                            {
                                Image = new Streetcode.DAL.Entities.Media.Images.Image
                                    { BlobName = "image2", MimeType = "image/png" },
                            },
                        },
                    },
                },
            };
        }
    }
}