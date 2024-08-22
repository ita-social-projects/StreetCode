using AutoMapper;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Media.Image.Create;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Media.Image.CreateImage
{
    public class CreateImageTests
    {
        private readonly Mock<IRepositoryWrapper> mockRepository;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly Mock<IBlobService> mockBlobService;
        private readonly Mock<IStringLocalizer<FailedToCreateSharedResource>> mockLocalizerFail;
        private readonly Mock<IStringLocalizer<CannotConvertNullSharedResource>> mockLocalizerConvertNull;

        public CreateImageTests()
        {
            this.mockRepository = new Mock<IRepositoryWrapper>();
            this.mockMapper = new Mock<IMapper>();
            this.mockLogger = new Mock<ILoggerService>();
            this.mockBlobService = new Mock<IBlobService>();
            this.mockLocalizerFail = new Mock<IStringLocalizer<FailedToCreateSharedResource>>();
            this.mockLocalizerConvertNull = new Mock<IStringLocalizer<CannotConvertNullSharedResource>>();
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_WhenCreated()
        {
            // Arrange
            var testCreateImageDTO = GetCreateImageDTO();
            var testImageDTO = GetImageDTO();
            var testImage = GetImage();

            this.SetupCreateRepository(1);
            this.SetupBlobService();
            this.SetupMapper(testImage, testImageDTO);

            var handler = new CreateImageHandler(this.mockBlobService.Object, this.mockRepository.Object, this.mockLogger.Object, this.mockMapper.Object, this.mockLocalizerFail.Object, this.mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new CreateImageCommand(testCreateImageDTO), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task ShouldThrowException_AltIsRequired()
        {
            // Arrange
            var expectedError = "Cannot create image without alt";
            this.mockLocalizerConvertNull.Setup(x => x["CannotCreateImageWithoutAlt"])
            .Returns(new LocalizedString("CannotConvertNullToCategory", expectedError));

            var testCreateImageDTO = GetImageCreateDTOWithoutAlt();
            var testImageDTO = GetImageDTO();
            var testImage = GetImage();

            this.SetupCreateRepository(1);
            this.SetupBlobService();
            this.SetupMapper(testImage, testImageDTO);

            var handler = new CreateImageHandler(this.mockBlobService.Object, this.mockRepository.Object, this.mockLogger.Object, this.mockMapper.Object, this.mockLocalizerFail.Object, this.mockLocalizerConvertNull.Object);

            // Act
            var result = await handler.Handle(new CreateImageCommand(testCreateImageDTO), CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(expectedError, result.Errors[0].Message);
        }

        [Fact]
        public async Task ShouldThrowException_SaveChangesAsyncIsNotSuccessful()
        {
            // Arrange
            var expectedError = "Failed to create an image";
            this.mockLocalizerFail.Setup(x => x["FailedToCreateAnImage"])
            .Returns(new LocalizedString("FailedToCreateAnImage", expectedError));

            var testCreateImageDTO = GetCreateImageDTO();
            var testImageDTO = GetImageDTO();
            var testImage = GetImage();

            this.SetupCreateRepository(-1);
            this.SetupBlobService();
            this.SetupMapper(testImage, testImageDTO);

            var handler = new CreateImageHandler(this.mockBlobService.Object, this.mockRepository.Object, this.mockLogger.Object, this.mockMapper.Object, this.mockLocalizerFail.Object, this.mockLocalizerConvertNull.Object);

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

        private static ImageFileBaseCreateDTO GetImageCreateDTOWithoutAlt()
        {
            return new ImageFileBaseCreateDTO()
            {
                Title = "Title",
                MimeType = "string",
                BaseFormat = "ab34",
                Extension = "string",
            };
        }

        private void SetupCreateRepository(int returnNumber)
        {
            this.mockRepository.Setup(x => x.ImageRepository.Create(It.IsAny<DAL.Entities.Media.Images.Image>()));
            this.mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(returnNumber);
        }

        private void SetupMapper(DAL.Entities.Media.Images.Image testImage, ImageDTO testImageDTO)
        {
            this.mockMapper.Setup(x => x.Map<DAL.Entities.Media.Images.Image>(It.IsAny<ImageFileBaseCreateDTO>()))
                .Returns(testImage);
            this.mockMapper.Setup(x => x.Map<ImageDTO>(It.IsAny<DAL.Entities.Media.Images.Image>()))
                .Returns(testImageDTO);
        }

        private void SetupBlobService()
        {
            this.mockBlobService.Setup(service => service.SaveFileInStorage(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()))
                .Returns("fake_blob_name");
            this.mockBlobService.Setup(service => service.FindFileInStorageAsBase64(
                It.IsAny<string>()))
                .Returns("fake_base64_string");
        }
    }
}
