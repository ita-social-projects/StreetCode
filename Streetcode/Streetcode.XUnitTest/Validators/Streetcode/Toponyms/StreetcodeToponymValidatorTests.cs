using FluentValidation.TestHelper;
using Streetcode.BLL.DTO.Toponyms;
using Streetcode.BLL.Enums;
using Streetcode.BLL.Validators.Streetcode.TimelineItem;
using Streetcode.BLL.Validators.Streetcode.Toponyms;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.Validators.Streetcode.Toponyms;

public class StreetcodeToponymValidatorTests
{
    private readonly MockFieldNamesLocalizer _mockNamesLocalizer;
    private readonly MockFailedToValidateLocalizer _mockValidationLocalizer;
    private readonly StreetcodeToponymValidator _validator;

    public StreetcodeToponymValidatorTests()
    {
        _mockNamesLocalizer = new MockFieldNamesLocalizer();
        _mockValidationLocalizer = new MockFailedToValidateLocalizer();
        _validator = new StreetcodeToponymValidator(_mockValidationLocalizer, _mockNamesLocalizer);
    }

    [Fact]
    public void ShouldReturnSuccessResult_WhenTimelineIsValid()
    {
        // Arrange
        var validTimeline = GetValidToponym();

        // Act
        var result = _validator.TestValidate(validTimeline);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ShouldReturnValidationError_WhenStreetNameExceedsMaxLength()
    {
        // Arrange
        var validTimeline = GetValidToponym();
        validTimeline.StreetName = new string('a', StreetcodeToponymValidator.StreetNameMaxLength + 1);
        var expectedError = _mockValidationLocalizer["MaxLength",
            _mockNamesLocalizer["StreetName"], StreetcodeToponymValidator.StreetNameMaxLength];

        // Act
        var result = _validator.TestValidate(validTimeline);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto.StreetName)
            .WithErrorMessage(expectedError);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("  ")]
    public void ShouldReturnValidationError_WhenStreetNameIsEmpty(string invalidTitle)
    {
        // Arrange
        var toponym = GetValidToponym();
        toponym.StreetName = invalidTitle;
        var expectedError = _mockValidationLocalizer["CannotBeEmpty", _mockNamesLocalizer["StreetName"]];

        // Act
        var result = _validator.TestValidate(toponym);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.StreetName)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnValidationError_WhenModelStateIsNotInEnum()
    {
        // Arrange
        var timeline = GetValidToponym();
        timeline.ModelState = (ModelState)5;

        // Act
        var result = _validator.TestValidate(timeline);
        var expectedError = _mockValidationLocalizer["Invalid", _mockNamesLocalizer["ModelState"]];

        // Asssert
        result.ShouldHaveValidationErrorFor(x => x.ModelState)
            .WithErrorMessage(expectedError);
    }

    private static StreetcodeToponymCreateUpdateDto GetValidToponym()
    {
        return new StreetcodeToponymCreateUpdateDto()
        {
            StreetcodeId = 1,
            StreetName = "StreetName Test",
            ToponymId = 1,
            ModelState = (ModelState)1,
        };
    }
}