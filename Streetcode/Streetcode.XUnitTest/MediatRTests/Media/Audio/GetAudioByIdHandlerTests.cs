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
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;
using Model = Streetcode.DAL.Entities.Media.Audio;

namespace Streetcode.XUnitTest.MediatRTests.Media.Audio
{
    public class GetAudioByIdHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _repository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IBlobService> _blobService;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizer;

        public GetAudioByIdHandlerTests()
        {
            _repository = new Mock<IRepositoryWrapper>();
            _mapper = new Mock<IMapper>();
            _blobService = new Mock<IBlobService>();
            _mockLogger = new Mock<ILoggerService>();
            _mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Theory]
        [InlineData(2)]
        public async Task Handle_ExistingId_Succcess(int id)
        {
            // Arrange
            var testAudio = new Model() { Id = id };
            var testAudioDto = new AudioDTO { Id = id };

            RepositorySetup(testAudio);
            MapperSetup(testAudioDto);

            var handler = new GetAudioByIdHandler(_repository.Object, _mapper.Object, _blobService.Object, _mockLogger.Object, _mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetAudioByIdQuery(id, UserRole.User), CancellationToken.None);

            // Assert
            Assert.Equal(id, result.Value.Id);
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_NonExistingId_ErrorHandling(int id)
        {
            // Arrange
            string expectedErrorMessage = $"Cannot find an audio with corresponding id: {id}";

            var testAudioDto = new AudioDTO { Id = id };

            RepositorySetup(null);
            MapperSetup(testAudioDto);

            var handler = new GetAudioByIdHandler(_repository.Object, _mapper.Object, _blobService.Object, _mockLogger.Object, _mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetAudioByIdQuery(id, UserRole.User), CancellationToken.None);

            // Assert
            Assert.Equal(expectedErrorMessage, result.Errors[0].Message);
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ReturnCorrectType(int id)
        {
            // Arrange
            var testAudio = new Model() { Id = id };

            var testAudioDto = new AudioDTO { Id = id };

            RepositorySetup(testAudio);
            MapperSetup(testAudioDto);

            var handler = new GetAudioByIdHandler(_repository.Object, _mapper.Object, _blobService.Object, _mockLogger.Object, _mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetAudioByIdQuery(id, UserRole.User), CancellationToken.None);

            // Assert
            Assert.IsAssignableFrom<AudioDTO>(result.Value);
        }

        private void RepositorySetup(Model? audio)
        {
            _repository.Setup(repo => repo.AudioRepository
                .GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Model, bool>>>(), It.IsAny<Func<IQueryable<Model>, IIncludableQueryable<Model, Model>>?>()))
                .ReturnsAsync(audio);

            _mockLocalizer.Setup(x => x[It.IsAny<string>(), It.IsAny<object>()])
            .Returns((string key, object[] args) =>
            {
                if (args != null && args.Length > 0 && args[0] is int id)
                {
                    return new LocalizedString(key, $"Cannot find an audio with corresponding id: {id}");
                }

                return new LocalizedString(key, "Cannot find an audio with unknown Id");
            });
        }

        private void MapperSetup(AudioDTO? audioDto)
        {
            _mapper.Setup(x => x.Map<AudioDTO?>(It.IsAny<Model>()))
                .Returns(audioDto);
        }
    }
}
