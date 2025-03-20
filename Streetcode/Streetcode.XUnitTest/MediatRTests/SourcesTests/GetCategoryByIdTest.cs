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
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.SourcesTests;

public class GetCategoryByIdTest
{
    private readonly Mock<IRepositoryWrapper> mockRepository;
    private readonly Mock<IMapper> mockMapper;
    private readonly Mock<IBlobService> mockBlobService;
    private readonly Mock<ILoggerService> mockLogger;
    private readonly Mock<IStringLocalizer<CannotFindSharedResource>> mockLocalizerCannotFind;

    public GetCategoryByIdTest()
    {
        this.mockBlobService = new Mock<IBlobService>();
        this.mockRepository = new Mock<IRepositoryWrapper>();
        this.mockMapper = new Mock<IMapper>();
        this.mockLogger = new Mock<ILoggerService>();
        this.mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
    }

    [Theory]
    [InlineData(1)]
    public async Task ShouldReturnSuccesfully(int id)
    {
        // Arrange
        this.mockRepository.Setup(x => x.SourceCategoryRepository
        .GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<SourceLinkCategory, bool>>>(),
            It.IsAny<Func<IQueryable<SourceLinkCategory>,
            IIncludableQueryable<SourceLinkCategory, object>>>()))
        .ReturnsAsync(this.GetSourceLinkCategory());

        this.mockMapper.Setup(x => x.Map<SourceLinkCategoryDTO>(It.IsAny<SourceLinkCategory>()))
            .Returns(this.GetSourceDTO());

        var handler = new GetCategoryByIdHandler(
            this.mockRepository.Object,
            this.mockMapper.Object,
            this.mockBlobService.Object,
            this.mockLogger.Object,
            this.mockLocalizerCannotFind.Object);

        // Act
        var result = await handler.Handle(new GetCategoryByIdQuery(id, UserRole.User), CancellationToken.None);

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
        this.mockRepository.Setup(x => x.SourceCategoryRepository
        .GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<SourceLinkCategory, bool>>>(),
            It.IsAny<Func<IQueryable<SourceLinkCategory>,
            IIncludableQueryable<SourceLinkCategory, object>>>()))
        .ReturnsAsync(this.GetSourceLinkCategoryNotExists());

        this.mockMapper.Setup(x => x.Map<SourceLinkCategoryDTO>(It.IsAny<SourceLinkCategory>()))
            .Returns(this.GetSourceDTO());

        var handler = new GetCategoryByIdHandler(
            this.mockRepository.Object,
            this.mockMapper.Object,
            this.mockBlobService.Object,
            this.mockLogger.Object,
            this.mockLocalizerCannotFind.Object);

        var expectedError = $"Cannot find any srcCategory by the corresponding id: {id}";
        this.mockLocalizerCannotFind.Setup(x => x[It.IsAny<string>(), It.IsAny<object>()]).Returns((string key, object[] args) =>
        {
            if (args != null && args.Length > 0 && args[0] is int id)
            {
                return new LocalizedString(key, $"Cannot find any srcCategory by the corresponding id: {id}");
            }

            return new LocalizedString(key, "Cannot find any srcCategory with unknown id");
        });

        // Act
        var result = await handler.Handle(new GetCategoryByIdQuery(id, UserRole.User), CancellationToken.None);

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
