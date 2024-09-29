using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Media.Audio;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Media.Audio.Update;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;
using AudioEntity = Streetcode.DAL.Entities.Media.Audio;

namespace Streetcode.XUnitTest.MediatRTests.Media.Audio
{
    public class UpdateAudioTests
    {
        private readonly Mock<IRepositoryWrapper> mockRepository;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly Mock<IBlobService> mockBlobService;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> mockLocalizerFail;
        private readonly Mock<IStringLocalizer<FailedToUpdateSharedResource>> mockLocalizerConvertNull;

        public UpdateAudioTests()
        {
            this.mockRepository = new Mock<IRepositoryWrapper>();
            this.mockMapper = new Mock<IMapper>();
            this.mockLogger = new Mock<ILoggerService>();
            this.mockBlobService = new Mock<IBlobService>();
            this.mockLocalizerFail = new Mock<IStringLocalizer<CannotFindSharedResource>>();
            this.mockLocalizerConvertNull = new Mock<IStringLocalizer<FailedToUpdateSharedResource>>();
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_WhenUpdated()
        {
            // Arrange
            var testUpdateAudioDTO = GetUpdateAudioDTO();
            var testAudioDTO = GetAudioDTO();
            var testAudio = GetAudio();

            this.SetupUpdateRepository(1, testAudio);
            this.SetupBlobService();
            this.SetupMapper(testAudio, testAudioDTO);

            var handler = new UpdateAudioHandler(this.mockMapper.Object, this.mockRepository.Object, this.mockBlobService.Object, this.mockLogger.Object, this.mockLocalizerConvertNull.Object, this.mockLocalizerFail.Object);

            // Act
            var result = await handler.Handle(new UpdateAudioCommand(testUpdateAudioDTO), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task ShouldThrowException_ExtensionIsRequired()
        {
            // Arrange
            var expectedError = "Extension is required";
            this.mockLocalizerConvertNull.Setup(x => x["ExtensionIsRequired"])
            .Returns(new LocalizedString("ExtensionIsRequired", expectedError));

            var testUpdateAudioDTO = GetUpdateAudioDTO_WithoutExtension();
            var testAudioDTO = GetAudioDTO();
            var testAudio = GetAudio();

            this.SetupUpdateRepository(-1, testAudio);
            this.SetupBlobService();
            this.SetupMapper(testAudio, testAudioDTO);

            var handler = new UpdateAudioHandler(this.mockMapper.Object, this.mockRepository.Object, this.mockBlobService.Object, this.mockLogger.Object, this.mockLocalizerConvertNull.Object, this.mockLocalizerFail.Object);

            // Act
            var result = await handler.Handle(new UpdateAudioCommand(testUpdateAudioDTO), CancellationToken.None);

            // Assert
            Assert.True(result.IsFailed);
            Assert.Equal(expectedError, result.Errors[0].Message);
        }

        [Fact]
        public async Task ShouldThrowException_TitleIsRequired()
        {
            // Arrange
            var expectedError = "Title is required";
            this.mockLocalizerConvertNull.Setup(x => x["TitleIsRequired"])
            .Returns(new LocalizedString("TitleIsRequired", expectedError));

            var testCreateAudioDTO = GetUpdateAudioDTO_WithoutTitle();
            var testAudioDTO = GetAudioDTO();
            var testAudio = GetAudio();

            this.SetupUpdateRepository(-1, testAudio);
            this.SetupBlobService();
            this.SetupMapper(testAudio, testAudioDTO);

            var handler = new UpdateAudioHandler(this.mockMapper.Object, this.mockRepository.Object, this.mockBlobService.Object, this.mockLogger.Object, this.mockLocalizerConvertNull.Object, this.mockLocalizerFail.Object);

            // Act
            var result = await handler.Handle(new UpdateAudioCommand(testCreateAudioDTO), CancellationToken.None);

            // Assert
            Assert.True(result.IsFailed);
            Assert.Equal(expectedError, result.Errors[0].Message);
        }

        private static AudioFileBaseUpdateDTO GetUpdateAudioDTO()
        {
            return new AudioFileBaseUpdateDTO()
            {
                Id = 1,
                Title = "Title",
                MimeType = "string",
                BaseFormat = "ab34",
                Extension = "string",
            };
        }

        private static AudioFileBaseUpdateDTO GetUpdateAudioDTO_WithoutExtension()
        {
            return new AudioFileBaseUpdateDTO()
            {
                Id = 1,
                Title = "Title",
                MimeType = "string",
                BaseFormat = "ab34",
            };
        }

        private static AudioFileBaseUpdateDTO GetUpdateAudioDTO_WithoutTitle()
        {
            return new AudioFileBaseUpdateDTO()
            {
                Id = 1,
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

        private void SetupUpdateRepository(int returnNumber, AudioEntity audioEntity)
        {
            this.mockRepository.Setup(x => x.AudioRepository.Update(It.IsAny<AudioEntity>()));
            this.mockRepository.Setup(repo => repo.AudioRepository
                .GetFirstOrDefaultAsync(It.IsAny<Expression<Func<AudioEntity, bool>>>(), It.IsAny<Func<IQueryable<AudioEntity>, IIncludableQueryable<AudioEntity, object>>>()))
                .ReturnsAsync(audioEntity);
            this.mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(returnNumber);
        }

        private void SetupBlobService()
        {
            this.mockBlobService.Setup(service => service.UpdateFileInStorage(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()))
                .Returns("fake_blob_name");
            this.mockBlobService.Setup(service => service.FindFileInStorageAsBase64(
                It.IsAny<string>()))
                .Returns("fake_base64_string");
        }

        private void SetupMapper(AudioEntity testAudio, AudioDTO testAudioDTO)
        {
            this.mockMapper.Setup(x => x.Map<AudioEntity>(It.IsAny<AudioFileBaseUpdateDTO>()))
                .Returns(testAudio);
            this.mockMapper.Setup(x => x.Map<AudioDTO>(It.IsAny<AudioEntity>()))
                .Returns(testAudioDTO);
        }
    }
}
