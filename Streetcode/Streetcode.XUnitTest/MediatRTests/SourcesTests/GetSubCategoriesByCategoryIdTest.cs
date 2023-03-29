using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.MediatR.Media.StreetcodeArt.GetByStreetcodeId;
using Streetcode.BLL.MediatR.Sources.SourceLink.GetCategoriesByStreetcodeId;
using Streetcode.BLL.MediatR.Sources.SourceLink.GetSubCategoriesByCategoryId;
using Streetcode.DAL.Entities.Sources;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.SourcesTests
{
    public class GetSubCategoriesByCategoryIdTest
    {
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        public GetSubCategoriesByCategoryIdTest()
        {
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
        }
        [Fact]
        public async Task ShouldReturnSuccesfully()
        {
            // assert
            _mockRepository.Setup(x => x.SourceSubCategoryRepository.
            GetAllAsync(It.IsAny<Expression<Func<SourceLinkSubCategory, bool>>>(),
                It.IsAny<Func<IQueryable<SourceLinkSubCategory>,
                IIncludableQueryable<SourceLinkSubCategory, object>>>()))
            .ReturnsAsync(GetSourceLinkSubCategories());

            _mockMapper.Setup(x => x.Map<IEnumerable<SourceLinkSubCategoryDTO>>(It.IsAny<IEnumerable<SourceLinkSubCategory>>()))
                .Returns(GetSourceSubDTOs());

            // arrange

            var handler = new GetSubCategoriesByCategoryIdHandler(_mockRepository.Object, _mockMapper.Object);

            // act

            var result = await handler.Handle(new GetSubCategoriesByCategoryIdQuery(1), CancellationToken.None);

            Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.IsType<List<SourceLinkSubCategoryDTO>>(result.ValueOrDefault)
            ) ;
        }

        [Theory]
        [InlineData(1)]
        public async Task ShouldReturnNull_NotExistingId(int id = 1)
        {
            // arrange 

            _mockRepository.Setup(x => x.SourceSubCategoryRepository
            .GetAllAsync(
               It.IsAny<Expression<Func<SourceLinkSubCategory, bool>>>(),
                It.IsAny<Func<IQueryable<SourceLinkSubCategory>,
                IIncludableQueryable<SourceLinkSubCategory, object>>>()))
            .ReturnsAsync(GetSourceLinkSubCategoriesNotExists());

            _mockMapper.Setup(x => x.Map<IEnumerable<SourceLinkSubCategoryDTO>>(It.IsAny<IEnumerable<SourceLinkSubCategory>>()))
                .Returns(GetSourceSubDTOs());

            var handler = new GetSubCategoriesByCategoryIdHandler(_mockRepository.Object, _mockMapper.Object);

            var expectedError = $"Cannot find any source category with corresponding id: {id}";
            
            // act

            var result = await handler.Handle(new GetSubCategoriesByCategoryIdQuery(1), CancellationToken.None);

            // assert

            Assert.Equal(expectedError, result.Errors.First().Message);
        }

        private List<SourceLinkSubCategory>? GetSourceLinkSubCategoriesNotExists()
        {
            return null;
        }

        private IEnumerable<SourceLinkSubCategoryDTO> GetSourceSubDTOs()
        {
            return new List<SourceLinkSubCategoryDTO>()
            {
                new SourceLinkSubCategoryDTO() { Id = 1 },
                new SourceLinkSubCategoryDTO() { Id = 2 },
            };
        }

        private IEnumerable<SourceLinkSubCategory> GetSourceLinkSubCategories()
        {
            return new List<SourceLinkSubCategory>()
            {
                new SourceLinkSubCategory() { Id = 1 },
                new SourceLinkSubCategory() { Id = 2 },
            };
        }
    }
}
