using FluentValidation.TestHelper;
using Streetcode.BLL.DTO.Partners.Create;
using Streetcode.BLL.Validators.Partners.SourceLinks;
using Streetcode.DAL.Enums;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.Validators.Partner;

public class PartnerSourceLinkValidatorTests
{
    private readonly MockFailedToValidateLocalizer _mockValidationLocalizer;
    private readonly MockFieldNamesLocalizer _mockNamesLocalizer;
    private readonly PartnerSourceLinkValidator _validator;

    public PartnerSourceLinkValidatorTests()
    {
        _mockValidationLocalizer = new MockFailedToValidateLocalizer();
        _mockNamesLocalizer = new MockFieldNamesLocalizer();
        _validator = new PartnerSourceLinkValidator(_mockNamesLocalizer, _mockValidationLocalizer);
    }

    [Fact]
    public void ShouldReturnSuccessResult_WhenSourceLinkIsValid()
    {
        // Assert
        var link = GetValidSourceLink();

        // Act
        var result = _validator.Validate(link);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ShouldReturnError_WhenLogoIsInvalid()
    {
        // Assert
        var expectedError = _mockValidationLocalizer["Invalid", _mockNamesLocalizer["LogoType"]];
        var link = GetValidSourceLink();
        link.LogoType = (LogoType)100;

        // Act
        var result = _validator.TestValidate(link);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.LogoType)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenTargetUrlIsEmpty()
    {
        // Assert
        var expectedError = _mockValidationLocalizer["CannotBeEmpty", _mockNamesLocalizer["SourceLinkUrl"]];
        var link = GetValidSourceLink();
        link.TargetUrl = string.Empty;

        // Act
        var result = _validator.TestValidate(link);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.TargetUrl)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenUrlLengthIsMoreThan255()
    {
        // Assert
        var expectedError = _mockValidationLocalizer["MaxLength", _mockNamesLocalizer["SourceLinkUrl"], PartnerSourceLinkValidator.PartnerLinkMaxLength];
        var link = GetValidSourceLink();
        link.TargetUrl = new string('x', PartnerSourceLinkValidator.PartnerLinkMaxLength + 1);

        // Act
        var result = _validator.TestValidate(link);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.TargetUrl)
            .WithErrorMessage(expectedError);
    }

    [Theory]
    [InlineData("test")]
    [InlineData("http//test")]
    [InlineData("http://test...com")]
    [InlineData("http:/.com")]
    public void ShouldReturnError_WhenUrlIsInvalid(string invalidUrl)
    {
        // Assert
        var expectedError = _mockValidationLocalizer["ValidUrl_UrlDisplayed", _mockNamesLocalizer["SourceLinkUrl"], invalidUrl];
        var link = GetValidSourceLink();
        link.TargetUrl = invalidUrl;

        // Act
        var result = _validator.TestValidate(link);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.TargetUrl)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenLogoDoesntMatchUrl()
    {
        // Assert
        var expectedError = _mockValidationLocalizer["LogoMustMatchUrl"];
        var link = GetValidSourceLink();
        link.TargetUrl = "https://www.youtube.com/watch?v=dQw4w9WgXcQ&pp=ygUXbmV2ZXIgZ29ubmEgZ2l2ZSB5b3UgdXA%3D";
        link.LogoType = LogoType.Facebook;

        // Act
        var result = _validator.TestValidate(link);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x)
            .WithErrorMessage(expectedError);
    }

    private static CreatePartnerSourceLinkDTO GetValidSourceLink()
    {
        return new CreatePartnerSourceLinkDTO()
        {
            Id = 1,
            LogoType = LogoType.Facebook,
            TargetUrl = "https://www.facebook.com/UAMON/",
        };
    }
}