using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Term.Delete;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Terms;

public class DeleteTermTest
{
    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly MockFailedToDeleteLocalizer _mockFailedToDeleteLocalizer;
    private readonly MockCannotConvertNullLocalizer _mockCannotConvertNullLocalizer;
    private readonly DeleteTermHandler _handler;

    public DeleteTermTest()
    {
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockLogger = new Mock<ILoggerService>();
        _mockFailedToDeleteLocalizer = new MockFailedToDeleteLocalizer();
        _mockCannotConvertNullLocalizer = new MockCannotConvertNullLocalizer();
        _handler = new DeleteTermHandler(
            _mockRepository.Object,
            _mockLogger.Object,
            _mockFailedToDeleteLocalizer,
            _mockCannotConvertNullLocalizer);
    }

    [Theory]
    [InlineData(1)]
    public async Task ShouldDeleteSuccessfully_WhenTermExists(int termId)
    {
        // Arrange
        var term = GetTerm(termId);
        var request = GetRequest(termId);

        MockHelpers.SetupMockTermGetFirstOrDefaultAsync(_mockRepository, term);
        SetupMockDeleteAndSaveChangesAsync(term, 1);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _mockRepository.Verify(x => x.TermRepository.Delete(term), Times.Once);
        _mockRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Theory]
    [InlineData(1)]
    public async Task ShouldDeleteSuccessfully_WithCorrectDataType(int termId)
    {
        // Arrange
        var term = GetTerm(termId);
        var request = GetRequest(termId);

        MockHelpers.SetupMockTermGetFirstOrDefaultAsync(_mockRepository, term);
        SetupMockDeleteAndSaveChangesAsync(term, 1);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrDefault.Should().BeOfType<Unit>();
    }

    [Theory]
    [InlineData(1)]
    public async Task ShouldDeleteFailingly_WhenTermNotExist(int termId)
    {
        // Arrange
        var request = GetRequest(termId);
        var expectedErrorMessage = _mockCannotConvertNullLocalizer["CannotConvertNullToTerm"].Value;

        MockHelpers.SetupMockTermGetFirstOrDefaultAsync(_mockRepository, null);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should().Be(expectedErrorMessage);
        _mockLogger.Verify(x => x.LogError(request, expectedErrorMessage), Times.Once);
    }

    [Theory]
    [InlineData(1)]
    public async Task ShouldDeleteFailingly_WhenSaveChangesAsyncFailed(int termId)
    {
        // Arrange
        var term = GetTerm(termId);
        var request = GetRequest(termId);
        var expectedErrorMessage = _mockFailedToDeleteLocalizer["FailedToDeleteTerm"].Value;

        MockHelpers.SetupMockTermGetFirstOrDefaultAsync(_mockRepository, term);
        SetupMockDeleteAndSaveChangesAsync(term, -1);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should().Be(expectedErrorMessage);
        _mockLogger.Verify(x => x.LogError(request, expectedErrorMessage), Times.Once);
    }

    private static Term GetTerm(int termId)
    {
        return new Term()
        {
            Id = termId,
        };
    }

    private static DeleteTermCommand GetRequest(int termId)
    {
        return new DeleteTermCommand(termId);
    }

    private void SetupMockDeleteAndSaveChangesAsync(Term term, int saveChangesResult)
    {
        _mockRepository
            .Setup(x => x.TermRepository.Delete(term));
        _mockRepository
            .Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(saveChangesResult);
    }
}
