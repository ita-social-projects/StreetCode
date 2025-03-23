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

namespace Streetcode.XUnitTest.MediatRTests.Media.Audio;

public class GetAllAudioHandlerTests
{
    private readonly Mock<IRepositoryWrapper> _repository;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IBlobService> _blob;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizer;

    public GetAllAudioHandlerTests()
    {
        _repository = new Mock<IRepositoryWrapper>();
        _mapper = new Mock<IMapper>();
        _blob = new Mock<IBlobService>();
        _mockLogger = new Mock<ILoggerService>();
        _mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
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
        var testAudioListDto = new List<AudioDTO>()
        {
            new AudioDTO() { Id = id },
        };
        var testAudio = new Model() { Id = id };

        RepositorySetup(testAudio, testAudioList);
        MapperSetup(testAudioListDto);
        BlobSetup(expectedBase64);

        var handler = new GetAllAudiosHandler(_repository.Object, _mapper.Object, _blob.Object, _mockLogger.Object, _mockLocalizer.Object);

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
        RepositorySetup(null!, null!);
        MapperSetup(new List<AudioDTO>());
        BlobSetup(null);

        _mockLocalizer.Setup(localizer => localizer["CannotFindAnyAudios"])
            .Returns(new LocalizedString("CannotFindAnyAudios", expectedErrorMessage));

        var handler = new GetAllAudiosHandler(_repository.Object, _mapper.Object, _blob.Object, _mockLogger.Object, _mockLocalizer.Object);

        // Act
        var result = await handler.Handle(new GetAllAudiosQuery(), CancellationToken.None);

        // Assert
        Assert.Equal(expectedErrorMessage, result.Errors.Single().Message);
    }

    private void RepositorySetup(Model? audio, List<Model> audios)
    {
        _repository.Setup(repo => repo.AudioRepository
                .GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Model, bool>>>(), It.IsAny<Func<IQueryable<Model>, IIncludableQueryable<Model, Model>>?>()))
            .ReturnsAsync(audio);

        _repository.Setup(repo => repo.AudioRepository
                .GetAllAsync(It.IsAny<Expression<Func<Model, bool>>>(), It.IsAny<Func<IQueryable<Model>, IIncludableQueryable<Model, object>>>()))
            .ReturnsAsync(audios);
    }

    private void MapperSetup(List<AudioDTO> audioDtOs)
    {
        _mapper.Setup(x => x.Map<IEnumerable<AudioDTO>>(It.IsAny<IEnumerable<object>>()))
            .Returns(audioDtOs);
    }

    private void BlobSetup(string? expectedBase64)
    {
        this._blob
            .Setup(blob => blob.FindFileInStorageAsBase64(It.IsAny<string>()))
            .Returns(expectedBase64!);
    }
}