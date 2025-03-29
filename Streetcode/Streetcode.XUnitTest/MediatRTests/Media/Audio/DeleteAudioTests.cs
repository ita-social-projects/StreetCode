using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Media.Audio.Delete;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Media.Audio;

public class DeleteAudioTests
{
    private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
    private readonly Mock<IBlobService> _mockBlobService;
    private readonly Mock<ILoggerService> _mockLoggerService;
    private readonly Mock<IStringLocalizer<FailedToDeleteSharedResource>> _mockStringLocalizerFailedToDelete;
    private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockStringLocalizerCannotFind;

    public DeleteAudioTests()
    {
        _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
        _mockBlobService = new Mock<IBlobService>();
        _mockLoggerService = new Mock<ILoggerService>();
        _mockStringLocalizerFailedToDelete = new Mock<IStringLocalizer<FailedToDeleteSharedResource>>();
        _mockStringLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
    }

    [Fact]
    public async Task Delete_ShouldDeleteSuccessfully()
    {
        int audioId = 1;

        SetupRepositoryWrapper(audioId, 1);
        SetupBlobService();

        var handler = new DeleteAudioHandler(_mockRepositoryWrapper.Object, _mockBlobService.Object, _mockLoggerService.Object, _mockStringLocalizerFailedToDelete.Object, _mockStringLocalizerCannotFind.Object);
        var response = await handler.Handle(new DeleteAudioCommand(audioId), CancellationToken.None);

        Assert.True(response.IsSuccess);
    }

    [Fact]
    public async Task Delete_IncorrectId_ShouldReturnError()
    {
        int audioId = -1;

        SetupRepositoryWrapper(audioId, -1);
        SetupBlobService();
        SetupStringLocalizer("Failed to delete audio");

        var handler = new DeleteAudioHandler(_mockRepositoryWrapper.Object, _mockBlobService.Object, _mockLoggerService.Object, _mockStringLocalizerFailedToDelete.Object, _mockStringLocalizerCannotFind.Object);
        var response = await handler.Handle(new DeleteAudioCommand(audioId), CancellationToken.None);

        Assert.True(response.IsFailed);
    }

    private void SetupRepositoryWrapper(int id, int expectedResponse)
    {
        _mockRepositoryWrapper.Setup(repo => repo.AudioRepository
            .GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<DAL.Entities.Media.Audio, bool>>>(),
                It.IsAny<Func<IQueryable<DAL.Entities.Media.Audio>, IIncludableQueryable<DAL.Entities.Media.Audio, object>>?>()))
            .ReturnsAsync(new DAL.Entities.Media.Audio { Id = id });

        _mockRepositoryWrapper.Setup(repo => repo.AudioRepository
            .Delete(It.IsAny<DAL.Entities.Media.Audio>()));

        _mockRepositoryWrapper.Setup(repo => repo
            .SaveChangesAsync())
            .ReturnsAsync(expectedResponse);
    }

    private void SetupBlobService()
    {
        _mockBlobService.Setup(service => service.DeleteFileInStorage(It.IsAny<string>()));
    }

    private void SetupStringLocalizer(string expectedErrorMsg)
    {
        _mockStringLocalizerFailedToDelete.Setup(localizer => localizer["FailedToDeleteAudio"])
            .Returns(new LocalizedString("FailedToDeleteAudio", expectedErrorMsg));
    }
}