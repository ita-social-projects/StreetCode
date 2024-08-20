using AutoMapper;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Media.Audio;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Media.Audio.Create;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

using AudioEntity = Streetcode.DAL.Entities.Media.Audio;

namespace Streetcode.XUnitTest.MediatRTests.Media.Audio
{
    public class CreateAudioTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IBlobService> _mockBlobService;
        private readonly Mock<IStringLocalizer<FailedToCreateSharedResource>> _mockLocalizerFail;

        public CreateAudioTests()
        {
            this._mockRepository = new Mock<IRepositoryWrapper>();
            this._mockMapper = new Mock<IMapper>();
            this._mockLogger = new Mock<ILoggerService>();
            this._mockBlobService = new Mock<IBlobService>();
            this._mockLocalizerFail = new Mock<IStringLocalizer<FailedToCreateSharedResource>>();
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_WhenCreated()
        {
            // Arrange
            var testCreateAudioDTO = GetCreateAudioDTO();
            var testAudioDTO = GetAudioDTO();
            var testAudio = GetAudio();

            this.SetupCreateRepository(1);
            this.SetupBlobService();
            this.SetupMapper(testAudio, testAudioDTO);

            var handler = new CreateAudioHandler(this._mockBlobService.Object, this._mockRepository.Object, this._mockLogger.Object, this._mockMapper.Object, this._mockLocalizerFail.Object);

            // Act
            var result = await handler.Handle(new CreateAudioCommand(testCreateAudioDTO), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task ShouldThrowException_ExtensionIsRequired()
        {
            // Arrange
            var expectedError = "Extension is required";
            this._mockLocalizerFail.Setup(x => x["ExtensionIsRequired"])
            .Returns(new LocalizedString("ExtensionIsRequired", expectedError));

            var testCreateAudioDTO = GetCreateAudioDTO_WithoutExtension();
            var testAudioDTO = GetAudioDTO();
            var testAudio = GetAudio();

            this.SetupCreateRepository(-1);
            this.SetupBlobService();
            this.SetupMapper(testAudio, testAudioDTO);

            var handler = new CreateAudioHandler(this._mockBlobService.Object, this._mockRepository.Object, this._mockLogger.Object, this._mockMapper.Object, this._mockLocalizerFail.Object);

            // Act
            var result = await handler.Handle(new CreateAudioCommand(testCreateAudioDTO), CancellationToken.None);

            // Assert
            Assert.True(result.IsFailed);
            Assert.Equal(expectedError, result.Errors[0].Message);
        }

        [Fact]
        public async Task ShouldThrowException_TitleIsRequired()
        {
            // Arrange
            var expectedError = "Title is required";
            this._mockLocalizerFail.Setup(x => x["TitleIsRequired"])
            .Returns(new LocalizedString("TitleIsRequired", expectedError));

            var testCreateAudioDTO = GetCreateAudioDTO_WithoutTitle();
            var testAudioDTO = GetAudioDTO();
            var testAudio = GetAudio();

            this.SetupCreateRepository(-1);
            this.SetupBlobService();
            this.SetupMapper(testAudio, testAudioDTO);

            var handler = new CreateAudioHandler(this._mockBlobService.Object, this._mockRepository.Object, this._mockLogger.Object, this._mockMapper.Object, this._mockLocalizerFail.Object);

            // Act
            var result = await handler.Handle(new CreateAudioCommand(testCreateAudioDTO), CancellationToken.None);

            // Assert
            Assert.True(result.IsFailed);
            Assert.Equal(expectedError, result.Errors[0].Message);
        }

        private static AudioFileBaseCreateDTO GetCreateAudioDTO()
        {
            return new AudioFileBaseCreateDTO()
            {
                Title = "Title",
                MimeType = "string",
                BaseFormat = "ab34",
                Extension = "string",
            };
        }

        private static AudioFileBaseCreateDTO GetCreateAudioDTO_WithoutExtension()
        {
            return new AudioFileBaseCreateDTO()
            {
                Title = "Title",
                MimeType = "string",
                BaseFormat = "ab34",
            };
        }

        private static AudioFileBaseCreateDTO GetCreateAudioDTO_WithoutTitle()
        {
            return new AudioFileBaseCreateDTO()
            {
                MimeType = "string",
                BaseFormat = "ab34",
                Extension = "string",
            };
        }

        private static AudioDTO GetAudioDTO()
        {
            return new AudioDTO()
            {
                Id = 1,
                BlobName = "fake_blob_name",
                Base64 = "fake_base64_string",
                MimeType = "string",
            };
        }

        private static DAL.Entities.Media.Audio GetAudio()
        {
            return new AudioEntity()
            {
                Id = 1,
                BlobName = "hzbTZ58ebTjpDJDCWosy5F55WRkZU0cl+1Gpo_NWJ+0=.string",
                Base64 = "ab34",
                MimeType = "string",
            };
        }

        private void SetupCreateRepository(int returnNumber)
        {
            this._mockRepository.Setup(x => x.AudioRepository.Create(It.IsAny<AudioEntity>()));
            this._mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(returnNumber);
        }

        private void SetupBlobService()
        {
            this._mockBlobService.Setup(service => service.SaveFileInStorage(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()))
                .Returns("fake_blob_name");
            this._mockBlobService.Setup(service => service.FindFileInStorageAsBase64(
                It.IsAny<string>()))
                .Returns("fake_base64_string");
        }

        private void SetupMapper(AudioEntity testAudio, AudioDTO testAudioDTO)
        {
            this._mockMapper.Setup(x => x.Map<AudioEntity>(It.IsAny<AudioFileBaseCreateDTO>()))
                .Returns(testAudio);
            this._mockMapper.Setup(x => x.Map<AudioDTO>(It.IsAny<AudioEntity>()))
                .Returns(testAudioDTO);
        }
    }
}
