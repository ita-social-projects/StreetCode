using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Entities.Sources;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Users;
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

    public static void SetupMockImageRepositoryGetFirstOrDefaultAsyncReturnsNull(Mock<IRepositoryWrapper> mockRepositoryWrapper)
    {
        // Returns null
        mockRepositoryWrapper.Setup(x => x.ImageRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Image, bool>>>(),
                It.IsAny<Func<IQueryable<Image>, IIncludableQueryable<Image, object>>>()))
            .ReturnsAsync((Image)null!);
    }

    public static void SetupMockPartnersRepositoryGetFirstOrDefaultAsync(Mock<IRepositoryWrapper> mockRepositoryWrapper, int imageId)
    {
        // Returns an Image with Id = 1
        mockRepositoryWrapper.Setup(x => x.PartnersRepository.GetSingleOrDefaultAsync(
                It.IsAny<Expression<Func<Partner, bool>>>(),
                It.IsAny<Func<IQueryable<Partner>, IIncludableQueryable<Partner, object>>>()))
            .ReturnsAsync(new Partner { LogoId = imageId });
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

    // This method will return existing streetcode ids
    public static void SetupMockStreetcodeRepositoryFindAll(Mock<IRepositoryWrapper> mockRepositoryWrapper, List<int> streetcodeIds)
    {
        mockRepositoryWrapper.Setup(x => x.StreetcodeRepository.GetAllAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>>(),
                It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>>()))
            .ReturnsAsync(streetcodeIds.Select(id => new StreetcodeContent { Id = id }).ToList());
    }

    // This method will return existing user with email
    public static void SetupMockUserRepositoryGetFirstOfDefaultAsync(Mock<IRepositoryWrapper> mockRepositoryWrapper, string email)
    {
        mockRepositoryWrapper.Setup(x => x.UserRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
            .ReturnsAsync(new User { Email = email });
    }

    public static void SetupMockMapper<TDestination, TSource>(
        Mock<IMapper> mockMapper,
        TDestination mapperResult,
        TSource mapperSource)
    {
        mockMapper
            .Setup(x => x.Map<TDestination>(mapperSource))
            .Returns(mapperResult);
    }
}

