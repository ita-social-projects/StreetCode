using System.Linq.Expressions;
using Moq;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Team.Position.Delete;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Team.Position;

public class DeletePositionTest
{
    private Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<ILoggerService> _mockLogger;

    public DeletePositionTest()
    {
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockLogger = new Mock<ILoggerService>();
    }

    [Fact]
    public async Task ShouldDeleteSuccessfully()
    {
        // Arrange
        var testPositions = DeletePositions();
        SetupMockRepositoryGetFirstOrDefault(testPositions);
        SetupMockRepositorySaveChangesReturns(1);

        var handler = new DeleteTeamPositionHandler(_mockRepository.Object, _mockLogger.Object);

        // Act
        var result = await handler.Handle(new DeleteTeamPositionCommand(testPositions.Id),
            CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.True(result.IsSuccess)
        );

        _mockRepository.Verify(
            x => x.PositionRepository.Delete(It.Is<Positions>(x => x.Id == testPositions.Id)),
            Times.Once);
        _mockRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ShouldThrowException_IdNotExisting()
    {
        // Arrange
        var testPositions = DeletePositions();
        var expectedError = $"No position found by entered Id - {testPositions.Id}";
        SetupMockRepositoryGetFirstOrDefault(null);

        var handler = new DeleteTeamPositionHandler(_mockRepository.Object, _mockLogger.Object);

        // Act
        var result = await handler.Handle(new DeleteTeamPositionCommand(testPositions.Id), CancellationToken.None);

        // Assert
        Assert.Equal(expectedError, result.Errors.First().Message);
        _mockRepository.Verify(x => x.PositionRepository.Delete(It.IsAny<Positions>()), Times.Never);
    }

    private void SetupMockRepositoryGetFirstOrDefault(Positions position)
    {
        _mockRepository.Setup(x => x.PositionRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Positions, bool>>>(), null))
            .ReturnsAsync(position);
    }

    private void SetupMockRepositorySaveChangesException(string expectedError)
    {
        _mockRepository.Setup(x => x.SaveChanges()).Throws(new Exception(expectedError));
    }

    private void SetupMockRepositorySaveChangesReturns(int number)
    {
        _mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(number);
    }

    private static Positions DeletePositions()
    {
        return new Positions
        {
            Id = 1
        };
    }
}