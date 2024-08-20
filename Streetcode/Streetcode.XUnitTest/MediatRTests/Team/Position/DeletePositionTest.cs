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
    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<ILoggerService> _mockLogger;

    public DeletePositionTest()
    {
        this._mockRepository = new Mock<IRepositoryWrapper>();
        this._mockLogger = new Mock<ILoggerService>();
    }

    [Fact]
    public async Task ShouldDeleteSuccessfully()
    {
        // Arrange
        var testPositions = DeletePositions();
        this.SetupMockRepositoryGetFirstOrDefault(testPositions);
        this.SetupMockRepositorySaveChangesReturns(1);

        var handler = new DeleteTeamPositionHandler(this._mockRepository.Object, this._mockLogger.Object);

        // Act
        var result = await handler.Handle(new DeleteTeamPositionCommand(testPositions.Id), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.True(result.IsSuccess));

        this._mockRepository.Verify(
            x => x.PositionRepository.Delete(It.Is<Positions>(x => x.Id == testPositions.Id)),
            Times.Once);
        this._mockRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ShouldThrowException_IdNotExisting()
    {
        // Arrange
        var testPositions = DeletePositions();
        var expectedError = $"No position found by entered Id - {testPositions.Id}";
        this.SetupMockRepositoryGetFirstOrDefault(null);

        var handler = new DeleteTeamPositionHandler(this._mockRepository.Object, this._mockLogger.Object);

        // Act
        var result = await handler.Handle(new DeleteTeamPositionCommand(testPositions.Id), CancellationToken.None);

        // Assert
        Assert.Equal(expectedError, result.Errors[0].Message);
        this._mockRepository.Verify(x => x.PositionRepository.Delete(It.IsAny<Positions>()), Times.Never);
    }

    private static Positions DeletePositions()
    {
        return new Positions
        {
            Id = 1,
        };
    }

    private void SetupMockRepositoryGetFirstOrDefault(Positions? position)
    {
        this._mockRepository
            .Setup(x => x.PositionRepository
                .GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Positions, bool>>>(), null))
            .ReturnsAsync(position);
    }

    private void SetupMockRepositorySaveChangesReturns(int number)
    {
        this._mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(number);
    }
}
