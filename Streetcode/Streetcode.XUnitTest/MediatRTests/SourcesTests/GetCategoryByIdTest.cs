using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Sources.SourceLink.GetCategoryById;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Sources;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.SourcesTests;

public class GetCategoryByIdTest
{
    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IBlobService> _mockBlobService;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizerCannotFind;

    public GetCategoryByIdTest()
    {
        this._mockBlobService = new Mock<IBlobService>();
        this._mockRepository = new Mock<IRepositoryWrapper>();
        this._mockMapper = new Mock<IMapper>();
        this._mockLogger = new Mock<ILoggerService>();
        this._mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
    }

    [Theory]
    [InlineData(1)]
    public async Task ShouldReturnSuccesfully(int id)
    {
        // Arrange
        this._mockRepository.Setup(x => x.SourceCategoryRepository
        .GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<SourceLinkCategory, bool>>>(),
            It.IsAny<Func<IQueryable<SourceLinkCategory>,
            IIncludableQueryable<SourceLinkCategory, object>>>()))
        .ReturnsAsync(this.GetSourceLinkCategory());

        this._mockMapper.Setup(x => x.Map<SourceLinkCategoryDTO>(It.IsAny<SourceLinkCategory>()))
            .Returns(this.GetSourceDTO());

        var handler = new GetCategoryByIdHandler(
            this._mockRepository.Object,
            this._mockMapper.Object,
            this._mockBlobService.Object,
            this._mockLogger.Object,
            this._mockLocalizerCannotFind.Object);

        // Act
        var result = await handler.Handle(new GetCategoryByIdQuery(id), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.IsType<SourceLinkCategoryDTO>(result.ValueOrDefault));
    }

    [Theory]
    [InlineData(1)]
    public async Task ShouldReturnNull_NotExistingId(int id)
    {
        // Arrange
        this._mockRepository.Setup(x => x.SourceCategoryRepository
        .GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<SourceLinkCategory, bool>>>(),
            It.IsAny<Func<IQueryable<SourceLinkCategory>,
            IIncludableQueryable<SourceLinkCategory, object>>>()))
        .ReturnsAsync(this.GetSourceLinkCategoryNotExists());

        this._mockMapper.Setup(x => x.Map<SourceLinkCategoryDTO>(It.IsAny<SourceLinkCategory>()))
            .Returns(this.GetSourceDTO());

        var handler = new GetCategoryByIdHandler(
            this._mockRepository.Object,
            this._mockMapper.Object,
            this._mockBlobService.Object,
            this._mockLogger.Object,
            this._mockLocalizerCannotFind.Object);

        var expectedError = $"Cannot find any srcCategory by the corresponding id: {id}";
        this._mockLocalizerCannotFind.Setup(x => x[It.IsAny<string>(), It.IsAny<object>()]).Returns((string key, object[] args) =>
        {
            if (args != null && args.Length > 0 && args[0] is int id)
            {
                return new LocalizedString(key, $"Cannot find any srcCategory by the corresponding id: {id}");
            }

            return new LocalizedString(key, "Cannot find any srcCategory with unknown id");
        });

        // Act
        var result = await handler.Handle(new GetCategoryByIdQuery(id), CancellationToken.None);

        // Assert
        Assert.Equal(expectedError, result.Errors[0].Message);
    }

    private SourceLinkCategoryDTO GetSourceDTO()
    {
        return new SourceLinkCategoryDTO()
        {
            Id = 1,
            Image = new BLL.DTO.Media.Images.ImageDTO()
            {
                BlobName = string.Empty,
            },
        };
    }

    private SourceLinkCategory GetSourceLinkCategory()
    {
        return new SourceLinkCategory()
        {
            Image = new DAL.Entities.Media.Images.Image()
            {
                BlobName = string.Empty,
            },
            Id = 1,
        };
    }

    private SourceLinkCategory? GetSourceLinkCategoryNotExists()
    {
        return null;
    }
}
