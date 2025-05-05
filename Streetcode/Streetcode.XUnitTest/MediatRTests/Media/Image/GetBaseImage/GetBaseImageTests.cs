using System.Linq.Expressions;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Media.Image.GetBaseImage;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Media.Image.GetBaseImage;

public class GetBaseImageTests
{
    private readonly Mock<IBlobService> _mockBlobService;
    private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockStringLocalizer;

    public GetBaseImageTests()
    {
        _mockBlobService = new Mock<IBlobService>();
        _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
        _mockLogger = new Mock<ILoggerService>();
        _mockStringLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
    }

    [Fact]
    public async Task GetBaseImage_ShouldReturnImageStream()
    {
        int validId = 1;
        var expectedStream = new MemoryStream(new byte[] { 1, 2, 3 });

        SetupBlobService(expectedStream);
        SetupRepositoryWrapper(validId);

        var handler = new GetBaseImageHandler(_mockBlobService.Object, _mockRepositoryWrapper.Object, _mockLogger.Object, _mockStringLocalizer.Object);

        var response = await handler.Handle(new GetBaseImageQuery(validId), CancellationToken.None);

        Assert.NotNull(response.Value);
        Assert.True(response.IsSuccess);
        Assert.Equal(expectedStream, response.Value);
    }

    [Fact]
    public async Task GetBaseImage_ShouldReturnNullImageStream()
    {
        int invalidId = -1;
        MemoryStream? expectedStream = null;

        SetupBlobService(expectedStream);
        SetupRepositoryWrapper(invalidId);

        var handler = new GetBaseImageHandler(_mockBlobService.Object, _mockRepositoryWrapper.Object, _mockLogger.Object, _mockStringLocalizer.Object);

        var response = await handler.Handle(new GetBaseImageQuery(invalidId), CancellationToken.None);

        Assert.Null(response.Value);
    }

    private void SetupBlobService(MemoryStream? stream)
    {
        _mockBlobService
            .Setup(service => service.FindFileInStorageAsMemoryStream(It.IsAny<string>()))
            .Returns(stream);
    }

    private void SetupRepositoryWrapper(int id)
    {
        _mockRepositoryWrapper
            .Setup(x => x.ImageRepository
                .GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DAL.Entities.Media.Images.Image, bool>>>(), null))
            .ReturnsAsync(new DAL.Entities.Media.Images.Image { Id = id });
    }
}