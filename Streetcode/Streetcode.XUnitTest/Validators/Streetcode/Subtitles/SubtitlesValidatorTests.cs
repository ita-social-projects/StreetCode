using FluentValidation.TestHelper;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.Validators.Streetcode.Subtitles;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.Validators.Streetcode.Subtitles;

public class SubtitlesValidatorTests
{
    private readonly MockFieldNamesLocalizer _mockNamesLocalizer;
    private readonly MockFailedToValidateLocalizer _mockValidationLocalizer;
    private readonly BaseSubtitleValidator _validator;

    public SubtitlesValidatorTests()
    {
        _mockNamesLocalizer = new MockFieldNamesLocalizer();
        _mockValidationLocalizer = new MockFailedToValidateLocalizer();
        _validator = new BaseSubtitleValidator(_mockValidationLocalizer, _mockNamesLocalizer);
    }

    [Fact]
    public void ShouldReturnSuccessResult_WhenSubtitleIsValid()
    {
       // Arrange
       var subtitle = GetValidSubtitle();

       // Act
       var result = _validator.Validate(subtitle);

       // Assert
       Assert.True(result.IsValid);
    }

    [Fact]
    public void ShouldReturnError_WhenSubtitleLengthIsMoreThan255()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["MaxLength", _mockNamesLocalizer["SubtitleText"], BaseSubtitleValidator.SubtitleMaxLength];
        var subtitle = GetValidSubtitle();
        subtitle.SubtitleText = new string('s', BaseSubtitleValidator.SubtitleMaxLength + 1);

        // Act
        var result = _validator.TestValidate(subtitle);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.SubtitleText)
            .WithErrorMessage(expectedError);
    }

    private SubtitleCreateUpdateDTO GetValidSubtitle()
    {
        return new SubtitleCreateDTO()
        {
            SubtitleText = "Subtitle text",
        };
    }
}