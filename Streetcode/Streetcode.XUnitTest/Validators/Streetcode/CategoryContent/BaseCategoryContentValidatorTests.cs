using FluentValidation.TestHelper;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.Validators.Streetcode.Art;
using Streetcode.BLL.Validators.Streetcode.CategoryContent;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.Validators.Streetcode.CategoryContent;

public class BaseCategoryContentValidatorTests
{
    private readonly MockFailedToValidateLocalizer _mockValidationLocalizer;
    private readonly MockFieldNamesLocalizer _mockNamesLocalizer;
    private readonly BaseCategoryContentValidator _validator;

    public BaseCategoryContentValidatorTests()
    {
        _mockValidationLocalizer = new MockFailedToValidateLocalizer();
        _mockNamesLocalizer = new MockFieldNamesLocalizer();
        _validator = new BaseCategoryContentValidator(_mockValidationLocalizer, _mockNamesLocalizer);
    }

    [Fact]
    public void ShouldReturnSuccess_WhenAllFieldsAreValid()
    {
        // Arrange
        var category = new StreetcodeCategoryContentDTO()
        {
            Text = "Some very important text",
        };

        // Act
        var result = _validator.Validate(category);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ShouldReturnError_WhenTextLengthIsMoreThan6000()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["MaxLength", _mockNamesLocalizer["CategoryContent"], BaseCategoryContentValidator.TextMaxLength];
        var category = new StreetcodeCategoryContentDTO()
        {
            Text = new string('*', BaseCategoryContentValidator.TextMaxLength + 1),
        };

        // Act
        var result = _validator.TestValidate(category);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Text)
            .WithErrorMessage(expectedError);
    }
}