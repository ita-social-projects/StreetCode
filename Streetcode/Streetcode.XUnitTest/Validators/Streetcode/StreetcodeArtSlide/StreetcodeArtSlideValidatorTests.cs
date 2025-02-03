using FluentValidation.TestHelper;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.BLL.Enums;
using Streetcode.BLL.Validators.Streetcode.StreetcodeArtSlide;
using Streetcode.DAL.Enums;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.Validators.Streetcode.StreetcodeArtSlide;

public class StreetcodeArtSlideValidatorTests
{
    private readonly MockFieldNamesLocalizer mockNamesLocalizer;
    private readonly MockFailedToValidateLocalizer mockValidationLocalizer;
    private readonly StreetcodeArtSlideValidator validator;

    public StreetcodeArtSlideValidatorTests()
    {
        this.mockNamesLocalizer = new MockFieldNamesLocalizer();
        this.mockValidationLocalizer = new MockFailedToValidateLocalizer();
        this.validator = new StreetcodeArtSlideValidator(this.mockValidationLocalizer, this.mockNamesLocalizer);
    }

    [Fact]
    public void ShouldReturnSuccessResult_WhenAllFieldsAreValid()
    {
        // Arrange
        var slide = this.GetArtSlideDto();

        // Act
        var result = this.validator.Validate(slide);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ShouldReturnError_WhenArtsAreEmpty()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["CannotBeEmpty", this.mockNamesLocalizer["StreetcodeArts"]];
        var slide = this.GetArtSlideDto();
        slide.StreetcodeArts = new List<StreetcodeArtCreateUpdateDto>();

        // Act
        var result = this.validator.TestValidate(slide);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.StreetcodeArts)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenTemplateIsInvalid()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["Invalid", this.mockNamesLocalizer["Template"]];
        var slide = this.GetArtSlideDto();
        slide.Template = (StreetcodeArtSlideTemplate)100;

        // Act
        var result = this.validator.TestValidate(slide);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Template)
            .WithErrorMessage(expectedError);
    }

    private StreetcodeArtSlideCreateUpdateDto GetArtSlideDto()
    {
        return new StreetcodeArtSlideCreateUpdateDto()
        {
            Index = 5,
            ModelState = ModelState.Created,
            SlideId = 5,
            Template = StreetcodeArtSlideTemplate.OneToTwo,
            StreetcodeArts = new List<StreetcodeArtCreateUpdateDto>()
            {
                new StreetcodeArtCreateUpdateDto(),
            },
        };
    }
}