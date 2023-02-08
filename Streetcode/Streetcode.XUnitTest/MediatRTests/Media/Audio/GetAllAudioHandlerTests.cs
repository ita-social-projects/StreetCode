using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Media;
using Streetcode.BLL.MediatR.Media.Audio.GetAll;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;

using Model = Streetcode.DAL.Entities.Media.Audio;

namespace Streetcode.XUnitTest.MediatRTests.Media.Audio
{
    public class GetAllAudioHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _repository;
        private readonly Mock<IMapper> _mapper;

        public GetAllAudioHandlerTests()
        {
            _repository = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
        }

        [Fact]
        public async Task Handle_ReturnsSuccess()
        {
            var testAudioList = new List<Model>()
            {
                new Model { Id = 1 }
            };
            var testAudioListDTO = new List<AudioDTO>()
            {
                new AudioDTO() { Id = 1 }
            };
            var testAudio = new Model() { Id = 1 };

            RepositorySetup(testAudio, testAudioList);
            MapperSetup(testAudioListDTO);

            var handler = new GetAllAudioHandler(_repository.Object, _mapper.Object);

            var result = await handler.Handle(new GetAllAudeoQuery(), CancellationToken.None);

            Assert.NotEmpty(result.Value);
        }
        [Fact]
        public async Task Handle_ReturnsError()
        {
            RepositorySetup(null, null);
            MapperSetup(null);

             var handler = new GetAllAudioHandler(_repository.Object, _mapper.Object);

            var result = await handler.Handle(new GetAllAudeoQuery(), CancellationToken.None);

            Assert.Null(result.Value);
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
    }
}
