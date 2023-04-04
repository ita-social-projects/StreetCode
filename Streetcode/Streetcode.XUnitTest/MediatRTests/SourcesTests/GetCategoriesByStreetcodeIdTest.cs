using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.MediatR.Sources.SourceLink.GetCategoriesByStreetcodeId;
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
        public GetCategoriesByStreetcodeIdTest()
        {
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _blobService = new Mock<IBlobService>();
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
                _blobService.Object);
            
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
                _blobService.Object);

            var expectedError = $"Cant find any source category with the streetcode id {id}";
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

