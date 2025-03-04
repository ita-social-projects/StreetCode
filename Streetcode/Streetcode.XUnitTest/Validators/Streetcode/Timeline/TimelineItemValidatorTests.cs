using FluentValidation;
using FluentValidation.TestHelper;
using Moq;
using Streetcode.BLL.DTO.Timeline.Update;
using Streetcode.BLL.Enums;
using Streetcode.BLL.Validators.Streetcode.TimelineItem;
using Streetcode.DAL.Enums;
using Streetcode.XUnitTest.Mocks;
using Xunit;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Streetcode.XUnitTest.Validators.Streetcode.Timeline;

public class TimelineItemValidatorTests
{
    private readonly MockFieldNamesLocalizer _mockNamesLocalizer;
    private readonly MockFailedToValidateLocalizer _mockValidationLocalizer;
    private readonly Mock<HistoricalContextValidator> _mockHistoricalContextValidator;
    private readonly TimelineItemValidator _validator;

    public TimelineItemValidatorTests()
    {
        _mockNamesLocalizer = new MockFieldNamesLocalizer();
        _mockValidationLocalizer = new MockFailedToValidateLocalizer();
        _mockHistoricalContextValidator = new Mock<HistoricalContextValidator>(_mockValidationLocalizer, _mockNamesLocalizer);
        _validator = new TimelineItemValidator(_mockHistoricalContextValidator.Object, _mockValidationLocalizer, _mockNamesLocalizer);
    }

    [Fact]
    public void ShouldReturnSuccessResult_WhenTimelineIsValid()
    {
        // Arrange
        var validTimeline = GetValidTimelineItem();

        // Act
        var result = _validator.TestValidate(validTimeline);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ShouldReturnValidationError_WhenTitleExceedsMaxLength()
    {
        // Arrange
        var validTimeline = GetValidTimelineItem();
        validTimeline.Title = new string('a', TimelineItemValidator.TitleMaxLength + 1);
        var expectedError = _mockValidationLocalizer["MaxLength", _mockNamesLocalizer["TimelineItemTitle"], TimelineItemValidator.TitleMaxLength];

        // Act
        var result = _validator.TestValidate(validTimeline);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto.Title)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnValidationError_WhenDescriptionExceedsMaxLength()
    {
        // Arrange
        var timeline = GetValidTimelineItem();
        timeline.Description = new string('a', TimelineItemValidator.DescriptionMaxLength + 1);
        var expectedError = _mockValidationLocalizer["MaxLength", _mockNamesLocalizer["TimelineItemDescription"], TimelineItemValidator.DescriptionMaxLength];

        // Act
        var result = _validator.TestValidate(timeline);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage(expectedError);
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    public void ShouldReturnValidationError_WhenDescriptionIsEmpty(string invalidDescription)
    {
        // Arrange
        var timeline = GetValidTimelineItem();
        timeline.Description = invalidDescription;
        var expectedError = _mockValidationLocalizer["CannotBeEmpty", _mockNamesLocalizer["TimelineItemDescription"]];

        // Act
        var result = _validator.TestValidate(timeline);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage(expectedError);
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    public void ShouldReturnValidationError_WhenTitleIsEmpty(string invalidTitle)
    {
        // Arrange
        var timeline = GetValidTimelineItem();
        timeline.Title = invalidTitle;
        var expectedError = _mockValidationLocalizer["CannotBeEmpty", _mockNamesLocalizer["TimelineItemTitle"]];

        // Act
        var result = _validator.TestValidate(timeline);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnValidationError_WhenDateIsEmpty()
    {
        // Arrange
        var timeline = GetValidTimelineItem();
        timeline.Date = default;
        var expectedError = _mockValidationLocalizer["CannotBeEmpty", _mockNamesLocalizer["TimelineItemDate"]];

        // Act
        var result = _validator.TestValidate(timeline);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Date)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnValidationError_WhenDateViewPatternIsNotInEnum()
    {
        // Arrange
        var timeline = GetValidTimelineItem();
        timeline.DateViewPattern = (DateViewPattern)5;

        // Act
        var result = _validator.TestValidate(timeline);
        var expectedError = _mockValidationLocalizer["Invalid", _mockNamesLocalizer["DateFormat"]];

        // Asssert
        result.ShouldHaveValidationErrorFor(x => x.DateViewPattern)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnValidationError_WhenModelStateIsNotInEnum()
    {
        // Arrange
        var timeline = GetValidTimelineItem();
        timeline.ModelState = (ModelState)5;

        // Act
        var result = _validator.TestValidate(timeline);
        var expectedError = _mockValidationLocalizer["Invalid", _mockNamesLocalizer["ModelState"]];

        // Asssert
        result.ShouldHaveValidationErrorFor(x => x.ModelState)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void TimelineItemValidator_ShouldCallHistoricalContextValidator()
    {
        // Arrange
        _mockHistoricalContextValidator
            .Setup(x => x.Validate(It.IsAny<ValidationContext<HistoricalContextCreateUpdateDTO>>()))
            .Returns(new ValidationResult());

        // Act
        var result = _validator.TestValidate(GetValidTimelineItem());

        // Assert
        _mockHistoricalContextValidator.Verify(
            x => x.Validate(It.IsAny<ValidationContext<HistoricalContextCreateUpdateDTO>>()), Times.Once);
    }

    private static TimelineItemCreateUpdateDTO GetValidTimelineItem()
    {
        return new TimelineItemCreateUpdateDTO()
        {
            Title = "Test Title",
            Description = "Description Test",
            Date = DateTime.Now,
            DateViewPattern = (DateViewPattern)1,
            HistoricalContexts = new List<HistoricalContextCreateUpdateDTO>()
            {
                new ()
                {
                    Id = 1,
                    Title = "Test Title",
                    TimelineId = 1,
                },
            },
            ModelState = (ModelState)1,
        };
    }
}