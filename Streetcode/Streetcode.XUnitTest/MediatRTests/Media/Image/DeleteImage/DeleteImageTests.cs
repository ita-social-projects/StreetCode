using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Media.Image.Delete;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Media.Image.DeleteImage;

public class DeleteImageTests
{
    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<IBlobService> _mockBlobService;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizerCannotFind;
    private readonly Mock<IStringLocalizer<FailedToDeleteSharedResource>> _mockLocalizerFailedToDelete;

    public DeleteImageTests()
    {
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockBlobService = new Mock<IBlobService>();
        _mockLogger = new Mock<ILoggerService>();
        _mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        _mockLocalizerFailedToDelete = new Mock<IStringLocalizer<FailedToDeleteSharedResource>>();
    }

    [Fact]
    public async Task DeleteImage_ShouldDeleteImage()
    {
        var testImage = GetImage();

        SetupRepository(testImage, 1);
        SetupBlobService();

        var handler = new DeleteImageHandler(_mockRepository.Object, _mockBlobService.Object, _mockLogger.Object, _mockLocalizerFailedToDelete.Object, _mockLocalizerCannotFind.Object);

        var response = await handler.Handle(new DeleteImageCommand(testImage.Id), CancellationToken.None);

        Assert.True(response.IsSuccess);
    }

    [Fact]
    public async Task DeleteImage_ShouldNotDeleteImage()
    {
        var expectedError = "Failed to delete an image";
        _mockLocalizerFailedToDelete.Setup(x => x["FailedToDeleteImage"])
            .Returns(new LocalizedString("FailedToDeleteImage", expectedError));

        var testImage = GetImage();

        SetupRepository(testImage, -1);
        SetupBlobService();

        var handler = new DeleteImageHandler(_mockRepository.Object, _mockBlobService.Object, _mockLogger.Object, _mockLocalizerFailedToDelete.Object, _mockLocalizerCannotFind.Object);

        var response = await handler.Handle(new DeleteImageCommand(testImage.Id), CancellationToken.None);

        Assert.True(response.IsFailed);
    }

    private static DAL.Entities.Media.Images.Image GetImage()
    {
        return new DAL.Entities.Media.Images.Image()
        {
            Id = 1,
            BlobName = "hzbTZ58ebTjpDJDCWosy5F55WRkZU0cl+1Gpo_NWJ+0=.string",
            Base64 = "ab34",
            MimeType = "string",
            ImageDetails = null,
        };
    }

    private void SetupRepository(DAL.Entities.Media.Images.Image testImage, int saveChangesReturnValue)
    {
        _mockRepository.Setup(repo => repo.ImageRepository
            .GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<DAL.Entities.Media.Images.Image, bool>>>(),
                It.IsAny<Func<IQueryable<DAL.Entities.Media.Images.Image>, IIncludableQueryable<DAL.Entities.Media.Images.Image, object>>?>()))
            .ReturnsAsync(testImage);

        _mockRepository.Setup(repo => repo.ImageRepository
            .Delete(It.IsAny<DAL.Entities.Media.Images.Image>()));

        _mockRepository.Setup(repo => repo
            .SaveChangesAsync())
            .ReturnsAsync(saveChangesReturnValue);
    }

    private void SetupBlobService()
    {
        _mockBlobService.Setup(service => service.DeleteFileInStorage(It.IsAny<string>()));
    }
}