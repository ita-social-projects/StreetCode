using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.MediatR.Sources.SourceLink.GetCategoryById;
using Streetcode.DAL.Entities.Sources;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.SourcesTests;

public class GetCategoryByIdTest
{
    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IBlobService> _mockBlobService;
    public GetCategoryByIdTest()
    {
        _mockBlobService = new Mock<IBlobService>();
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockMapper = new Mock<IMapper>();
    }
    [Theory]
    [InlineData(1)]
    public async Task ShouldReturnSuccesfully(int id)
    {
        // arrange 

        _mockRepository.Setup(x => x.SourceCategoryRepository
        .GetFirstOrDefaultAsync(
           It.IsAny<Expression<Func<SourceLinkCategory, bool>>>(),
            It.IsAny<Func<IQueryable<SourceLinkCategory>,
            IIncludableQueryable<SourceLinkCategory, object>>>()))
        .ReturnsAsync(GetSourceLinkCategory());

        _mockMapper.Setup(x => x.Map<SourceLinkCategoryDTO>(It.IsAny<SourceLinkCategory>()))
            .Returns(GetSourceDTO());

        var handler = new GetCategoryByIdHandler(
            _mockRepository.Object,
            _mockMapper.Object,
            _mockBlobService.Object);

        // act

        var result = await handler.Handle(new GetCategoryByIdQuery(id), CancellationToken.None);

        // assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.IsType<SourceLinkCategoryDTO>(result.ValueOrDefault)
        );
    }
    [Theory]
    [InlineData(1)]
    public async Task ShouldReturnNull_NotExistingId(int id)
    {
        // arrange 

        _mockRepository.Setup(x => x.SourceCategoryRepository
        .GetFirstOrDefaultAsync(
           It.IsAny<Expression<Func<SourceLinkCategory, bool>>>(),
            It.IsAny<Func<IQueryable<SourceLinkCategory>,
            IIncludableQueryable<SourceLinkCategory, object>>>()))
        .ReturnsAsync(GetSourceLinkCategoryNotExists());

        _mockMapper.Setup(x => x.Map<SourceLinkCategoryDTO>(It.IsAny<SourceLinkCategory>()))
            .Returns(GetSourceDTO());

        var handler = new GetCategoryByIdHandler(
            _mockRepository.Object,
            _mockMapper.Object,
            _mockBlobService.Object);

        var expectedError = $"Cannot find any srcCategory by the corresponding id: {id}";
        // act

        var result = await handler.Handle(new GetCategoryByIdQuery(id), CancellationToken.None);

        // assert

        Assert.Equal(expectedError, result.Errors.First().Message);
    }

    private SourceLinkCategoryDTO GetSourceDTO()
    {
        return new SourceLinkCategoryDTO() {
            Id = 1,
            Image = new BLL.DTO.Media.Images.ImageDTO()
            {
                BlobName = ""
            }
        };
    }

    private SourceLinkCategory GetSourceLinkCategory()
    {
        return new SourceLinkCategory() {
            Image = new DAL.Entities.Media.Images.Image()
            {
                BlobName = ""
            },

            Id = 1 
        };
    }

    private SourceLinkCategory? GetSourceLinkCategoryNotExists()
    {
        return null;
    }
}
