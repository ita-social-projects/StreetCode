using Xunit;
using Moq;
using AutoMapper;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.BLL.MediatR.Media.Audio.GetByStreetcodeId;
using Streetcode.BLL.DTO.Media;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using Model = Streetcode.DAL.Entities.Media.Audio;
using Streetcode.BLL.Interfaces.BlobStorage;

namespace Streetcode.XUnitTest.MediatRTests.Media.Audio
{
    public class GetAudioByStreetcodeIdHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _repository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IBlobService> _blobService;

        public GetAudioByStreetcodeIdHandlerTests()
        {
            _repository = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _blobService = new Mock<IBlobService>();
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

            var handler = new GetAudioByStreetcodeIdQueryHandler(_repository.Object, _mapper.Object, _blobService.Object);
            // act
            var result = await handler.Handle(new GetAudioByStreetcodeIdQuery(id), CancellationToken.None);
            // assert
            Assert.NotNull(result.Value);
        }

        [Theory]
        [InlineData(2)]
        public async Task Handle_NotExistingId_ReturnsError(int id)
        {
            // arrange
            string expectedErrorMessage = $"Cannot find an audio with the corresponding streetcode id: {id}";
            var testAudioDTO = new AudioDTO {  };

            RepositorySetup(null);
            MapperSetup(testAudioDTO);

            var handler = new GetAudioByStreetcodeIdQueryHandler(_repository.Object, _mapper.Object, _blobService.Object);
            // act
            var result = await handler.Handle(new GetAudioByStreetcodeIdQuery(id), CancellationToken.None);
            // assert
            Assert.Equal(expectedErrorMessage, result.Errors.First().Message);
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

            var handler = new GetAudioByStreetcodeIdQueryHandler(_repository.Object, _mapper.Object, _blobService.Object);
            // act
            var result = await handler.Handle(new GetAudioByStreetcodeIdQuery(id), CancellationToken.None);
            // assert
            Assert.IsType<AudioDTO>(result.ValueOrDefault);
        }

        private void RepositorySetup( Model audio)
        {
            _repository.Setup(repo => repo.AudioRepository
                .GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Model, bool>>>(), It.IsAny<Func<IQueryable<Model>, IIncludableQueryable<Model, Model>>?>()))
                .ReturnsAsync(audio);
        }

        private void MapperSetup(AudioDTO audioDTO)
        {
            _mapper.Setup(x => x.Map<AudioDTO>(It.IsAny<Model>()))
                .Returns(audioDTO);
        }

    }
}
