using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Media.Audio;
using Streetcode.BLL.DTO.Media;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.MediatR.Media.Audio.GetAll;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using Xunit;
using Model = Streetcode.DAL.Entities.Media.Audio;
using Streetcode.BLL.Interfaces.Logging;

namespace Streetcode.XUnitTest.MediatRTests.Media.Audio
{
     public class GetAllAudioHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _repository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IBlobService> _blob;
        private readonly Mock<ILoggerService> _mockLogger;

        public GetAllAudioHandlerTests()
        {
            _repository = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _blob = new Mock<IBlobService>();
            _mockLogger = new Mock<ILoggerService>();
        }

        [Theory]
        [InlineData(1, "base64-encoded-audio")]
        public async Task Handle_ReturnsSuccess(int id, string expectedBase64)
        {   
            // arrange 
            var testAudioList = new List<Model>()
            {
                new Model { Id = id }
            };
            var testAudioListDTO = new List<AudioDTO>()
            {
                new AudioDTO() { Id = id }
            };
            var testAudio = new Model() { Id = id };

            RepositorySetup(testAudio, testAudioList);
            MapperSetup(testAudioListDTO);
            BlobSetup(expectedBase64);

            var handler = new GetAllAudiosHandler(_repository.Object, _mapper.Object, _blob.Object, _mockLogger.Object);
            // act
            var result = await handler.Handle(new GetAllAudiosQuery(), CancellationToken.None);
            // assert
            Assert.NotEmpty(result.Value);
        }

        [Fact]
        public async Task Handle_ReturnsError()
        {
            // arrange
            string expectedErrorMessage = "Cannot find any audios";
            RepositorySetup(null, null);
            MapperSetup(null);
            BlobSetup(null);
            var handler = new GetAllAudiosHandler(_repository.Object, _mapper.Object, _blob.Object, _mockLogger.Object);
            // act
            var result = await handler.Handle(new GetAllAudiosQuery(), CancellationToken.None);
            // assert
            Assert.Equal(expectedErrorMessage, result.Errors.Single().Message);
        }

        private void RepositorySetup(Model audio, List<Model> audios)
        {
            _repository.Setup(repo => repo.AudioRepository
             .GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Model, bool>>>(), It.IsAny<Func<IQueryable<Model>, IIncludableQueryable<Model, Model>>?>()))
             .ReturnsAsync(audio);

            _repository.Setup(repo => repo.AudioRepository
                .GetAllAsync(It.IsAny<Expression<Func<Model, bool>>>(), It.IsAny<Func<IQueryable<Model>, IIncludableQueryable<Model, object>>>()))
                .ReturnsAsync(audios);
        }

        private void MapperSetup(List<AudioDTO> audioDTOs)
        {
            _mapper.Setup(x => x.Map<IEnumerable<AudioDTO>>(It.IsAny<IEnumerable<object>>()))
                .Returns(audioDTOs);
        }
        private void BlobSetup(string? expectedBase64)
        {
            _blob.Setup(blob => blob.FindFileInStorageAsBase64(It.IsAny<string>()))
                .Returns(expectedBase64);
        }
    } 
}
