using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Partners;
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
        private readonly Mock<IRepositoryWrapper> mockRepository;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IBlobService> blobService;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> mockLocalizerCannotFind;

        public GetCategoriesByStreetcodeIdTest()
        {
            this.mockRepository = new Mock<IRepositoryWrapper>();
            this.mockMapper = new Mock<IMapper>();
            this.blobService = new Mock<IBlobService>();
            this.mockLogger = new Mock<ILoggerService>();
            this.mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Theory]
        [InlineData(1)]
        public async Task ShouldReturnSuccesfully(int id)
        {
            // Arrange
            this.mockRepository.Setup(x => x.SourceCategoryRepository
            .GetAllAsync(
                It.IsAny<Expression<Func<SourceLinkCategory, bool>>>(),
                It.IsAny<Func<IQueryable<SourceLinkCategory>,
                IIncludableQueryable<SourceLinkCategory, object>>>()))
            .ReturnsAsync(GetSourceLinkCategories());

            this.mockMapper.Setup(x => x.Map<IEnumerable<SourceLinkCategoryDto>>(It.IsAny<IEnumerable<SourceLinkCategory>>()))
                .Returns(GetSourceDTOs());

            var handler = new GetCategoriesByStreetcodeIdHandler(
                this.mockRepository.Object,
                this.mockMapper.Object,
                this.blobService.Object,
                this.mockLogger.Object,
                this.mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new GetCategoriesByStreetcodeIdQuery(id), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotEmpty(result.Value),
                () => Assert.IsAssignableFrom<IEnumerable<SourceLinkCategoryDto>>(result.ValueOrDefault));
        }

        [Theory]
        [InlineData(1)]
        public async Task ShouldReturnEmptyArray_NotExistingId(int id)
        {
            // Arrange
            this.mockRepository.Setup(x => x.SourceCategoryRepository
            .GetAllAsync(
                It.IsAny<Expression<Func<SourceLinkCategory, bool>>>(),
                It.IsAny<Func<IQueryable<SourceLinkCategory>,
                IIncludableQueryable<SourceLinkCategory, object>>>()))
            .ReturnsAsync(this.GetSourceLinkCategoriesNotExists());


            var handler = new GetCategoriesByStreetcodeIdHandler(
                this.mockRepository.Object,
                this.mockMapper.Object,
                this.blobService.Object,
                this.mockLogger.Object,
                this.mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new GetCategoriesByStreetcodeIdQuery(id), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<Result<IEnumerable<SourceLinkCategoryDto>>>(result),
                () => Assert.IsAssignableFrom<IEnumerable<SourceLinkCategoryDto>>(result.Value),
                () => Assert.Empty(result.Value));
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

        private static List<SourceLinkCategoryDto> GetSourceDTOs()
        {
            return new List<SourceLinkCategoryDto>()
            {
                new SourceLinkCategoryDto()
                {
                    Id = 1,
                    Image = new BLL.DTO.Media.Images.ImageDto(),
                },
                new SourceLinkCategoryDto()
                {
                    Id = 2,
                    Image = new BLL.DTO.Media.Images.ImageDto(),
                },
            };
        }

        private List<SourceLinkCategory> GetSourceLinkCategoriesNotExists()
        {
            return new List<SourceLinkCategory>();
        }
    }
}
