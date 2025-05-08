using AutoMapper;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.ImageComparator;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Media.Image.Create;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Media.Image.CreateImage
{
    public class CreateImageTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IBlobService> _mockBlobService;
        private readonly Mock<IStringLocalizer<FailedToCreateSharedResource>> _mockLocalizerFail;
        private readonly Mock<IStringLocalizer<CannotConvertNullSharedResource>> _mockLocalizerConvertNull;
        private readonly Mock<IImageHashGeneratorService> _mockImageHashGeneratorService;

        public CreateImageTests()
        {
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerService>();
            _mockBlobService = new Mock<IBlobService>();
            _mockLocalizerFail = new Mock<IStringLocalizer<FailedToCreateSharedResource>>();
            _mockLocalizerConvertNull = new Mock<IStringLocalizer<CannotConvertNullSharedResource>>();
            _mockImageHashGeneratorService = new Mock<IImageHashGeneratorService>();
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_WhenCreated()
        {
            // Arrange
            var testCreateImageDTO = GetCreateImageDTO();
            var testImageDTO = GetImageDTO();
            var testImage = GetImage();

            SetupImageHashGeneratorService();
            SetupCreateRepository(1);
            SetupBlobService();
            SetupMapper(testImage, testImageDTO);

            var handler = new CreateImageHandler(_mockBlobService.Object, _mockRepository.Object, _mockLogger.Object, _mockMapper.Object, _mockLocalizerFail.Object, _mockLocalizerConvertNull.Object, _mockImageHashGeneratorService.Object);

            // Act
            var result = await handler.Handle(new CreateImageCommand(testCreateImageDTO), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task ShouldThrowException_SaveChangesAsyncIsNotSuccessful()
        {
            // Arrange
            var expectedError = "Failed to create an image";
            _mockLocalizerFail.Setup(x => x["FailedToCreateAnImage"])
            .Returns(new LocalizedString("FailedToCreateAnImage", expectedError));

            var testCreateImageDTO = GetCreateImageDTO();
            var testImageDTO = GetImageDTO();
            var testImage = GetImage();

            SetupImageHashGeneratorService();
            SetupCreateRepository(-1);
            SetupBlobService();
            SetupMapper(testImage, testImageDTO);

            var handler = new CreateImageHandler(_mockBlobService.Object, _mockRepository.Object, _mockLogger.Object, _mockMapper.Object, _mockLocalizerFail.Object, _mockLocalizerConvertNull.Object, _mockImageHashGeneratorService.Object);

            // Act
            var result = await handler.Handle(new CreateImageCommand(testCreateImageDTO), CancellationToken.None);

            // Assert
            Assert.True(result.IsFailed);
            Assert.Equal(expectedError, result.Errors[0].Message);
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

        private static ImageDTO GetImageDTO()
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

        private static ImageFileBaseCreateDTO GetCreateImageDTO()
        {
            return new ImageFileBaseCreateDTO()
            {
                Title = "Title",
                MimeType = "string",
                BaseFormat = "ab34",
                Extension = "string",
                Alt = "String",
            };
        }

        private void SetupImageHashGeneratorService()
        {
            _mockImageHashGeneratorService.Setup(x => x.GenerateImageHash(It.IsAny<string>()));
        }

        private void SetupCreateRepository(int returnNumber)
        {
            _mockRepository.Setup(x => x.ImageRepository.Create(It.IsAny<DAL.Entities.Media.Images.Image>()));
            _mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(returnNumber);
        }

        private void SetupMapper(DAL.Entities.Media.Images.Image testImage, ImageDTO testImageDTO)
        {
            _mockMapper.Setup(x => x.Map<DAL.Entities.Media.Images.Image>(It.IsAny<ImageFileBaseCreateDTO>()))
                .Returns(testImage);
            _mockMapper.Setup(x => x.Map<ImageDTO>(It.IsAny<DAL.Entities.Media.Images.Image>()))
                .Returns(testImageDTO);
        }

        private void SetupBlobService()
        {
            _mockBlobService.Setup(service => service.SaveFileInStorage(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()))
                .Returns("fake_blob_name");
            _mockBlobService.Setup(service => service.FindFileInStorageAsBase64(
                It.IsAny<string>()))
                .Returns("fake_base64_string");
        }
    }
}