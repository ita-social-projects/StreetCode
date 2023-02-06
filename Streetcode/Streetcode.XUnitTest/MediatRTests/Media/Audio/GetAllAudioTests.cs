using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Media;
using Streetcode.BLL.MediatR.Media.Audio.GetAll;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;

using Entities = Streetcode.DAL.Entities.Media;

namespace Streetcode.XUnitTest.MediatRTests.Media.Audio
{
    public class GetAllAudioTests
    {
        [Fact]
        public async Task GetAllAudio_Success()
        {
            var repository = new Mock<IRepositoryWrapper>();
            var mockMapper = new Mock<IMapper>();

            var testAudioList = new List<Entities.Audio>()
            {
                new Entities.Audio() {Id = 1 }
            };
            var testAudioListDTO = new List<AudioDTO>()
            {
                new AudioDTO() {Id = 1 }
            };
            var testAudio = new Entities.Audio() { Id = 1 };

            repository.Setup(repo => repo.AudioRepository
                .GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Entities.Audio, bool>>>(), It.IsAny<Func<IQueryable<Entities.Audio>, IIncludableQueryable<Entities.Audio, Entities.Audio>>?>()))
                .ReturnsAsync(testAudio);

            repository.Setup(repo => repo.AudioRepository
                .GetAllAsync(null, null)).ReturnsAsync(testAudioList);

            mockMapper.Setup(x => x.Map<IEnumerable<AudioDTO>>(It.IsAny<IEnumerable<object>>()))
                .Returns(testAudioListDTO);

            var handler = new GetAllAudioHandler(repository.Object, mockMapper.Object);

            var result = await handler.Handle(new GetAllAudeoQuery(), CancellationToken.None);

            Assert.NotNull(result);
            Assert.NotEmpty(result.Value);
        }
        [Fact]
        public async Task GetAllAudio_Error()
        {
            var repository = new Mock<IRepositoryWrapper>();
            var mockMapper = new Mock<IMapper>();

            repository.Setup(repo => repo.AudioRepository
            .GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Entities.Audio, bool>>>(), It.IsAny<Func<IQueryable<Entities.Audio>, IIncludableQueryable<Entities.Audio, Entities.Audio>>?>()))
            .ReturnsAsync((Entities.Audio)null);

            repository.Setup(repo => repo.AudioRepository
            .GetAllAsync(null, null)).ReturnsAsync((List<Entities.Audio>)null);

            mockMapper.Setup(x => x.Map<IEnumerable<AudioDTO>>(It.IsAny<IEnumerable<object>>()))
            .Returns(
                new List<AudioDTO>() { new AudioDTO() { Id = 1 } }
            );

            var handler = new GetAllAudioHandler(repository.Object, mockMapper.Object);

            var result = await handler.Handle(new GetAllAudeoQuery(), CancellationToken.None);

            Assert.NotNull(result);
        }
    }
}
