using FluentValidation.TestHelper;
using Streetcode.BLL.DTO.Timeline.Update;
using Streetcode.BLL.Enums;
using Streetcode.BLL.Validators.Streetcode.TimelineItem;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.Validators.Streetcode.Timeline;

public class HistoricalContextValidatorTests
{
    private readonly MockFieldNamesLocalizer _mockNamesLocalizer;
    private readonly MockFailedToValidateLocalizer _mockValidationLocalizer;
    private readonly HistoricalContextValidator _validator;

    public HistoricalContextValidatorTests()
    {
        _mockNamesLocalizer = new MockFieldNamesLocalizer();
        _mockValidationLocalizer = new MockFailedToValidateLocalizer();
        _validator = new HistoricalContextValidator(_mockValidationLocalizer, _mockNamesLocalizer);
    }

    [Fact]
    public void ShouldReturnSuccessResult_WhenTextCreateDTOIsValid()
    {
        // Arrange
        var validHistoricalContext = GetValidHistoricalContext();

        // Act
        var result = _validator.TestValidate(validHistoricalContext);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ShouldReturnValidationError_WhenTitleIsEmpty()
    {
        // Arrange
        var validHistoricalContext = GetValidHistoricalContext();
        validHistoricalContext.Title = string.Empty;

        // Act
        var result = _validator.TestValidate(validHistoricalContext);
        var expectedError = _mockValidationLocalizer["CannotBeEmpty", _mockNamesLocalizer["HistoricalContextTitle"]];

        // Asssert
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnValidationError_WhenTitleExceedsMaxLength()
    {
        // Arrange
        var validHistoricalContext = GetValidHistoricalContext();
        validHistoricalContext.Title = new string('a', HistoricalContextValidator.TitleMaxLength + 1);

        // Act
        var result = _validator.TestValidate(validHistoricalContext);
        var expectedError = _mockValidationLocalizer["MaxLength", _mockNamesLocalizer["HistoricalContextTitle"],
            HistoricalContextValidator.TitleMaxLength];

        // Asssert
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnValidationError_WhenModelStateIsNotInEnum()
    {
        // Arrange
        var validHistoricalContext = GetValidHistoricalContext();
        validHistoricalContext.ModelState = (ModelState)5;

        // Act
        var result = _validator.TestValidate(validHistoricalContext);
        var expectedError = _mockValidationLocalizer["Invalid", _mockNamesLocalizer["ModelState"]];

        // Asssert
        result.ShouldHaveValidationErrorFor(x => x.ModelState)
            .WithErrorMessage(expectedError);
    }

    private static HistoricalContextCreateUpdateDTO GetValidHistoricalContext()
    {
        return new HistoricalContextCreateUpdateDTO()
        {
            Title = "Title Test",
            ModelState = (ModelState)1,
            TimelineId = 1,
        };
    }
}