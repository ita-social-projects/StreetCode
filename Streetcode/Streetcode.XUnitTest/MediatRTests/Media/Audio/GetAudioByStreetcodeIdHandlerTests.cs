using Xunit;
using Moq;
using AutoMapper;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.BLL.MediatR.Media.Audio.GetByStreetcodeId;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using Model = Streetcode.DAL.Entities.Media.Audio;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.DTO.Media.Audio;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Media;

namespace Streetcode.XUnitTest.MediatRTests.Media.Audio
{
  public class GetAudioByStreetcodeIdHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _repository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IBlobService> _blobService;
        private readonly Mock<ILoggerService> _mockLogger;

        public GetAudioByStreetcodeIdHandlerTests()
        {
            _repository = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _blobService = new Mock<IBlobService>();
            _mockLogger = new Mock<ILoggerService>();
        }
        [Theory]
        [InlineData(1)]
        public async Task Handle_ExistingId_ReturnsSuccess(int id)
        {
            // arrange
            var testAudio = new Model() {  };
            var testAudioDTO = new AudioDTO {  };

            RepositorySetup(testAudio);
            MapperSetup(testAudioDTO);

            var handler = new GetAudioByStreetcodeIdQueryHandler(_repository.Object, _mapper.Object, _blobService.Object, _mockLogger.Object);
            // act
            var result = await handler.Handle(new GetAudioByStreetcodeIdQuery(id), CancellationToken.None);
            // assert
            Assert.NotNull(result.Value);
        }

        [Theory]
        [InlineData(-2)]
        public async Task Handle_NotExistingId_ReturnsError(int id)
        {
            StreetcodeContent streetcode = null;
            var testAudioDTO = new AudioDTO {  };

            RepositoryResultFailed();
            MapperSetup(testAudioDTO);
            _repository.Setup(repo => repo.StreetcodeRepository
                    .GetFirstOrDefaultAsync(It.IsAny<Expression<Func<StreetcodeContent, bool>>>(),
                    It.IsAny<Func<IQueryable<StreetcodeContent>,
                    IIncludableQueryable<StreetcodeContent, StreetcodeContent>>?>()))
             .ReturnsAsync(streetcode);
            var handler = new GetAudioByStreetcodeIdQueryHandler(_repository.Object, _mapper.Object, _blobService.Object, _mockLogger.Object);
            // act
            var result = await handler.Handle(new GetAudioByStreetcodeIdQuery(id), CancellationToken.None);
            // assert
            Assert.False(result.IsSuccess);
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ReturnsCorrectType(int id)
        {
            // arrange
            var testAudio = new Model() { };
            var testAudioDTO = new AudioDTO {  };

            RepositorySetup(testAudio);
            MapperSetup(testAudioDTO);

            var handler = new GetAudioByStreetcodeIdQueryHandler(_repository.Object, _mapper.Object, _blobService.Object, _mockLogger.Object);
            // act
            var result = await handler.Handle(new GetAudioByStreetcodeIdQuery(id), CancellationToken.None);
            // assert
            Assert.True(result.IsSuccess);
            Assert.IsType<AudioDTO>(result.ValueOrDefault);
        }

        private void RepositorySetup( Model audio)
        {
            _repository.Setup(repo => repo.AudioRepository
                .GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Model, bool>>>(), It.IsAny<Func<IQueryable<Model>, IIncludableQueryable<Model, Model>>?>()))
                .ReturnsAsync(audio);
            _repository.Setup(repo => repo.StreetcodeRepository
                .GetFirstOrDefaultAsync(It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), It.IsAny<Func<IQueryable<StreetcodeContent>, 
                IIncludableQueryable<StreetcodeContent, object>>?>()))
                .ReturnsAsync(new StreetcodeContent() { Audio = audio});
        }

        private void RepositoryResultFailed() {
            StreetcodeContent streetcode = null;
            _repository.Setup(repo => repo.StreetcodeRepository
                .GetFirstOrDefaultAsync(It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), It.IsAny<Func<IQueryable<StreetcodeContent>,
                IIncludableQueryable<StreetcodeContent, object>>?>()))
                .ReturnsAsync(streetcode);
        }

        private void MapperSetup(AudioDTO audioDTO)
        {
            _mapper.Setup(x => x.Map<AudioDTO>(It.IsAny<Model>()))
                .Returns(audioDTO);
            _mapper.Setup(x => x.Map<AudioDTO>(It.IsAny<DAL.Entities.Media.Audio>()))
                .Returns(audioDTO);
        }
    }
}
