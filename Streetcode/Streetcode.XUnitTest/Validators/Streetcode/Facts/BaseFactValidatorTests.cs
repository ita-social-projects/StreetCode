using FluentValidation.TestHelper;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.BLL.Validators.Streetcode.Facts;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.Validators.Streetcode.Facts;

public class BaseFactValidatorTests
{
    private readonly MockFailedToValidateLocalizer _mockValidationLocalizer;
    private readonly MockFieldNamesLocalizer _mockNamesLocalizer;
    private readonly BaseFactValidator _validator;

    public BaseFactValidatorTests()
    {
        _mockValidationLocalizer = new MockFailedToValidateLocalizer();
        _mockNamesLocalizer = new MockFieldNamesLocalizer();
        _validator = new BaseFactValidator(_mockValidationLocalizer, _mockNamesLocalizer);
    }

    [Fact]
    public void ShouldReturnSuccessResult_WhenAllFieldsAreValid()
    {
        // Arrange
        var fact = GetValidFactDto();

        // Act
        var result = _validator.Validate(fact);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ShouldReturnError_WhenTitleIsEmpty()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["CannotBeEmpty", _mockNamesLocalizer["FactTitle"]];
        var fact = GetValidFactDto();
        fact.Title = string.Empty;

        // Act
        var result = _validator.TestValidate(fact);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenTitleLengthIsMoreThan68()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["MaxLength", _mockNamesLocalizer["FactTitle"], BaseFactValidator.TitleMaxLength];
        var fact = GetValidFactDto();
        fact.Title = new string('t', BaseFactValidator.TitleMaxLength + 1);

        // Act
        var result = _validator.TestValidate(fact);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenContentIsEmpty()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["CannotBeEmpty", _mockNamesLocalizer["FactContent"]];
        var fact = GetValidFactDto();
        fact.FactContent = string.Empty;

        // Act
        var result = _validator.TestValidate(fact);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FactContent)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenContentLengthIsMoreThan600()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["MaxLength", _mockNamesLocalizer["FactContent"], BaseFactValidator.ContentMaxLength];
        var fact = GetValidFactDto();
        fact.FactContent = new string('t', BaseFactValidator.ContentMaxLength + 1);

        // Act
        var result = _validator.TestValidate(fact);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FactContent)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenImageDescriptionLengthIsMoreThan200()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["MaxLength", _mockNamesLocalizer["FactImageDescription"], BaseFactValidator.ImageDescriptionMaxLength];
        var fact = GetValidFactDto();
        fact.ImageDescription = new string('t', BaseFactValidator.ImageDescriptionMaxLength + 1);

        // Act
        var result = _validator.TestValidate(fact);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ImageDescription)
            .WithErrorMessage(expectedError);
    }

    private FactUpdateCreateDto GetValidFactDto()
    {
        return new StreetcodeFactCreateDTO()
        {
            FactContent = "FactContent",
            Title = "WOW",
        };
    }
}