using System.Linq.Expressions;
using Moq;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.RelatedFigure.Сreate;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;
using Entities = Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.XUnitTest.MediatRTests.Streetcode.RelatedFigure;

public class CreateRelatedFigureHandlerTests
{
    private readonly Mock<IRepositoryWrapper> _repositoryMock;
    private readonly Mock<ILoggerService> _loggerMock;
    private readonly MockNoSharedResourceLocalizer _mockNoShared;
    private readonly MockFailedToCreateLocalizer _mockFailedToCreateLocalizer;
    private readonly CreateRelatedFigureHandler _handler;

    public CreateRelatedFigureHandlerTests()
    {
        _repositoryMock = new Mock<IRepositoryWrapper>();
        _loggerMock = new Mock<ILoggerService>();
        _mockNoShared = new MockNoSharedResourceLocalizer();
        _mockFailedToCreateLocalizer = new MockFailedToCreateLocalizer();

        _handler = new CreateRelatedFigureHandler(
            _repositoryMock.Object,
            _loggerMock.Object,
            _mockNoShared,
            _mockFailedToCreateLocalizer);
    }

    [Fact]
    public async Task Handle_WhenRelationCreatedSuccessfully_ReturnsSuccess()
    {
        // Arrange
        var request = new CreateRelatedFigureCommand(1, 2);
        SetupMocksForExistingStreetcodes(request.ObserverId, request.TargetId);
        SetupMocksForNonExistingRelation();
        _repositoryMock.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.True(result.IsSuccess);
            _repositoryMock.Verify(repo => repo.RelatedFigureRepository.CreateAsync(It.IsAny<Entities.RelatedFigure>()), Times.Once);
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        });
    }

    [Fact]
    public async Task Handle_WhenObserverStreetcodeDoesNotExist_ReturnsError()
    {
        // Arrange
        var request = new CreateRelatedFigureCommand(1, 2);
        SetupMocksForNonExistingStreetcode(request.ObserverId);
        var expectedError = _mockNoShared["NoExistingStreetcodeWithId", request.ObserverId];

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.False(result.IsSuccess);
            Assert.Contains(expectedError, result.Errors.Single().Message);
            _loggerMock.Verify(logger => logger.LogError(request, expectedError), Times.Once);
        });
    }

    [Fact]
    public async Task Handle_WhenTargetStreetcodeDoesNotExist_ReturnsError()
    {
        // Arrange
        var request = new CreateRelatedFigureCommand(1, 2);
        SetupMocksForExistingStreetcodes(request.ObserverId);
        SetupMocksForNonExistingStreetcode(request.TargetId);
        var expectedError = _mockNoShared["NoExistingStreetcodeWithId", request.TargetId];

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.False(result.IsSuccess);
            Assert.Contains(expectedError, result.Errors.Single().Message);
            _loggerMock.Verify(logger => logger.LogError(request, expectedError), Times.Once);
        });
    }

    [Fact]
    public async Task Handle_WhenRelationAlreadyExists_ReturnsError()
    {
        // Arrange
        var request = new CreateRelatedFigureCommand(1, 2);
        SetupMocksForExistingStreetcodes(request.ObserverId, request.TargetId);
        SetupMocksForExistingRelation(request.ObserverId, request.TargetId);
        var expectedError = _mockFailedToCreateLocalizer["TheStreetcodesAreAlreadyLinked"];

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.False(result.IsSuccess);
            Assert.Contains(expectedError, result.Errors.Single().Message);
            _loggerMock.Verify(logger => logger.LogError(request, expectedError), Times.Once);
            _repositoryMock.Verify(repo => repo.RelatedFigureRepository.Create(It.IsAny<Entities.RelatedFigure>()), Times.Never);
        });
    }

    [Fact]
    public async Task Handle_WhenSavingFails_ReturnsError()
    {
        // Arrange
        var request = new CreateRelatedFigureCommand(1, 2);
        SetupMocksForExistingStreetcodes(request.ObserverId, request.TargetId);
        SetupMocksForNonExistingRelation();
        _repositoryMock.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(0);
        var expectedError = _mockFailedToCreateLocalizer["FailedToCreateRelation"];

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.False(result.IsSuccess);
            Assert.Contains(expectedError, result.Errors.Single().Message);
            _loggerMock.Verify(logger => logger.LogError(request, expectedError), Times.Once);
        });
    }

    private void SetupMocksForExistingStreetcodes(params int[] streetcodeIds)
    {
        foreach (var id in streetcodeIds)
        {
            _repositoryMock
                .Setup(repo => repo.StreetcodeRepository.GetFirstOrDefaultAsync(It.Is<Expression<Func<StreetcodeContent, bool>>>(expr => expr.Compile().Invoke(new StreetcodeContent { Id = id })), null))
                .ReturnsAsync(new StreetcodeContent { Id = id });
        }
    }

    private void SetupMocksForNonExistingStreetcode(int streetcodeId)
    {
        _repositoryMock
            .Setup(repo => repo.StreetcodeRepository.GetFirstOrDefaultAsync(It.Is<Expression<Func<StreetcodeContent, bool>>>(expr => expr.Compile().Invoke(new StreetcodeContent { Id = streetcodeId })), null))
            .ReturnsAsync((StreetcodeContent)null!);
    }

    private void SetupMocksForExistingRelation(int observerId, int targetId)
    {
        _repositoryMock
            .Setup(repo => repo.RelatedFigureRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Entities.RelatedFigure, bool>>>(), null))
            .ReturnsAsync(new DAL.Entities.Streetcode.RelatedFigure { ObserverId = observerId, TargetId = targetId });
    }

    private void SetupMocksForNonExistingRelation()
    {
        _repositoryMock
            .Setup(repo => repo.RelatedFigureRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Entities.RelatedFigure, bool>>>(), null))
            .ReturnsAsync((DAL.Entities.Streetcode.RelatedFigure)null!);
    }
}
