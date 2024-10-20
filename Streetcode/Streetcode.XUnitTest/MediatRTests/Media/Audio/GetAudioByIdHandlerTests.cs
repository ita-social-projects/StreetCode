using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Media.Audio;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Media.Audio.GetById;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;
using Model = Streetcode.DAL.Entities.Media.Audio;

namespace Streetcode.XUnitTest.MediatRTests.Media.Audio
{
    public class GetAudioByIdHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> repository;
        private readonly Mock<IMapper> mapper;
        private readonly Mock<IBlobService> blobService;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> mockLocalizer;

        public GetAudioByIdHandlerTests()
        {
            this.repository = new Mock<IRepositoryWrapper>();
            this.mapper = new Mock<IMapper>();
            this.blobService = new Mock<IBlobService>();
            this.mockLogger = new Mock<ILoggerService>();
            this.mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Theory]
        [InlineData(2)]
        public async Task Handle_ExistingId_Succcess(int id)
        {
            // Arrange
            var testAudio = new Model() { Id = id };
            var testAudioDTO = new AudioDTO { Id = id };

            this.RepositorySetup(testAudio);
            this.MapperSetup(testAudioDTO);

            var handler = new GetAudioByIdHandler(this.repository.Object, this.mapper.Object, this.blobService.Object, this.mockLogger.Object, this.mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetAudioByIdQuery(id), CancellationToken.None);

            // Assert
            Assert.Equal(id, result.Value.Id);
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_NonExistingId_ErrorHandling(int id)
        {
            // Arrange
            string expectedErrorMessage = $"Cannot find an audio with corresponding id: {id}";

            var testAudioDTO = new AudioDTO { Id = id };

            this.RepositorySetup(null);
            this.MapperSetup(testAudioDTO);

            var handler = new GetAudioByIdHandler(this.repository.Object, this.mapper.Object, this.blobService.Object, this.mockLogger.Object, this.mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetAudioByIdQuery(id), CancellationToken.None);

            // Assert
            Assert.Equal(expectedErrorMessage, result.Errors[0].Message);
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ReturnCorrectType(int id)
        {
            // Arrange
            var testAudio = new Model() { Id = id };

            var testAudioDTO = new AudioDTO { Id = id };

            this.RepositorySetup(testAudio);
            this.MapperSetup(testAudioDTO);

            var handler = new GetAudioByIdHandler(this.repository.Object, this.mapper.Object, this.blobService.Object, this.mockLogger.Object, this.mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetAudioByIdQuery(id), CancellationToken.None);

            // Assert
            Assert.IsAssignableFrom<AudioDTO>(result.Value);
        }

        private void RepositorySetup(Model? audio)
        {
            this.repository.Setup(repo => repo.AudioRepository
                .GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Model, bool>>>(), It.IsAny<Func<IQueryable<Model>, IIncludableQueryable<Model, Model>>?>()))
                .ReturnsAsync(audio);

            this.mockLocalizer.Setup(x => x[It.IsAny<string>(), It.IsAny<object>()])
            .Returns((string key, object[] args) =>
            {
                if (args != null && args.Length > 0 && args[0] is int id)
                {
                    return new LocalizedString(key, $"Cannot find an audio with corresponding id: {id}");
                }

                return new LocalizedString(key, "Cannot find an audio with unknown Id");
            });
        }

        private void MapperSetup(AudioDTO? audioDTO)
        {
            this.mapper.Setup(x => x.Map<AudioDTO?>(It.IsAny<Model>()))
                .Returns(audioDTO);
        }
    }
}
