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
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IBlobService> _mockBlobService;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizerFail;
        private readonly Mock<IStringLocalizer<FailedToUpdateSharedResource>> _mockLocalizerConvertNull;

        public UpdateAudioTests()
        {
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerService>();
            _mockBlobService = new Mock<IBlobService>();
            _mockLocalizerFail = new Mock<IStringLocalizer<CannotFindSharedResource>>();
            _mockLocalizerConvertNull = new Mock<IStringLocalizer<FailedToUpdateSharedResource>>();
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_WhenUpdated()
        {
            // Arrange
            var testUpdateAudioDto = GetUpdateAudioDto();
            var testAudioDto = GetAudioDto();
            var testAudio = GetAudio();

            SetupUpdateRepository(1, testAudio);
            SetupBlobService();
            SetupMapper(testAudio, testAudioDto);

            var handler = new UpdateAudioHandler(_mockMapper.Object, _mockRepository.Object, _mockBlobService.Object, _mockLogger.Object, _mockLocalizerConvertNull.Object, _mockLocalizerFail.Object);

            // Act
            var result = await handler.Handle(new UpdateAudioCommand(testUpdateAudioDto), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
        }

        private static AudioFileBaseUpdateDTO GetUpdateAudioDto()
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

        private static AudioDTO GetAudioDto()
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
            _mockRepository.Setup(x => x.AudioRepository.Update(It.IsAny<AudioEntity>()));
            _mockRepository.Setup(repo => repo.AudioRepository
                .GetFirstOrDefaultAsync(It.IsAny<Expression<Func<AudioEntity, bool>>>(), It.IsAny<Func<IQueryable<AudioEntity>, IIncludableQueryable<AudioEntity, object>>>()))
                .ReturnsAsync(audioEntity);
            _mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(returnNumber);
        }

        private void SetupBlobService()
        {
            _mockBlobService.Setup(service => service.UpdateFileInStorage(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()))
                .Returns("fake_blob_name");
            _mockBlobService.Setup(service => service.FindFileInStorageAsBase64(
                It.IsAny<string>()))
                .Returns("fake_base64_string");
        }

        private void SetupMapper(AudioEntity testAudio, AudioDTO testAudioDto)
        {
            _mockMapper.Setup(x => x.Map<AudioEntity>(It.IsAny<AudioFileBaseUpdateDTO>()))
                .Returns(testAudio);
            _mockMapper.Setup(x => x.Map<AudioDTO>(It.IsAny<AudioEntity>()))
                .Returns(testAudioDto);
        }
    }
}
