using FluentValidation.TestHelper;
using Streetcode.BLL.DTO.Media.Create;
using Streetcode.BLL.Enums;
using Streetcode.BLL.Validators.Streetcode.Art;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.Validators.Streetcode.Art;

public class ArtCreateUpdateValidatorTests
{
    private readonly MockFailedToValidateLocalizer mockValidationLocalizer;
    private readonly MockFieldNamesLocalizer mockNamesLocalizer;
    private readonly ArtCreateUpdateDtoValidator validator;

    public ArtCreateUpdateValidatorTests()
    {
        this.mockValidationLocalizer = new MockFailedToValidateLocalizer();
        this.mockNamesLocalizer = new MockFieldNamesLocalizer();
        this.validator = new ArtCreateUpdateDtoValidator(this.mockValidationLocalizer, this.mockNamesLocalizer);
    }

    [Fact]
    public void ShouldReturnSuccessResult_WhenAllFieldsAreValid()
    {
        // Arrange
        var art = this.GetValidArt();

        // Act
        var result = this.validator.Validate(art);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ShouldReturnError_WhenTitleLengthIsMoreThan150()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["MaxLength", this.mockNamesLocalizer["ArtTitle"], ArtCreateUpdateDtoValidator.MaxTitleLength];
        var art = this.GetValidArt();
        art.Title = new string('*', ArtCreateUpdateDtoValidator.MaxTitleLength + 1);

        // Act
        var result = this.validator.TestValidate(art);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenTitleDescriptionIsMoreThan150()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["MaxLength", this.mockNamesLocalizer["ArtDescription"], ArtCreateUpdateDtoValidator.MaxDescriptionLength];
        var art = this.GetValidArt();
        art.Description = new string('*', ArtCreateUpdateDtoValidator.MaxDescriptionLength + 1);

        // Act
        var result = this.validator.TestValidate(art);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenModelStateIsInvalid()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["Invalid", this.mockNamesLocalizer["ModelState"]];
        var art = this.GetValidArt();
        art.ModelState = (ModelState)50;

        // Act
        var result = this.validator.TestValidate(art);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ModelState)
            .WithErrorMessage(expectedError);
    }
    private ArtCreateUpdateDto GetValidArt()
    {
        return new ArtCreateUpdateDto()
        {
            Title = "Title",
            Description = "Description",
            Id = 7,
            ImageId = 5,
            ModelState = ModelState.Created,
        };
    }
}