using FluentAssertions;
using Moq;
using Streetcode.BLL.Interfaces.Text;
using Streetcode.BLL.MediatR.Streetcode.Text.GetParsed;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Texts;

public class UpdateParsedTextAdminPreviewTest
{
    private readonly Mock<ITextService> _mockTextService;
    private readonly UpdateParsedTextAdminPreviewHandler _handler;

    public UpdateParsedTextAdminPreviewTest()
    {
        _mockTextService = new Mock<ITextService>();
        _handler = new UpdateParsedTextAdminPreviewHandler(_mockTextService.Object);
    }

    [Theory]
    [InlineData("qwerty")]
    public async Task ShouldUpdateParsedAdminPreviewSuccessfully_WhenTextParsed(string textToParse)
    {
        // Arrange
        var parsedText = GetParsedText(textToParse);
        var request = GetRequest(textToParse);

        SetupMockTextService(request, parsedText);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(parsedText);
        _mockTextService.Verify(x => x.AddTermsTag(request.TextToParse), Times.Once);
    }

    [Theory]
    [InlineData("qwerty")]
    public async Task ShouldUpdateParsedAdminPreviewSuccessfully_WithCorrectDataType(string textToParse)
    {
        // Arrange
        var parsedText = GetParsedText(textToParse);
        var request = GetRequest(textToParse);

        SetupMockTextService(request, parsedText);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrDefault.Should().BeOfType<string>();
    }

    private static string GetParsedText(string textToParse)
    {
        return $"<parsed-version-of-text>{textToParse}</parsed-version-of-text>";
    }

    private static UpdateParsedTextForAdminPreviewCommand GetRequest(string textToParse)
    {
        return new UpdateParsedTextForAdminPreviewCommand(textToParse);
    }

    private void SetupMockTextService(UpdateParsedTextForAdminPreviewCommand request, string parsedText)
    {
        _mockTextService
            .Setup(x => x.AddTermsTag(request.TextToParse))
            .ReturnsAsync(parsedText);
    }
}
