using FluentValidation.TestHelper;
using Streetcode.BLL.DTO.Media.Create;
using Streetcode.BLL.Enums;
using Streetcode.BLL.Validators.Streetcode.Art;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.Validators.Streetcode.Art;

public class ArtCreateUpdateValidatorTests
{
    private readonly MockFailedToValidateLocalizer _mockValidationLocalizer;
    private readonly MockFieldNamesLocalizer _mockNamesLocalizer;
    private readonly ArtCreateUpdateDTOValidator _validator;

    public ArtCreateUpdateValidatorTests()
    {
        _mockValidationLocalizer = new MockFailedToValidateLocalizer();
        _mockNamesLocalizer = new MockFieldNamesLocalizer();
        _validator = new ArtCreateUpdateDTOValidator(_mockValidationLocalizer, _mockNamesLocalizer);
    }

    [Fact]
    public void ShouldReturnSuccessResult_WhenAllFieldsAreValid()
    {
        // Arrange
        var art = GetValidArt();

        // Act
        var result = _validator.Validate(art);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ShouldReturnError_WhenTitleLengthIsMoreThan150()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["MaxLength", _mockNamesLocalizer["ArtTitle"], ArtCreateUpdateDTOValidator.MaxTitleLength];
        var art = GetValidArt();
        art.Title = new string('*', ArtCreateUpdateDTOValidator.MaxTitleLength + 1);

        // Act
        var result = _validator.TestValidate(art);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenTitleDescriptionIsMoreThan150()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["MaxLength", _mockNamesLocalizer["ArtDescription"], ArtCreateUpdateDTOValidator.MaxDescriptionLength];
        var art = GetValidArt();
        art.Description = new string('*', ArtCreateUpdateDTOValidator.MaxDescriptionLength + 1);

        // Act
        var result = _validator.TestValidate(art);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenModelStateIsInvalid()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["Invalid", _mockNamesLocalizer["ModelState"]];
        var art = GetValidArt();
        art.ModelState = (ModelState)50;

        // Act
        var result = _validator.TestValidate(art);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ModelState)
            .WithErrorMessage(expectedError);
    }

    private static ArtCreateUpdateDTO GetValidArt()
    {
        return new ArtCreateUpdateDTO()
        {
            Title = "Title",
            Description = "Description",
            Id = 7,
            ImageId = 5,
            ModelState = ModelState.Created,
        };
    }
}