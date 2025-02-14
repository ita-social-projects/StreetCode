using FluentValidation.TestHelper;
using Streetcode.BLL.DTO.Streetcode.TextContent.Text;
using Streetcode.BLL.Validators.Streetcode.Text;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.Validators.Streetcode.Text;

public class BaseTextValidatorTests
{
    private readonly MockFieldNamesLocalizer _mockNamesLocalizer;
    private readonly MockFailedToValidateLocalizer _mockValidationLocalizer;
    private readonly BaseTextValidator _validator;

    public BaseTextValidatorTests()
    {
        _mockNamesLocalizer = new MockFieldNamesLocalizer();
        _mockValidationLocalizer = new MockFailedToValidateLocalizer();
        _validator = new BaseTextValidator(_mockValidationLocalizer, _mockNamesLocalizer);
    }

    [Fact]
    public void ShouldReturnSuccessResult_WhenTextCreateDTOIsValid()
    {
        // Arrange
        var validText = GetValidTextCreateDto();

        // Act
        var result = _validator.TestValidate(validText);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ShouldReturnValidationError_WhenTitleExceedsMaxLength()
    {
        // Arrange
        var invalidText = GetValidTextCreateDto();
        invalidText.Title = new string('a', BaseTextValidator.TitleMaxLength + 1);
        var expectedError = _mockValidationLocalizer["MaxLength", _mockNamesLocalizer["TextTitle"], BaseTextValidator.TitleMaxLength];

        // Act
        var result = _validator.TestValidate(invalidText);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto.Title)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnValidationError_WhenTextExceedsMaxLength()
    {
        // Arrange
        var invalidText = GetValidTextCreateDto();
        invalidText.TextContent = new string('a', BaseTextValidator.TextMaxLength + 1);
        var expectedError = _mockValidationLocalizer["MaxLength", _mockNamesLocalizer["TextContent"], BaseTextValidator.TextMaxLength];

        // Act
        var result = _validator.TestValidate(invalidText);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto.TextContent)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnValidationError_WhenAdditionalTextExceedsMaxLength()
    {
        // Arrange
        var invalidText = GetValidTextCreateDto();
        invalidText.AdditionalText = new string('a', BaseTextValidator.AdditionalTextMaxLength + 1);
        var expectedError = _mockValidationLocalizer["MaxLength", _mockNamesLocalizer["AdditionalText"], BaseTextValidator.AdditionalTextMaxLength];

        // Act
        var result = _validator.TestValidate(invalidText);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto.AdditionalText)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnValidationError_WhenTextContentExistsButTitleIsEmpty()
    {
        // Arrange
        var invalidText = GetValidTextCreateDto();
        invalidText.Title = string.Empty;
        var expectedError = _mockValidationLocalizer["CannotBeEmptyWithCondition", _mockNamesLocalizer["TextTitle"], _mockNamesLocalizer["TextContent"]];

        // Act
        var result = _validator.TestValidate(invalidText);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnValidationError_WhenAdditionalTextExistsButTextContentIsEmpty()
    {
        // Arrange
        var invalidText = GetValidTextCreateDto();
        invalidText.TextContent = string.Empty;
        var expectedError = _mockValidationLocalizer["CannotBeEmptyWithCondition", _mockNamesLocalizer["TextContent"], _mockNamesLocalizer["AdditionalText"]];

        // Act
        var result = _validator.TestValidate(invalidText);

        // Assert
        result.ShouldHaveValidationErrorFor(dto => dto)
            .WithErrorMessage(expectedError);
    }

    private static BaseTextDTO GetValidTextCreateDto()
    {
        return new TextCreateDTO()
        {
            AdditionalText = "AdditionalText Test",
            TextContent = "TextContent",
            Title = "Title Test",
        };
    }
}