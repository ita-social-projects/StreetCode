using System.Linq.Expressions;
using AutoMapper;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Media.Image.Update;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Media.Image.UpdateImage;

public class UpdateImageTests
{
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<IBlobService> _mockBlobService;
    private readonly Mock<ILoggerService> _mockLoggerService;
    private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockStringLocalizerCannotFind;
    private readonly Mock<IStringLocalizer<FailedToUpdateSharedResource>> _mockStringLocalizerFailedToUpdate;

    public UpdateImageTests()
    {
        _mockMapper = new Mock<IMapper>();
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockBlobService = new Mock<IBlobService>();
        _mockLoggerService = new Mock<ILoggerService>();
        _mockStringLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        _mockStringLocalizerFailedToUpdate = new Mock<IStringLocalizer<FailedToUpdateSharedResource>>();
    }

    [Fact]
    public async Task Update_ShouldUpdateImage()
    {
        var testImage = GetImage();
        var testImageDto = GetImageDto();
        var testUpdateImageDto = GetUpdateImageDto();

        SetupBlobService();
        SetupRepository(testImage, 1);
        SetupMapper(testImage, testImageDto);

        var handler = new UpdateImageHandler(_mockMapper.Object, _mockRepository.Object, _mockBlobService.Object, _mockLoggerService.Object, _mockStringLocalizerFailedToUpdate.Object, _mockStringLocalizerCannotFind.Object);

        var response = await handler.Handle(new UpdateImageCommand(testUpdateImageDto), CancellationToken.None);

        Assert.True(response.IsSuccess);
    }

    [Fact]
    public async Task Update_ShouldNotUpdateImage()
    {
        var expectedError = "Failed to update an image";
        _mockStringLocalizerFailedToUpdate.Setup(x => x["FailedToUpdateImage"])
            .Returns(new LocalizedString("FailedToUpdateImage", expectedError));

        var testImage = GetImage();
        var testImageDto = GetImageDto();
        var testUpdateImageDto = GetUpdateImageDto();

        SetupBlobService();
        SetupRepository(testImage, -1);
        SetupMapper(testImage, testImageDto);

        var handler = new UpdateImageHandler(_mockMapper.Object, _mockRepository.Object, _mockBlobService.Object, _mockLoggerService.Object, _mockStringLocalizerFailedToUpdate.Object, _mockStringLocalizerCannotFind.Object);

        var response = await handler.Handle(new UpdateImageCommand(testUpdateImageDto), CancellationToken.None);

        Assert.True(response.IsFailed);
        Assert.Equal(expectedError, response.Errors[0].Message);
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

    private static ImageDTO GetImageDto()
    {
        return new ImageDTO()
        {
            Id = 1,
            BlobName = "fake_blob_name",
            Base64 = "fake_base64_string",
            MimeType = "string",
            ImageDetails = null,
        };
    }

    private static ImageFileBaseUpdateDTO GetUpdateImageDto()
    {
        return new ImageFileBaseUpdateDTO()
        {
            Id = 1,
            Title = "Title",
            MimeType = "string",
            BaseFormat = "ab34",
            Extension = "string",
            Alt = "String",
        };
    }

    private void SetupRepository(DAL.Entities.Media.Images.Image testImage, int testReturnValue)
    {
        _mockRepository.Setup(repository => repository.ImageRepository
            .GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DAL.Entities.Media.Images.Image, bool>>>(), null))
            .ReturnsAsync(testImage);

        _mockRepository.Setup(repository => repository.ImageRepository
            .Update(It.IsAny<DAL.Entities.Media.Images.Image>()));

        _mockRepository.Setup(repository => repository
            .SaveChangesAsync())
            .ReturnsAsync(testReturnValue);
    }

    private void SetupMapper(DAL.Entities.Media.Images.Image testImage, ImageDTO testImageDto)
    {
        _mockMapper.Setup(mapper => mapper
                .Map<DAL.Entities.Media.Images.Image>(It.IsAny<ImageFileBaseUpdateDTO>()))
            .Returns(testImage);

        _mockMapper.Setup(mapper => mapper
                .Map<ImageDTO>(It.IsAny<DAL.Entities.Media.Images.Image>()))
            .Returns(testImageDto);
    }

    private void SetupBlobService()
    {
        _mockBlobService.Setup(blobService => blobService.UpdateFileInStorage(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()))
            .Returns("fake_blob_name");

        _mockBlobService.Setup(blobService => blobService.FindFileInStorageAsBase64(
                It.IsAny<string>()))
            .Returns("fake_base64_string");
    }
}