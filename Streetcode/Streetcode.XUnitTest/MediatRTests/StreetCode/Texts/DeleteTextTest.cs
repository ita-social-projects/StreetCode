using FluentAssertions;
using MediatR;
using Moq;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Text.Delete;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Texts;

public class DeleteTextTest
{
    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly MockFailedToDeleteLocalizer _mockFailedToDeleteLocalizer;
    private readonly MockCannotFindLocalizer _mockCannotFindLocalizer;
    private readonly DeleteTextHandler _handler;

    public DeleteTextTest()
    {
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockLogger = new Mock<ILoggerService>();
        _mockFailedToDeleteLocalizer = new MockFailedToDeleteLocalizer();
        _mockCannotFindLocalizer = new MockCannotFindLocalizer();
        _handler = new DeleteTextHandler(
            _mockRepository.Object,
            _mockLogger.Object,
            _mockFailedToDeleteLocalizer,
            _mockCannotFindLocalizer);
    }

    [Theory]
    [InlineData(1)]
    public async Task ShouldDeleteSuccessfully_WhenTextExists(int textId)
    {
        // Arrange
        var text = GetText(textId);
        var request = GetRequest(textId);

        MockHelpers.SetupMockTextGetFirstOrDefaultAsync(_mockRepository, text);
        SetupMockDeleteAndSaveChangesAsync(text, 1);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _mockRepository.Verify(x => x.TextRepository.Delete(text), Times.Once);
        _mockRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Theory]
    [InlineData(1)]
    public async Task ShouldDeleteSuccessfully_WithCorrectDataType(int termId)
    {
        // Arrange
        var text = GetText(termId);
        var request = GetRequest(termId);

        MockHelpers.SetupMockTextGetFirstOrDefaultAsync(_mockRepository, text);
        SetupMockDeleteAndSaveChangesAsync(text, 1);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrDefault.Should().BeOfType<Unit>();
    }

    [Theory]
    [InlineData(1)]
    public async Task ShouldDeleteFailingly_WhenTextNotExist(int textId)
    {
        // Arrange
        var request = GetRequest(textId);
        var expectedErrorMessage = _mockCannotFindLocalizer["CannotFindTextWithCorrespondingCategoryId", request.Id].Value;

        MockHelpers.SetupMockTextGetFirstOrDefaultAsync(_mockRepository, null);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should().Be(expectedErrorMessage);
        _mockLogger.Verify(x => x.LogError(request, expectedErrorMessage), Times.Once);
    }

    [Theory]
    [InlineData(1)]
    public async Task ShouldDeleteFailingly_WhenSaveChangesAsyncFailed(int textId)
    {
        // Arrange
        var text = GetText(textId);
        var request = GetRequest(textId);
        var expectedErrorMessage = _mockFailedToDeleteLocalizer["FailedToDeleteText"].Value;

        MockHelpers.SetupMockTextGetFirstOrDefaultAsync(_mockRepository, text);
        SetupMockDeleteAndSaveChangesAsync(text, -1);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Message.Should().Be(expectedErrorMessage);
        _mockLogger.Verify(x => x.LogError(request, expectedErrorMessage), Times.Once);
    }

    private static Text GetText(int textId)
    {
        return new Text()
        {
            Id = textId,
        };
    }

    private static DeleteTextCommand GetRequest(int textId)
    {
        return new DeleteTextCommand(textId);
    }

    private void SetupMockDeleteAndSaveChangesAsync(Text text, int saveChangesResult)
    {
        _mockRepository
            .Setup(x => x.TextRepository.Delete(text));
        _mockRepository
            .Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(saveChangesResult);
    }
}
