using Xunit;
using Moq;
using AutoMapper;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.BLL.MediatR.Media.Audio.GetByStreetcodeId;
using Streetcode.BLL.DTO.Media;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

using Entities = Streetcode.DAL.Entities.Media;

namespace Streetcode.XUnitTest.MediatRTests.Media.Audio
{

    public class GetAudioByStreetcodeIdTests
    {
        [Theory]
        [InlineData(1)]
        public async Task GetAudioByStreetcodeId_ExistingId(int id)
        {
            var repository = new Mock<IRepositoryWrapper>();
            var mockMapper = new Mock<IMapper>();

            var testAudio = new Entities.Audio() { StreetcodeId = id };

            repository.Setup(repo => repo.AudioRepository
            .GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Entities.Audio, bool>>>(), It.IsAny<Func<IQueryable<Entities.Audio>, IIncludableQueryable<Entities.Audio, Entities.Audio>>?>()))
            .ReturnsAsync(testAudio);

            mockMapper.Setup(x => x.Map<AudioDTO>(It.IsAny<Entities.Audio>()))
            .Returns((Entities.Audio sourceAudio) =>
            {
                return new AudioDTO { StreetcodeId = sourceAudio.StreetcodeId };
            });
            // arrange
            var handler = new GetAudioByStreetcodeIdQueryHandler(repository.Object, mockMapper.Object);
            // act
            var result = await handler.Handle(new GetAudioByStreetcodeIdQuery(id), CancellationToken.None);
            // assert
            Assert.NotNull(result);
            Assert.Equal(id, result.Value.StreetcodeId);
        }

        [Theory]
        [InlineData(2)]
        public async Task GetAudioByStreetcodeId_NotExistingId(int id)
        {
            var repository = new Mock<IRepositoryWrapper>();
            var mockMapper = new Mock<IMapper>();

            repository.Setup(repo => repo.AudioRepository
            .GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Entities.Audio, bool>>>(), It.IsAny<Func<IQueryable<Entities.Audio>, IIncludableQueryable<Entities.Audio, Entities.Audio>>?>()))
            .ReturnsAsync((Entities.Audio)null);

            mockMapper.Setup(x => x.Map<AudioDTO>(It.IsAny<Entities.Audio>()))
            .Returns((Entities.Audio sourceAudio) =>
            {
                return new AudioDTO { StreetcodeId = sourceAudio.StreetcodeId };
            });
            // arrange
            var handler = new GetAudioByStreetcodeIdQueryHandler(repository.Object, mockMapper.Object);
            // act
            var result = await handler.Handle(new GetAudioByStreetcodeIdQuery(id), CancellationToken.None);
            // assert
            Assert.NotNull(result);
            Assert.True(result.IsFailed);
            Assert.Equal($"Cannot find an audio with the corresponding streetcode id: {id}", result.Errors.First().Message);
        }

        [Theory]
        [InlineData(1)]
        public async Task GetAudioByStreetcodeId_TypeCheck(int id)
        {
            var repository = new Mock<IRepositoryWrapper>();
            var mockMapper = new Mock<IMapper>();

            var testAudio = new Entities.Audio() { StreetcodeId = id };

            repository.Setup(repo => repo.AudioRepository
            .GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Entities.Audio, bool>>>(), It.IsAny<Func<IQueryable<Entities.Audio>, IIncludableQueryable<Entities.Audio, Entities.Audio>>?>()))
            .ReturnsAsync(testAudio);

            mockMapper.Setup(x => x.Map<AudioDTO>(It.IsAny<Entities.Audio>()))
            .Returns((Entities.Audio sourceAudio) =>
            {
                return new AudioDTO { StreetcodeId = sourceAudio.StreetcodeId };
            });
            // arrange
            var handler = new GetAudioByStreetcodeIdQueryHandler(repository.Object, mockMapper.Object);
            // act
            var result = await handler.Handle(new GetAudioByStreetcodeIdQuery(id), CancellationToken.None);
            // assert
            Assert.NotNull(result);
            Assert.IsType<AudioDTO>(result.ValueOrDefault);
        }

    }
}
