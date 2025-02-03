using FluentValidation.TestHelper;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.Validators.Streetcode.Art;
using Streetcode.BLL.Validators.Streetcode.CategoryContent;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.Validators.Streetcode.CategoryContent;

public class BaseCategoryContentValidatorTests
{
    private readonly MockFailedToValidateLocalizer mockValidationLocalizer;
    private readonly MockFieldNamesLocalizer mockNamesLocalizer;
    private readonly BaseCategoryContentValidator validator;

    public BaseCategoryContentValidatorTests()
    {
        this.mockValidationLocalizer = new MockFailedToValidateLocalizer();
        this.mockNamesLocalizer = new MockFieldNamesLocalizer();
        this.validator = new BaseCategoryContentValidator(this.mockValidationLocalizer, this.mockNamesLocalizer);
    }

    [Fact]
    public void ShouldReturnSuccess_WhenAllFieldsAreValid()
    {
        // Arrange
        var category = new StreetcodeCategoryContentDto()
        {
            Text = "Some very important text",
        };

        // Act
        var result = this.validator.Validate(category);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ShouldReturnError_WhenTextLengthIsMoreThan6000()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["MaxLength", this.mockNamesLocalizer["CategoryContent"], BaseCategoryContentValidator.TextMaxLength];
        var category = new StreetcodeCategoryContentDto()
        {
            Text = new string('*', BaseCategoryContentValidator.TextMaxLength + 1),
        };

        // Act
        var result = this.validator.TestValidate(category);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Text);
    }
}