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
    private readonly MockFieldNamesLocalizer _mockNamesLocalizer;
    private readonly MockFailedToValidateLocalizer _mockValidationLocalizer;
    private readonly StreetcodeArtSlideValidator _validator;

    public StreetcodeArtSlideValidatorTests()
    {
        _mockNamesLocalizer = new MockFieldNamesLocalizer();
        _mockValidationLocalizer = new MockFailedToValidateLocalizer();
        _validator = new StreetcodeArtSlideValidator(_mockValidationLocalizer, _mockNamesLocalizer);
    }

    [Fact]
    public void ShouldReturnSuccessResult_WhenAllFieldsAreValid()
    {
        // Arrange
        var slide = GetArtSlideDto();

        // Act
        var result = _validator.Validate(slide);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ShouldReturnError_WhenArtsAreEmpty()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["CannotBeEmpty", _mockNamesLocalizer["StreetcodeArts"]];
        var slide = GetArtSlideDto();
        slide.StreetcodeArts = new List<StreetcodeArtCreateUpdateDTO>();

        // Act
        var result = _validator.TestValidate(slide);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.StreetcodeArts)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenTemplateIsInvalid()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["Invalid", _mockNamesLocalizer["Template"]];
        var slide = GetArtSlideDto();
        slide.Template = (StreetcodeArtSlideTemplate)100;

        // Act
        var result = _validator.TestValidate(slide);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Template)
            .WithErrorMessage(expectedError);
    }

    private StreetcodeArtSlideCreateUpdateDTO GetArtSlideDto()
    {
        return new StreetcodeArtSlideCreateUpdateDTO()
        {
            Index = 5,
            ModelState = ModelState.Created,
            SlideId = 5,
            Template = StreetcodeArtSlideTemplate.OneToTwo,
            StreetcodeArts = new List<StreetcodeArtCreateUpdateDTO>()
            {
                new StreetcodeArtCreateUpdateDTO(),
            },
        };
    }
}