using FluentValidation.TestHelper;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.Validators.Timeline.HistoricalContext;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.Validators.Timeline;

public class HistoricalContextValidatorTests
{
    private readonly MockFailedToValidateLocalizer _mockValidationLocalizer;
    private readonly MockFieldNamesLocalizer _mockNamesLocalizer;
    private readonly BaseHistoricalContextValidator _validator;

    public HistoricalContextValidatorTests()
    {
        _mockValidationLocalizer = new MockFailedToValidateLocalizer();
        _mockNamesLocalizer = new MockFieldNamesLocalizer();
        _validator = new BaseHistoricalContextValidator(_mockValidationLocalizer, _mockNamesLocalizer);
    }

    [Fact]
    public void ShouldReturnSuccessResult_WhenHistoricalContextIsValid()
    {
        // Arrange
        var context = GetValidHistoricalContext();

        // Act
        var result = _validator.Validate(context);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ShouldReturnError_WhenTitleIsEmpty()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["IsRequired", _mockNamesLocalizer["Title"]];
        var context = GetValidHistoricalContext();
        context.Title = string.Empty;

        // Act
        var result = _validator.TestValidate(context);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenTitleLengthIsMoreThan50()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["MaxLength", _mockNamesLocalizer["Title"], BaseHistoricalContextValidator.MaxTitleLength];
        var context = GetValidHistoricalContext();
        context.Title = new string('*', BaseHistoricalContextValidator.MaxTitleLength + 1);

        // Act
        var result = _validator.TestValidate(context);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage(expectedError);
    }

    private static HistoricalContextDTO GetValidHistoricalContext()
    {
        return new HistoricalContextDTO()
        {
            Id = 1,
            Title = "Title",
        };
    }
}