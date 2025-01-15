using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.News;
using Streetcode.DAL.Entities.Sources;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.XUnitTest.Mocks;

public static class MockHelpers
{
    public static void SetupMockImageRepositoryGetFirstOrDefaultAsync(Mock<IRepositoryWrapper> mockRepositoryWrapper, int imageId)
    {
        // Returns an Image with Id = 1
        mockRepositoryWrapper.Setup(x => x.ImageRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Image, bool>>>(),
                It.IsAny<Func<IQueryable<Image>, IIncludableQueryable<Image, object>>>()))
            .ReturnsAsync(new Image { Id = imageId });
    }

    public static void SetupMockNewsRepositoryGetFirstOrDefaultAsync(Mock<IRepositoryWrapper> mockRepositoryWrapper)
    {
        // Returns an Image with Id = 1
        mockRepositoryWrapper.Setup(x => x.NewsRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<News, bool>>>(),
                It.IsAny<Func<IQueryable<News>, IIncludableQueryable<News, object>>>()))
            .ReturnsAsync(new News());
    }

    public static void SetupMockImageRepositoryGetFirstOrDefaultAsyncReturnsNull(Mock<IRepositoryWrapper> mockRepositoryWrapper)
    {
        // Returns null
        mockRepositoryWrapper.Setup(x => x.ImageRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Image, bool>>>(),
                It.IsAny<Func<IQueryable<Image>, IIncludableQueryable<Image, object>>>()))
            .ReturnsAsync((Image)null!);
    }

    // This mock method will return an existing category (not null)
    public static void SetupMockSourceCategoryRepositoryWithExistingCategory(Mock<IRepositoryWrapper> mockRepositoryWrapper, string title)
    {
        mockRepositoryWrapper.Setup(x => x.SourceCategoryRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<SourceLinkCategory, bool>>>(),
                It.IsAny<Func<IQueryable<SourceLinkCategory>, IIncludableQueryable<SourceLinkCategory, object>>>()))
            .ReturnsAsync(new SourceLinkCategory { Title = title });
    }

    // This mock method will return null, simulating no existing category found
    public static void SetupMockSourceCategoryRepositoryWithoutExistingCategory(Mock<IRepositoryWrapper> mockRepositoryWrapper)
    {
        mockRepositoryWrapper.Setup(x => x.SourceCategoryRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<SourceLinkCategory, bool>>>(),
                It.IsAny<Func<IQueryable<SourceLinkCategory>, IIncludableQueryable<SourceLinkCategory, object>>>()))
            .ReturnsAsync((SourceLinkCategory)null!);
    }
}