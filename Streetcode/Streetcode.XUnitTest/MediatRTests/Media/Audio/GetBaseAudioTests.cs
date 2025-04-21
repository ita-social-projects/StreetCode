using System.Linq.Expressions;
using AutoMapper;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Media.Audio.GetBaseAudio;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Media.Audio;

public class GetBaseAudioTests
{
    private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
    private readonly Mock<IBlobService> _mockBlobService;
    private readonly Mock<ILoggerService> _mockLoggerService;
    private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockStringLocalizerCannotFind;

    public GetBaseAudioTests()
    {
        _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
        _mockBlobService = new Mock<IBlobService>();
        _mockLoggerService = new Mock<ILoggerService>();
        _mockStringLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
    }

    [Fact]
    public async Task GetBaseAudio_ShouldReturnSuccessfully()
    {
        var testAudio = GetBaseAudio();
        var expectedStream = new MemoryStream(new byte[] { 1, 2, 3 });

        SetupBlobService(expectedStream);
        SetupRepositoryWrapper(testAudio.Id);

        var handler = new GetBaseAudioHandler(_mockBlobService.Object, _mockRepositoryWrapper.Object,
            _mockLoggerService.Object, _mockStringLocalizerCannotFind.Object);
        var response = await handler.Handle(new GetBaseAudioQuery(testAudio.Id, UserRole.Admin), CancellationToken.None);

        Assert.True(response.IsSuccess);
        Assert.Equal(expectedStream, response.Value);
    }

    [Fact]
    public async Task GetBaseAudio_InvalidId_ShouldReturnFailure()
    {
        int invalidId = -1;
        MemoryStream expectedStream = null;

        SetupRepositoryWrapper(invalidId);
        SetupBlobService(expectedStream);

        var handler = new GetBaseAudioHandler(_mockBlobService.Object, _mockRepositoryWrapper.Object,
            _mockLoggerService.Object, _mockStringLocalizerCannotFind.Object);
        var response = await handler.Handle(new GetBaseAudioQuery(invalidId, UserRole.Admin), CancellationToken.None);

        Assert.Null(response.Value);
    }

    private static DAL.Entities.Media.Audio GetBaseAudio()
    {
        return new DAL.Entities.Media.Audio
        {
            Id = 1,
        };
    }

    private void SetupRepositoryWrapper(int returnId)
    {
        _mockRepositoryWrapper.Setup(repo => repo.AudioRepository
                .GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DAL.Entities.Media.Audio, bool>>?>(), null))
            .ReturnsAsync(new DAL.Entities.Media.Audio { Id = returnId });
    }

    private void SetupBlobService(MemoryStream testMemoryStream)
    {
        _mockBlobService.Setup(service => service
                .FindFileInStorageAsMemoryStream(It.IsAny<string>()))
            .Returns(testMemoryStream);
    }

    private void SetupStringLocalizer(string expectedError)
    {
        _mockStringLocalizerCannotFind.Setup(localizer => localizer["CannotFindAnAudioWithCorrespondingId"])
            .Returns(new LocalizedString("CannotFindAnAudioWithCorrespondingId", expectedError));
    }
}