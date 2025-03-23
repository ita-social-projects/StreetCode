using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Media.Audio;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Media.Audio.GetByStreetcodeId;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;
using Model = Streetcode.DAL.Entities.Media.Audio;

namespace Streetcode.XUnitTest.MediatRTests.Media.Audio
{
    public class GetAudioByStreetcodeIdHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _repository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IBlobService> _blobService;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizer;

        public GetAudioByStreetcodeIdHandlerTests()
        {
            _repository = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _blobService = new Mock<IBlobService>();
            _mockLogger = new Mock<ILoggerService>();
            _mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ExistingId_ReturnsSuccess(int id)
        {
            // Arrange
            var testAudio = new Model() { };
            var testAudioDto = new AudioDTO { };

            RepositorySetup(testAudio);
            MapperSetup(testAudioDto);

            var handler = new GetAudioByStreetcodeIdQueryHandler(_repository.Object, _mapper.Object, _blobService.Object, _mockLogger.Object, _mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetAudioByStreetcodeIdQuery(id), CancellationToken.None);

            // Assert
            Assert.NotNull(result.Value);
        }

        [Theory]
        [InlineData(-2)]
        public async Task Handle_NotExistingId_ReturnsError(int id)
        {
            StreetcodeContent? streetcode = null;
            var testAudioDto = new AudioDTO { };

            RepositoryResultFailed();
            MapperSetup(testAudioDto);

            var errorMessage = "Localized error message for the test.";
            var localizedString = new LocalizedString("CannotFindAnAudioWithTheCorrespondingStreetcodeId", errorMessage);
            _mockLocalizer.Setup(l => l["CannotFindAnAudioWithTheCorrespondingStreetcodeId", id]).Returns(localizedString);

            _repository.Setup(repo => repo.StreetcodeRepository
                .GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<StreetcodeContent, bool>>>(),
                    It.IsAny<Func<IQueryable<StreetcodeContent>,
                    IIncludableQueryable<StreetcodeContent, StreetcodeContent>>?>()))
                .ReturnsAsync(streetcode);
            var handler = new GetAudioByStreetcodeIdQueryHandler(_repository.Object, _mapper.Object, _blobService.Object, _mockLogger.Object, _mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetAudioByStreetcodeIdQuery(id), CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ReturnsCorrectType(int id)
        {
            // Arrange
            var testAudio = new Model() { };
            var testAudioDto = new AudioDTO { };

            RepositorySetup(testAudio);
            MapperSetup(testAudioDto);

            var handler = new GetAudioByStreetcodeIdQueryHandler(_repository.Object, _mapper.Object, _blobService.Object, _mockLogger.Object, _mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetAudioByStreetcodeIdQuery(id), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.IsType<AudioDTO>(result.ValueOrDefault);
        }

        private void RepositorySetup(Model audio)
        {
            _repository.Setup(repo => repo.AudioRepository
                .GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Model, bool>>>(), It.IsAny<Func<IQueryable<Model>, IIncludableQueryable<Model, Model>>?>()))
                .ReturnsAsync(audio);
            _repository.Setup(repo => repo.StreetcodeRepository
                .GetFirstOrDefaultAsync(It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), It.IsAny<Func<IQueryable<StreetcodeContent>,
                IIncludableQueryable<StreetcodeContent, object>>?>()))
                .ReturnsAsync(new StreetcodeContent() { Audio = audio });
        }

        private void RepositoryResultFailed()
        {
            StreetcodeContent? streetcode = null;
            _repository.Setup(repo => repo.StreetcodeRepository
                .GetFirstOrDefaultAsync(It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), It.IsAny<Func<IQueryable<StreetcodeContent>,
                IIncludableQueryable<StreetcodeContent, object>>?>()))
                .ReturnsAsync(streetcode);
        }

        private void MapperSetup(AudioDTO audioDto)
        {
            _mapper.Setup(x => x.Map<AudioDTO>(It.IsAny<Model>()))
                .Returns(audioDto);
            _mapper.Setup(x => x.Map<AudioDTO>(It.IsAny<DAL.Entities.Media.Audio>()))
                .Returns(audioDto);
        }
    }
}
