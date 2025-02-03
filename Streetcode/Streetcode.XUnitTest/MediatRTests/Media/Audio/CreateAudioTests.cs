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
        private readonly Mock<IRepositoryWrapper> mockRepository;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly Mock<IBlobService> mockBlobService;
        private readonly Mock<IStringLocalizer<FailedToCreateSharedResource>> mockLocalizerFail;

        public CreateAudioTests()
        {
            this.mockRepository = new Mock<IRepositoryWrapper>();
            this.mockMapper = new Mock<IMapper>();
            this.mockLogger = new Mock<ILoggerService>();
            this.mockBlobService = new Mock<IBlobService>();
            this.mockLocalizerFail = new Mock<IStringLocalizer<FailedToCreateSharedResource>>();
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

            var handler = new CreateAudioHandler(this.mockBlobService.Object, this.mockRepository.Object, this.mockLogger.Object, this.mockMapper.Object, this.mockLocalizerFail.Object);

            // Act
            var result = await handler.Handle(new CreateAudioCommand(testCreateAudioDTO), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
        }

        private static AudioFileBaseCreateDto GetCreateAudioDTO()
        {
            return new AudioFileBaseCreateDto()
            {
                Title = "Title",
                MimeType = "string",
                BaseFormat = "ab34",
                Extension = "string",
            };
        }

        private static AudioFileBaseCreateDto GetCreateAudioDTO_WithoutExtension()
        {
            return new AudioFileBaseCreateDto()
            {
                Title = "Title",
                MimeType = "string",
                BaseFormat = "ab34",
            };
        }

        private static AudioFileBaseCreateDto GetCreateAudioDTO_WithoutTitle()
        {
            return new AudioFileBaseCreateDto()
            {
                MimeType = "string",
                BaseFormat = "ab34",
                Extension = "string",
            };
        }

        private static AudioDto GetAudioDTO()
        {
            return new AudioDto()
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
            this.mockRepository.Setup(x => x.AudioRepository.Create(It.IsAny<AudioEntity>()));
            this.mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(returnNumber);
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

        private void SetupMapper(AudioEntity testAudio, AudioDto testAudioDTO)
        {
            this.mockMapper.Setup(x => x.Map<AudioEntity>(It.IsAny<AudioFileBaseCreateDto>()))
                .Returns(testAudio);
            this.mockMapper.Setup(x => x.Map<AudioDto>(It.IsAny<AudioEntity>()))
                .Returns(testAudioDTO);
        }
    }
}
