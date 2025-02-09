using System.Linq.Expressions;
using Moq;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.RelatedFigure.Delete;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Streetcode.RelatedFigure;

public class DeleteRelatedFigureHandlerTests
{
    private readonly Mock<IRepositoryWrapper> _repositoryMock;
    private readonly Mock<ILoggerService> _loggerMock;
    private readonly MockCannotFindLocalizer _mockCannotFindLocalizer;
    private readonly MockFailedToDeleteLocalizer _mockFailedToDeleteLocalizer;
    private readonly DeleteRelatedFigureHandler _handler;

    public DeleteRelatedFigureHandlerTests()
    {
        _repositoryMock = new Mock<IRepositoryWrapper>();
        _loggerMock = new Mock<ILoggerService>();
        _mockCannotFindLocalizer = new MockCannotFindLocalizer();
        _mockFailedToDeleteLocalizer = new MockFailedToDeleteLocalizer();

        _handler = new DeleteRelatedFigureHandler(
            _repositoryMock.Object,
            _loggerMock.Object,
            _mockFailedToDeleteLocalizer,
            _mockCannotFindLocalizer);
    }

    [Fact]
    public async Task Handle_WhenRelationExists_DeletesRelationAndReturnsSuccess()
    {
        // Arrange
        var observerId = 1;
        var targetId = 2;
        var relation = new DAL.Entities.Streetcode.RelatedFigure { ObserverId = observerId, TargetId = targetId };

        SetupMocksForHandler(relation, 1);

        var command = new DeleteRelatedFigureCommand(observerId, targetId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _repositoryMock.Verify(repo => repo.RelatedFigureRepository.Delete(relation), Times.Once);
        _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenRelationDoesNotExist_ReturnsError()
    {
        // Arrange
        var observerId = 1;
        var targetId = 2;
        string expectedErrorMessage = _mockCannotFindLocalizer["CannotFindRelationBetweenStreetcodesWithCorrespondingIds", observerId, targetId];

        SetupMocksForHandler(null, 0);

        var command = new DeleteRelatedFigureCommand(observerId, targetId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(expectedErrorMessage, result.Errors.Single().Message);
        _loggerMock.Verify(logger => logger.LogError(command, expectedErrorMessage), Times.Once);
        _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenSaveChangesFails_ReturnsError()
    {
        // Arrange
        var observerId = 1;
        var targetId = 2;
        var relation = new DAL.Entities.Streetcode.RelatedFigure { ObserverId = observerId, TargetId = targetId };
        string expectedErrorMessage = _mockFailedToDeleteLocalizer["FailedToDeleteRelation"];

        SetupMocksForHandler(relation, 0);

        var command = new DeleteRelatedFigureCommand(observerId, targetId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(expectedErrorMessage, result.Errors.Single().Message);
        _loggerMock.Verify(logger => logger.LogError(command, expectedErrorMessage), Times.Once);
        _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    private void SetupMocksForHandler(DAL.Entities.Streetcode.RelatedFigure? relation, int saveChangesResult)
    {
        _repositoryMock
            .Setup(repo => repo.RelatedFigureRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<DAL.Entities.Streetcode.RelatedFigure, bool>>>(), null))
            .ReturnsAsync(relation);

        _repositoryMock
            .Setup(repo => repo.SaveChangesAsync())
            .ReturnsAsync(saveChangesResult);
    }
}