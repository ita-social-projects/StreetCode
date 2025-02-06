using System.Linq.Expressions;
using Moq;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.DeleteSoft;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Streetcode;

public class DeleteSoftStreetcodeHandlerTests
{
    private readonly Mock<IRepositoryWrapper> _repositoryMock;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly MockCannotFindLocalizer _mockCannotFindLocalizer;
    private readonly MockFailedToUpdateLocalizer _mockFailedToUpdateLocalizer;
    private readonly DeleteSoftStreetcodeHandler _handler;

    public DeleteSoftStreetcodeHandlerTests()
    {
        _repositoryMock = new Mock<IRepositoryWrapper>();
        _mockLogger = new Mock<ILoggerService>();
        _mockCannotFindLocalizer = new MockCannotFindLocalizer();
        _mockFailedToUpdateLocalizer = new MockFailedToUpdateLocalizer();

        _handler = new DeleteSoftStreetcodeHandler(
            _repositoryMock.Object,
            _mockLogger.Object,
            _mockCannotFindLocalizer,
            _mockFailedToUpdateLocalizer);
    }

    [Fact]
    public async Task Handle_WhenStreetcodeExists_UpdatesStatusAndUpdatedAt()
    {
        // Arrange
        var streetcodeId = 1;
        var testStreetcode = new StreetcodeContent { Id = streetcodeId, Status = StreetcodeStatus.Published, UpdatedAt = DateTime.UtcNow.AddDays(-1) };
        var previousUpdatedAt = testStreetcode.UpdatedAt;
        int testSaveChangesSuccess = 1;

        SetupRepositoryMocks(testStreetcode, testSaveChangesSuccess);

        // Act
        var result = await _handler.Handle(new DeleteSoftStreetcodeCommand(streetcodeId), CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(StreetcodeStatus.Deleted, testStreetcode.Status);
        Assert.True(testStreetcode.UpdatedAt > previousUpdatedAt);
        _repositoryMock.Verify(repo => repo.StreetcodeRepository.Update(testStreetcode), Times.Once);
        _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenStreetcodeNotFound_ReturnsError()
    {
        // Arrange
        var streetcodeId = 2;
        string expectedErrorKey = "CannotFindAnyStreetcodeWithCorrespondingId";
        string expectedErrorValue = _mockCannotFindLocalizer[expectedErrorKey, streetcodeId];

        SetupRepositoryMocks(null, 1);

        // Act
        var result = await _handler.Handle(new DeleteSoftStreetcodeCommand(streetcodeId), CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(expectedErrorValue, result.Errors.Single().Message);
        _mockLogger.Verify(logger => logger.LogError(It.IsAny<DeleteSoftStreetcodeCommand>(), It.IsAny<string>()), Times.Once);
        _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenSaveChangesFails_ReturnsUpdateError()
    {
        // Arrange
        var streetcodeId = 3;
        var testStreetcode = new StreetcodeContent { Id = streetcodeId, Status = StreetcodeStatus.Published };
        string expectedErrorKey = "FailedToChangeStatusOfStreetcodeToDeleted";
        string expectedErrorValue = _mockFailedToUpdateLocalizer[expectedErrorKey];

        SetupRepositoryMocks(testStreetcode, -1);

        // Act
        var result = await _handler.Handle(new DeleteSoftStreetcodeCommand(streetcodeId), CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(expectedErrorValue, result.Errors.Single().Message);
        _mockLogger.Verify(logger => logger.LogError(It.IsAny<DeleteSoftStreetcodeCommand>(), It.IsAny<string>()), Times.Once);
        _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    private void SetupRepositoryMocks(StreetcodeContent? streetcodeContent, int saveChangesVariable)
    {
        _repositoryMock
            .Setup(repo => repo.StreetcodeRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), null))
            .ReturnsAsync(streetcodeContent);

        if (streetcodeContent != null)
        {
            _repositoryMock.Setup(repo => repo.StreetcodeRepository.Update(streetcodeContent));
        }

        _repositoryMock
            .Setup(repo => repo.SaveChangesAsync())
            .ReturnsAsync(saveChangesVariable);
    }
}