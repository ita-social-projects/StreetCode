using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Sources.SourceLink.GetCategoriesByStreetcodeId;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Sources;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.SourcesTests
{
    public class GetCategoriesByStreetcodeIdTest
    {
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IBlobService> _blobService;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizerCannotFind;
        private readonly GetCategoriesByStreetcodeIdHandler _handler;

        public GetCategoriesByStreetcodeIdTest()
        {
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _blobService = new Mock<IBlobService>();
            _mockLogger = new Mock<ILoggerService>();
            _mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
            _handler = new GetCategoriesByStreetcodeIdHandler(
                _mockRepository.Object,
                _mockMapper.Object,
                _blobService.Object,
                _mockLogger.Object);
        }

        [Theory]
        [InlineData(1)]
        public async Task ShouldReturnSuccesfully(int id)
        {
            // Arrange
            _mockRepository.Setup(x => x.SourceCategoryRepository
            .GetAllAsync(
                It.IsAny<Expression<Func<SourceLinkCategory, bool>>>(),
                It.IsAny<Func<IQueryable<SourceLinkCategory>,
                IIncludableQueryable<SourceLinkCategory, object>>>()))
            .ReturnsAsync(GetSourceLinkCategories());

            _mockMapper.Setup(x => x.Map<IEnumerable<SourceLinkCategoryDTO>>(It.IsAny<IEnumerable<SourceLinkCategory>>()))
                .Returns(GetSourceDtOs());

            // Act
            var result = await _handler.Handle(new GetCategoriesByStreetcodeIdQuery(id), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotEmpty(result.Value),
                () => Assert.IsAssignableFrom<IEnumerable<SourceLinkCategoryDTO>>(result.ValueOrDefault));
        }

        [Theory]
        [InlineData(1)]
        public async Task ShouldReturnEmptyArray_NotExistingId(int id)
        {
            // Arrange
            _mockRepository.Setup(x => x.SourceCategoryRepository
            .GetAllAsync(
                It.IsAny<Expression<Func<SourceLinkCategory, bool>>>(),
                It.IsAny<Func<IQueryable<SourceLinkCategory>,
                IIncludableQueryable<SourceLinkCategory, object>>>()))
            .ReturnsAsync(GetSourceLinkCategoriesNotExists());

            // Act
            var result = await _handler.Handle(new GetCategoriesByStreetcodeIdQuery(id), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<Result<IEnumerable<SourceLinkCategoryDTO>>>(result),
                () => Assert.IsAssignableFrom<IEnumerable<SourceLinkCategoryDTO>>(result.Value),
                () => Assert.Empty(result.Value));
        }

        private static IEnumerable<SourceLinkCategory> GetSourceLinkCategories()
        {
            return new List<SourceLinkCategory>()
            {
                new SourceLinkCategory()
                {
                    Id = 1,
                    Image = new DAL.Entities.Media.Images.Image(),
                },
                new SourceLinkCategory()
                {
                    Id = 2,
                    Image = new DAL.Entities.Media.Images.Image(),
                },
            };
        }

        private static List<SourceLinkCategoryDTO> GetSourceDtOs()
        {
            return new List<SourceLinkCategoryDTO>()
            {
                new SourceLinkCategoryDTO()
                {
                    Id = 1,
                    Image = new BLL.DTO.Media.Images.ImageDTO(),
                },
                new SourceLinkCategoryDTO()
                {
                    Id = 2,
                    Image = new BLL.DTO.Media.Images.ImageDTO(),
                },
            };
        }

        private static IEnumerable<SourceLinkCategory> GetSourceLinkCategoriesNotExists()
        {
            return new List<SourceLinkCategory>();
        }
    }
}
