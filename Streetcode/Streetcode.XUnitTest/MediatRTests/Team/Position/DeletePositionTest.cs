using System.Linq.Expressions;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Team.Position.Delete;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Team.Position;

public class DeletePositionTest
{
    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizer;

    public DeletePositionTest()
    {
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockLogger = new Mock<ILoggerService>();
        _mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        _mockLocalizer.Setup(x => x["CannotFindPositionWithCorrespondingId", It.IsAny<object[]>()]).Returns(new LocalizedString("CannotFindPositionWithCorrespondingId", "CannotFindPositionWithCorrespondingId"));
    }

    [Fact]
    public async Task ShouldDeleteSuccessfully()
    {
        // Arrange
        var testPositions = DeletePositions();
        SetupMockRepositoryGetFirstOrDefault(testPositions);
        SetupMockRepositorySaveChangesReturns(1);

        var handler = new DeleteTeamPositionHandler(_mockRepository.Object, _mockLogger.Object, _mockLocalizer.Object);

        // Act
        var result = await handler.Handle(new DeleteTeamPositionCommand(testPositions.Id), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.True(result.IsSuccess));

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
        var expectedError = "CannotFindPositionWithCorrespondingId";
        SetupMockRepositoryGetFirstOrDefault(null);

        var handler = new DeleteTeamPositionHandler(_mockRepository.Object, _mockLogger.Object, _mockLocalizer.Object);

        // Act
        var result = await handler.Handle(new DeleteTeamPositionCommand(testPositions.Id), CancellationToken.None);

        // Assert
        Assert.Equal(expectedError, result.Errors[0].Message);
        _mockRepository.Verify(x => x.PositionRepository.Delete(It.IsAny<Positions>()), Times.Never);
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
        _mockRepository
            .Setup(x => x.PositionRepository
                .GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Positions, bool>>>(), null))
            .ReturnsAsync(position);
    }

    private void SetupMockRepositorySaveChangesReturns(int number)
    {
        _mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(number);
    }
}
