using FluentValidation.TestHelper;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.Validators.Streetcode.TimelineItem;
using Streetcode.BLL.Validators.Timeline.HistoricalContext;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.Validators.Timeline;

public class HistoricalContextValidatorTests
{
    private readonly MockFailedToValidateLocalizer mockValidationLocalizer;
    private readonly MockFieldNamesLocalizer mockNamesLocalizer;
    private readonly BaseHistoricalContextValidator validator;

    public HistoricalContextValidatorTests()
    {
        this.mockValidationLocalizer = new MockFailedToValidateLocalizer();
        this.mockNamesLocalizer = new MockFieldNamesLocalizer();
        this.validator = new BaseHistoricalContextValidator(this.mockValidationLocalizer, this.mockNamesLocalizer);
    }

    [Fact]
    public void ShouldReturnSuccessResult_WhenHistoricalContextIsValid()
    {
        // Arrange
        var context = this.GetValidHistoricalContext();

        // Act
        var result = this.validator.Validate(context);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ShouldReturnError_WhenTitleIsEmpty()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["IsRequired", this.mockNamesLocalizer["Title"]];
        var context = this.GetValidHistoricalContext();
        context.Title = string.Empty;

        // Act
        var result = this.validator.TestValidate(context);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenTitleLengthIsMoreThan50()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["MaxLength", this.mockNamesLocalizer["Title"], BaseHistoricalContextValidator.MaxTitleLength];
        var context = this.GetValidHistoricalContext();
        context.Title = new string('*', BaseHistoricalContextValidator.MaxTitleLength + 1);

        // Act
        var result = this.validator.TestValidate(context);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage(expectedError);
    }

    private HistoricalContextDto GetValidHistoricalContext()
    {
        return new HistoricalContextDto()
        {
            Id = 1,
            Title = "Title",
        };
    }
}