using System.Linq.Expressions;
using AutoMapper;
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

        public GetCategoriesByStreetcodeIdTest()
        {
            this._mockRepository = new Mock<IRepositoryWrapper>();
            this._mockMapper = new Mock<IMapper>();
            this._blobService = new Mock<IBlobService>();
            this._mockLogger = new Mock<ILoggerService>();
            this._mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Theory]
        [InlineData(1)]
        public async Task ShouldReturnSuccesfully(int id)
        {
            // Arrange
            this._mockRepository.Setup(x => x.SourceCategoryRepository
            .GetAllAsync(
                It.IsAny<Expression<Func<SourceLinkCategory, bool>>>(),
                It.IsAny<Func<IQueryable<SourceLinkCategory>,
                IIncludableQueryable<SourceLinkCategory, object>>>()))
            .ReturnsAsync(GetSourceLinkCategories());

            this._mockMapper.Setup(x => x.Map<IEnumerable<SourceLinkCategoryDTO>>(It.IsAny<IEnumerable<SourceLinkCategory>>()))
                .Returns(GetSourceDTOs());

            var handler = new GetCategoriesByStreetcodeIdHandler(
                this._mockRepository.Object,
                this._mockMapper.Object,
                this._blobService.Object,
                this._mockLogger.Object,
                this._mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new GetCategoriesByStreetcodeIdQuery(id), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.IsType<List<SourceLinkCategoryDTO>>(result.ValueOrDefault));
        }

        [Theory]
        [InlineData(1)]
        public async Task ShouldReturnNull_NotExistingId(int id)
        {
            // Arrange
            this._mockRepository.Setup(x => x.SourceCategoryRepository
            .GetAllAsync(
                It.IsAny<Expression<Func<SourceLinkCategory, bool>>>(),
                It.IsAny<Func<IQueryable<SourceLinkCategory>,
                IIncludableQueryable<SourceLinkCategory, object>>>()))
            .ReturnsAsync(this.GetSourceLinkCategoriesNotExists());

            this._mockMapper.Setup(x => x.Map<IEnumerable<SourceLinkCategoryDTO>>(It.IsAny<IEnumerable<SourceLinkCategory>>()))
                .Returns(GetSourceDTOs());

            var handler = new GetCategoriesByStreetcodeIdHandler(
                this._mockRepository.Object,
                this._mockMapper.Object,
                this._blobService.Object,
                this._mockLogger.Object,
                this._mockLocalizerCannotFind.Object);

            var expectedError = $"Cant find any source category with the streetcode id {id}";
            this._mockLocalizerCannotFind.Setup(x => x[It.IsAny<string>(), It.IsAny<object>()]).Returns((string key, object[] args) =>
            {
                if (args != null && args.Length > 0 && args[0] is int id)
                {
                    return new LocalizedString(key, $"Cant find any source category with the streetcode id {id}");
                }

                return new LocalizedString(key, "Cannot find any source category with unknown id");
            });

            // Act
            var result = await handler.Handle(new GetCategoriesByStreetcodeIdQuery(id), CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors[0].Message);
        }

        private static List<SourceLinkCategory> GetSourceLinkCategories()
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

        private static List<SourceLinkCategoryDTO> GetSourceDTOs()
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

        private List<SourceLinkCategory> GetSourceLinkCategoriesNotExists()
        {
            return new List<SourceLinkCategory>();
        }
    }
}
