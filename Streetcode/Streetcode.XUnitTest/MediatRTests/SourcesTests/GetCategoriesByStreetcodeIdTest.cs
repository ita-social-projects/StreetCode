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
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
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
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _blobService = new Mock<IBlobService>();
            _mockLogger = new Mock<ILoggerService>();
            _mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }
        [Theory]
        [InlineData(1)]
        public async Task ShouldReturnSuccesfully(int id)
        {
            // arrange 
            _mockRepository.Setup(x => x.SourceCategoryRepository
            .GetAllAsync(
               It.IsAny<Expression<Func<SourceLinkCategory, bool>>>(),
                It.IsAny<Func<IQueryable<SourceLinkCategory>,
                IIncludableQueryable<SourceLinkCategory, object>>>()))
            .ReturnsAsync(GetSourceLinkCategories());

            _mockMapper.Setup(x => x.Map<IEnumerable<SourceLinkCategoryDTO>>(It.IsAny<IEnumerable<SourceLinkCategory>>()))
                .Returns(GetSourceDTOs());

            var handler = new GetCategoriesByStreetcodeIdHandler(
                _mockRepository.Object,
                _mockMapper.Object,
                _blobService.Object,
                _mockLogger.Object,
                _mockLocalizerCannotFind.Object);

            // act

            var result = await handler.Handle(new GetCategoriesByStreetcodeIdQuery(id), CancellationToken.None);

            // assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.IsType<List<SourceLinkCategoryDTO>>(result.ValueOrDefault)
            );
        }

        [Theory]
        [InlineData(1)]
        public async Task ShouldReturnNull_NotExistingId(int id)
        {
            // arrange 
            _mockRepository.Setup(x => x.SourceCategoryRepository
            .GetAllAsync(
               It.IsAny<Expression<Func<SourceLinkCategory, bool>>>(),
                It.IsAny<Func<IQueryable<SourceLinkCategory>,
                IIncludableQueryable<SourceLinkCategory, object>>>()))
            .ReturnsAsync(GetSourceLinkCategoriesNotExists());

            _mockMapper.Setup(x => x.Map<IEnumerable<SourceLinkCategoryDTO>>(It.IsAny<IEnumerable<SourceLinkCategory>>()))
                .Returns(GetSourceDTOs());

            var handler = new GetCategoriesByStreetcodeIdHandler(
                _mockRepository.Object,
                _mockMapper.Object,
                _blobService.Object,
                _mockLogger.Object,
                _mockLocalizerCannotFind.Object);

            var expectedError = $"Cant find any source category with the streetcode id {id}";
            _mockLocalizerCannotFind.Setup(x => x[It.IsAny<string>(), It.IsAny<object>()]).Returns((string key, object[] args) =>
            {
                if (args != null && args.Length > 0 && args[0] is int id)
                {
                    return new LocalizedString(key, $"Cant find any source category with the streetcode id {id}");
                }

                return new LocalizedString(key, "Cannot find any source category with unknown id");
            });
            // act

            var result = await handler.Handle(new GetCategoriesByStreetcodeIdQuery(id), CancellationToken.None);

            // assert

            Assert.Equal(expectedError, result.Errors.First().Message);
        }

        private List<SourceLinkCategory>? GetSourceLinkCategoriesNotExists()
        {
            return null;
        }

        private static List<SourceLinkCategory> GetSourceLinkCategories()
        {
            return new List<SourceLinkCategory>() 
            {
                new SourceLinkCategory() { 
                    Id = 1,
                    Image = new DAL.Entities.Media.Images.Image()
                },
                new SourceLinkCategory() {
                    Id = 2,
                    Image = new DAL.Entities.Media.Images.Image()
                }
            };
        }
        private static List<SourceLinkCategoryDTO> GetSourceDTOs()
        {
            return new List<SourceLinkCategoryDTO>()
            {
                new SourceLinkCategoryDTO() {
                    Id = 1,
                    Image = new BLL.DTO.Media.Images.ImageDTO()
                },
                new SourceLinkCategoryDTO() { 
                    Id = 2,
                    Image = new BLL.DTO.Media.Images.ImageDTO()
                }
            };
        }
    }
}

