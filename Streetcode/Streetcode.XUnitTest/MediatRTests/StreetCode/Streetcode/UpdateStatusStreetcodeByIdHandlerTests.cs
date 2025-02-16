using System.Linq.Expressions;
using Moq;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.UpdateStatus;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Streetcode;

public class UpdateStatusStreetcodeByIdHandlerTests
{
    private readonly Mock<IRepositoryWrapper> _repositoryMock;
    private readonly Mock<ILoggerService> _loggerMock;
    private readonly MockFailedToUpdateLocalizer _mockFailedToUpdateLocalizer;
    private readonly MockCannotFindLocalizer _mockCannotFindLocalizer;
    private readonly UpdateStatusStreetcodeByIdHandler _handler;

    public UpdateStatusStreetcodeByIdHandlerTests()
    {
        _repositoryMock = new Mock<IRepositoryWrapper>();
        _loggerMock = new Mock<ILoggerService>();
        _mockCannotFindLocalizer = new MockCannotFindLocalizer();
        _mockFailedToUpdateLocalizer = new MockFailedToUpdateLocalizer();
        _handler = new UpdateStatusStreetcodeByIdHandler(
            _repositoryMock.Object,
            _loggerMock.Object,
            _mockFailedToUpdateLocalizer,
            _mockCannotFindLocalizer);
    }

    [Fact]
    public async Task Handle_WhenStreetcodeExists_UpdatesStatusSuccessfully()
    {
        // Arrange
        var request = new UpdateStatusStreetcodeByIdCommand(1, StreetcodeStatus.Published);
        var streetcode = new StreetcodeContent { Id = 1, Status = StreetcodeStatus.Draft };

        _repositoryMock
            .Setup(repo => repo.StreetcodeRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), null))
            .ReturnsAsync(streetcode);

        _repositoryMock
            .Setup(repo => repo.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.True(result.IsSuccess);
            Assert.Equal(StreetcodeStatus.Published, streetcode.Status);
            _repositoryMock.Verify(repo => repo.StreetcodeRepository.Update(streetcode), Times.Once);
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        });
    }

    [Fact]
    public async Task Handle_WhenStreetcodeNotFound_ReturnsError()
    {
        // Arrange
        var request = new UpdateStatusStreetcodeByIdCommand(999, StreetcodeStatus.Published);
        string expectedErrorKey = "CannotFindAnyStreetcodeWithCorrespondingId";
        string expectedErrorValue = _mockCannotFindLocalizer[expectedErrorKey, request.Id];

        _repositoryMock
            .Setup(repo => repo.StreetcodeRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), null))
            .ReturnsAsync((StreetcodeContent)null!);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.False(result.IsSuccess);
            Assert.Contains(expectedErrorValue, result.Errors[0].Message);
            _loggerMock.Verify(logger => logger.LogError(request, expectedErrorValue), Times.Once);
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        });
    }

    [Fact]
    public async Task Handle_WhenUpdateFails_ReturnsError()
    {
        // Arrange
        var request = new UpdateStatusStreetcodeByIdCommand(1, StreetcodeStatus.Published);
        var streetcode = new StreetcodeContent { Id = 1, Status = StreetcodeStatus.Draft };
        string expectedErrorKey = "FailedToUpdateStatusOfStreetcode";
        string expectedErrorValue = _mockFailedToUpdateLocalizer[expectedErrorKey];

        _repositoryMock
            .Setup(repo => repo.StreetcodeRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), null))
            .ReturnsAsync(streetcode);

        _repositoryMock
            .Setup(repo => repo.SaveChangesAsync())
            .ReturnsAsync(0);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.False(result.IsSuccess);
            Assert.Contains(expectedErrorValue, result.Errors[0].Message);
            _loggerMock.Verify(logger => logger.LogError(request, expectedErrorValue), Times.Once);
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        });
    }
}