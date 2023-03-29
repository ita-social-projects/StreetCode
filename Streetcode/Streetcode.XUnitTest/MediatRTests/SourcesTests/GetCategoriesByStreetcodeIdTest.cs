using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Media;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.MediatR.Media.Audio.GetAll;
using Streetcode.BLL.MediatR.Sources.SourceLink.GetCategoriesByStreetcodeId;
using Streetcode.BLL.MediatR.Sources.SourceLink.GetCategoryById;
using Streetcode.DAL.Entities.Sources;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.WebApi.Controllers.Source;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.SourcesTests
{
    public class GetCategoriesByStreetcodeIdTest
    {
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        public GetCategoriesByStreetcodeIdTest()
        {
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
        }
        [Fact]
        public async Task ShouldReturnSuccesfully()
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

            var handler = new GetCategoriesByStreetcodeIdHandler(_mockRepository.Object, _mockMapper.Object);
            
            // act
            
            var result = await handler.Handle(new GetCategoriesByStreetcodeIdQuery(1), CancellationToken.None);

            // assert
            Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.IsType<List<SourceLinkCategoryDTO>>(result.ValueOrDefault)
            );
        }

        [Theory]
        [InlineData(1)]
        public async Task ShouldReturnNull_NotExistingId(int id = 1)
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

            var handler = new GetCategoriesByStreetcodeIdHandler(_mockRepository.Object, _mockMapper.Object);

            var expectedError = $"Cant find any source category with the streetcode id {id}";
            // act

            var result = await handler.Handle(new GetCategoriesByStreetcodeIdQuery(1), CancellationToken.None);

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
                new SourceLinkCategory() { Id = 1 },
                new SourceLinkCategory() { Id = 2 }
            };
        }
        private static List<SourceLinkCategoryDTO> GetSourceDTOs()
        {
            return new List<SourceLinkCategoryDTO>()
            {
                new SourceLinkCategoryDTO() { Id = 1 },
                new SourceLinkCategoryDTO() { Id = 2 }
            };
        }
    }
}

