using FluentValidation.TestHelper;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.Validators.Streetcode.Subtitles;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.Validators.Streetcode.Subtitles;

public class SubtitlesValidatorTests
{
    private readonly MockFieldNamesLocalizer mockNamesLocalizer;
    private readonly MockFailedToValidateLocalizer mockValidationLocalizer;
    private readonly BaseSubtitleValidator validator;

    public SubtitlesValidatorTests()
    {
        this.mockNamesLocalizer = new MockFieldNamesLocalizer();
        this.mockValidationLocalizer = new MockFailedToValidateLocalizer();
        this.validator = new BaseSubtitleValidator(this.mockValidationLocalizer, this.mockNamesLocalizer);
    }

    [Fact]
    public void ShouldReturnSuccessResult_WhenSubtitleIsValid()
    {
       // Arrange
       var subtitle = this.GetValidSubtitle();

       // Act
       var result = this.validator.Validate(subtitle);

       // Assert
       Assert.True(result.IsValid);
    }

    [Fact]
    public void ShouldReturnError_WhenSubtitleLengthIsMoreThan255()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["MaxLength", this.mockNamesLocalizer["SubtitleText"], BaseSubtitleValidator.SubtitleMaxLength];
        var subtitle = this.GetValidSubtitle();
        subtitle.SubtitleText = new string('s', BaseSubtitleValidator.SubtitleMaxLength + 1);

        // Act
        var result = this.validator.TestValidate(subtitle);

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