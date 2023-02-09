using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Media;
using Streetcode.BLL.Mapping.Media;
using Streetcode.BLL.MediatR.Media.Audio.GetById;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

using Entities = Streetcode.DAL.Entities.Media;

namespace Streetcode.XUnitTest.MediatRTests.Media.Audio
{
    public class GetAudioByIdTests
    {   
        [Theory]
        [InlineData(2)]
        public async Task GetAudioById_ExistingId_Succcess(int id)
        {
            var repository = new Mock<IRepositoryWrapper>();
            var mockMapper = new Mock<IMapper>();

            var testAudio = new Entities.Audio() { Id = id };

            repository.Setup(repo => repo.AudioRepository
            .GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Entities.Audio, bool>>>(), It.IsAny<Func<IQueryable<Entities.Audio>, IIncludableQueryable<Entities.Audio, Entities.Audio>>?>()))
            .ReturnsAsync(testAudio);

            mockMapper.Setup(x => x.Map<AudioDTO>(It.IsAny<Entities.Audio>()))
            .Returns((Entities.Audio sourceAudio) =>
            {
                return new AudioDTO { Id = sourceAudio.Id };
            });

            var handler = new GetAudioByIdHandler(repository.Object, mockMapper.Object);

            var result = await handler.Handle(new GetAudioByIdQuery(id), CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(id, result.Value.Id);
        }

        [Theory]
        [InlineData(1)]
        public async Task GetAudioById_NonExistingId_ErrorHandling(int id)
        {
            var repository = new Mock<IRepositoryWrapper>();
            var mockMapper = new Mock<IMapper>();

            repository.Setup(repo => repo.AudioRepository
            .GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Entities.Audio, bool>>>(), It.IsAny<Func<IQueryable<Entities.Audio>, IIncludableQueryable<Entities.Audio, Entities.Audio>>?>()))
            .ReturnsAsync((Entities.Audio)null);

            mockMapper.Setup(x => x.Map<AudioDTO>(It.IsAny<Entities.Audio>()))
            .Returns((Entities.Audio sourceAudio) =>
            {
                return new AudioDTO { Id = sourceAudio.Id };
            });

            var handler = new GetAudioByIdHandler(repository.Object, mockMapper.Object);

            var result = await handler.Handle(new GetAudioByIdQuery(id), CancellationToken.None);

            Assert.NotNull(result);
            Assert.True(result.IsFailed);
            Assert.Equal($"Cannot find an audio with corresponding id: {id}", result.Errors.First().Message);
        }

        [Theory]
        [InlineData(1)]
        public async Task GetAudioById_TypeCheck(int id)
        {
            var repository = new Mock<IRepositoryWrapper>();
            var mockMapper = new Mock<IMapper>();

            var testAudio = new Entities.Audio() { Id = id };

            repository.Setup(repo => repo.AudioRepository
            .GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Entities.Audio, bool>>>(), It.IsAny<Func<IQueryable<Entities.Audio>, IIncludableQueryable<Entities.Audio, Entities.Audio>>?>()))
            .ReturnsAsync(testAudio);

            mockMapper.Setup(x => x.Map<AudioDTO>(It.IsAny<Entities.Audio>()))
                .Returns((Entities.Audio sourceAudio) =>
                {
                    return new AudioDTO { Id = sourceAudio.Id };
                });

            var handler = new GetAudioByIdHandler(repository.Object, mockMapper.Object);

            var result = await handler.Handle(new GetAudioByIdQuery(2), CancellationToken.None);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.IsAssignableFrom<AudioDTO>(result.Value);
        }
    }
}
