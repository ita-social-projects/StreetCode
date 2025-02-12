using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Fact.Delete;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Facts;

public class DeleteFactTest
{
    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly MockFailedToDeleteLocalizer _mockFailedToDeleteLocalizer;
    private readonly MockCannotFindLocalizer _mockCannotFindLocalizer;
    private readonly DeleteFactHandler _handler;

    public DeleteFactTest()
    {
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockLogger = new Mock<ILoggerService>();
        _mockFailedToDeleteLocalizer = new MockFailedToDeleteLocalizer();
        _mockCannotFindLocalizer = new MockCannotFindLocalizer();
        _handler = new DeleteFactHandler(
            _mockRepository.Object,
            _mockLogger.Object,
            _mockFailedToDeleteLocalizer,
            _mockCannotFindLocalizer);
    }

    [Theory]
    [InlineData(1)]
    public async Task ShouldDeleteSuccessfully_WhenFactExists(int factId)
    {
        // Arrange
        var fact = GetFact(factId);
        var request = GetRequest(factId);

        SetupMockGetFirstOrDefaultAsync(request, fact);
        SetupMockDeleteAndSaveChangesAsync(fact, 1);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _mockRepository.Verify(x => x.FactRepository.Delete(fact), Times.Once);
        _mockRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Theory]
    [InlineData(1)]
    public async Task ShouldDeleteSuccessfully_WithCorrectDataType(int factId)
    {
        // Arrange
        var fact = GetFact(factId);
        var request = GetRequest(factId);

        SetupMockGetFirstOrDefaultAsync(request, fact);
        SetupMockDeleteAndSaveChangesAsync(fact, 1);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrDefault.Should().BeOfType<Unit>();
    }

    [Theory]
    [InlineData(1)]
    public async Task ShouldDeleteFailingly_WhenFactNotExist(int factId)
    {
        // Arrange
        var request = GetRequest(factId);
        var expectedErrorMessage = _mockCannotFindLocalizer["CannotFindFactWithCorrespondingCategoryId", factId].Value;

        SetupMockGetFirstOrDefaultAsync(request, null);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should().Be(expectedErrorMessage);
        _mockLogger.Verify(x => x.LogError(request, expectedErrorMessage), Times.Once);
    }

    [Theory]
    [InlineData(1)]
    public async Task ShouldDeleteFailingly_WhenSaveChangesAsyncFailed(int factId)
    {
        // Arrange
        var fact = GetFact(factId);
        var request = GetRequest(factId);
        var expectedErrorMessage = _mockFailedToDeleteLocalizer["FailedToDeleteFact"].Value;

        SetupMockGetFirstOrDefaultAsync(request, fact);
        SetupMockDeleteAndSaveChangesAsync(fact, -1);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should().Be(expectedErrorMessage);
        _mockLogger.Verify(x => x.LogError(request, expectedErrorMessage), Times.Once);
    }

    private static Fact GetFact(int factId)
    {
        return new Fact()
        {
            Id = factId,
        };
    }

    private static DeleteFactCommand GetRequest(int factId)
    {
        return new DeleteFactCommand(factId);
    }

    private void SetupMockGetFirstOrDefaultAsync(DeleteFactCommand request, Fact? getFirstOrDefaultAsyncResult)
    {
        _mockRepository
            .Setup(x => x.FactRepository.GetFirstOrDefaultAsync(
                f => f.Id == request.Id,
                It.IsAny<Func<IQueryable<Fact>, IIncludableQueryable<Fact, object>>>()))
            .ReturnsAsync(getFirstOrDefaultAsyncResult);
    }

    private void SetupMockDeleteAndSaveChangesAsync(Fact fact, int saveChangesResult)
    {
        _mockRepository
            .Setup(x => x.FactRepository.Delete(fact));
        _mockRepository
            .Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(saveChangesResult);
    }
}
