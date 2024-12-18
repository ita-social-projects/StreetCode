using FluentValidation.TestHelper;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.BLL.Validators.Streetcode.Facts;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.Validators.Streetcode.Facts;

public class BaseFactValidatorTests
{
    private readonly MockFailedToValidateLocalizer mockValidationLocalizer;
    private readonly MockFieldNamesLocalizer mockNamesLocalizer;
    private readonly BaseFactValidator validator;

    public BaseFactValidatorTests()
    {
        this.mockValidationLocalizer = new MockFailedToValidateLocalizer();
        this.mockNamesLocalizer = new MockFieldNamesLocalizer();
        this.validator = new BaseFactValidator(this.mockValidationLocalizer, this.mockNamesLocalizer);
    }

    [Fact]
    public void ShouldReturnSuccessResult_WhenAllFieldsAreValid()
    {
        // Arrange
        var fact = this.GetValidFactDto();

        // Act
        var result = this.validator.Validate(fact);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ShouldReturnError_WhenTitleIsEmpty()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["CannotBeEmpty", this.mockNamesLocalizer["FactTitle"]];
        var fact = this.GetValidFactDto();
        fact.Title = string.Empty;

        // Act
        var result = this.validator.TestValidate(fact);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenTitleLengthIsMoreThan68()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["MaxLength", this.mockNamesLocalizer["FactTitle"], BaseFactValidator.TitleMaxLength];
        var fact = this.GetValidFactDto();
        fact.Title = new string('t', BaseFactValidator.TitleMaxLength + 1);

        // Act
        var result = this.validator.TestValidate(fact);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenContentIsEmpty()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["CannotBeEmpty", this.mockNamesLocalizer["FactContent"]];
        var fact = this.GetValidFactDto();
        fact.FactContent = string.Empty;

        // Act
        var result = this.validator.TestValidate(fact);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FactContent)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenContentLengthIsMoreThan600()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["MaxLength", this.mockNamesLocalizer["FactContent"], BaseFactValidator.ContentMaxLength];
        var fact = this.GetValidFactDto();
        fact.FactContent = new string('t', BaseFactValidator.ContentMaxLength + 1);

        // Act
        var result = this.validator.TestValidate(fact);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FactContent)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenImageDescriptionLengthIsMoreThan200()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["MaxLength", this.mockNamesLocalizer["FactImageDescription"], BaseFactValidator.ImageDescriptionMaxLength];
        var fact = this.GetValidFactDto();
        fact.ImageDescription = new string('t', BaseFactValidator.ImageDescriptionMaxLength + 1);

        // Act
        var result = this.validator.TestValidate(fact);

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