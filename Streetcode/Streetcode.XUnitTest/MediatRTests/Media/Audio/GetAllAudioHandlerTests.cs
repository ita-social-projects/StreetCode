using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Media.Audio;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Media.Audio.GetAll;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;
using Model = Streetcode.DAL.Entities.Media.Audio;

namespace Streetcode.XUnitTest.MediatRTests.Media.Audio
{
    public class GetAllAudioHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> repository;
        private readonly Mock<IMapper> mapper;
        private readonly Mock<IBlobService> blob;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> mockLocalizer;

        public GetAllAudioHandlerTests()
        {
            this.repository = new Mock<IRepositoryWrapper>();
            this.mapper = new Mock<IMapper>();
            this.blob = new Mock<IBlobService>();
            this.mockLogger = new Mock<ILoggerService>();
            this.mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Theory]
        [InlineData(1, "base64-encoded-audio")]
        public async Task Handle_ReturnsSuccess(int id, string expectedBase64)
        {
            // Arrange
            var testAudioList = new List<Model>()
            {
                new Model { Id = id },
            };
            var testAudioListDTO = new List<AudioDTO>()
            {
                new AudioDTO() { Id = id },
            };
            var testAudio = new Model() { Id = id };

            this.RepositorySetup(testAudio, testAudioList);
            this.MapperSetup(testAudioListDTO);
            this.BlobSetup(expectedBase64);

            var handler = new GetAllAudiosHandler(this.repository.Object, this.mapper.Object, this.blob.Object, this.mockLogger.Object, this.mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetAllAudiosQuery(), CancellationToken.None);

            // Assert
            Assert.NotEmpty(result.Value);
        }

        [Fact]
        public async Task Handle_ReturnsError()
        {
            // Arrange
            string expectedErrorMessage = "Cannot find any audios";
            this.RepositorySetup(null, new List<Model>());
            this.MapperSetup(new List<AudioDTO>());
            this.BlobSetup(null);

            this.mockLocalizer.Setup(localizer => localizer["CannotFindAnyAudios"])
                .Returns(new LocalizedString("CannotFindAnyAudios", expectedErrorMessage));

            var handler = new GetAllAudiosHandler(this.repository.Object, this.mapper.Object, this.blob.Object, this.mockLogger.Object, this.mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetAllAudiosQuery(), CancellationToken.None);

            // Assert
            Assert.Equal(expectedErrorMessage, result.Errors.Single().Message);
        }

        private void RepositorySetup(Model? audio, List<Model> audios)
        {
            this.repository.Setup(repo => repo.AudioRepository
             .GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Model, bool>>>(), It.IsAny<Func<IQueryable<Model>, IIncludableQueryable<Model, Model>>?>()))
             .ReturnsAsync(audio);

            this.repository.Setup(repo => repo.AudioRepository
                .GetAllAsync(It.IsAny<Expression<Func<Model, bool>>>(), It.IsAny<Func<IQueryable<Model>, IIncludableQueryable<Model, object>>>()))
                .ReturnsAsync(audios);
        }

        private void MapperSetup(List<AudioDTO> audioDTOs)
        {
            this.mapper.Setup(x => x.Map<IEnumerable<AudioDTO>>(It.IsAny<IEnumerable<object>>()))
                .Returns(audioDTOs);
        }

        private void BlobSetup(string? expectedBase64)
        {
            this.blob
                .Setup(blob => blob.FindFileInStorageAsBase64(It.IsAny<string>()))
                .Returns(expectedBase64!);
        }
    }
}
