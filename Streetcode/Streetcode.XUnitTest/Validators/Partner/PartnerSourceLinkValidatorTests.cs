using FluentValidation.TestHelper;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.DTO.Partners.Create;
using Streetcode.BLL.Validators.Partners.SourceLinks;
using Streetcode.DAL.Enums;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.Validators.Partner;

public class PartnerSourceLinkValidatorTests
{
    private readonly MockFailedToValidateLocalizer mockValidationLocalizer;
    private readonly MockFieldNamesLocalizer mockNamesLocalizer;
    private readonly PartnerSourceLinkValidator validator;

    public PartnerSourceLinkValidatorTests()
    {
        this.mockValidationLocalizer = new MockFailedToValidateLocalizer();
        this.mockNamesLocalizer = new MockFieldNamesLocalizer();
        this.validator = new PartnerSourceLinkValidator(this.mockNamesLocalizer, this.mockValidationLocalizer);
    }

    [Fact]
    public void ShouldReturnSuccessResult_WhenSourceLinkIsValid()
    {
        // Assert
        var link = this.GetValidSourceLink();

        // Act
        var result = this.validator.Validate(link);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ShouldReturnError_WhenLogoIsInvalid()
    {
        // Assert
        var expectedError = this.mockValidationLocalizer["Invalid", this.mockNamesLocalizer["LogoType"]];
        var link = this.GetValidSourceLink();
        link.LogoType = (LogoType)100;

        // Act
        var result = this.validator.TestValidate(link);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.LogoType)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenTargetUrlIsEmpty()
    {
        // Assert
        var expectedError = this.mockValidationLocalizer["CannotBeEmpty", this.mockNamesLocalizer["SourceLinkUrl"]];
        var link = this.GetValidSourceLink();
        link.TargetUrl = string.Empty;

        // Act
        var result = this.validator.TestValidate(link);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.TargetUrl)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenUrlLengthIsMoreThan255()
    {
        // Assert
        var expectedError = this.mockValidationLocalizer["MaxLength", this.mockNamesLocalizer["SourceLinkUrl"], PartnerSourceLinkValidator.PartnerLinkMaxLength];
        var link = this.GetValidSourceLink();
        link.TargetUrl = new string('x', PartnerSourceLinkValidator.PartnerLinkMaxLength + 1);

        // Act
        var result = this.validator.TestValidate(link);

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
        var expectedError = this.mockValidationLocalizer["ValidUrl_UrlDisplayed", this.mockNamesLocalizer["SourceLinkUrl"], invalidUrl];
        var link = this.GetValidSourceLink();
        link.TargetUrl = invalidUrl;

        // Act
        var result = this.validator.TestValidate(link);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.TargetUrl)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenLogoDoesntMatchUrl()
    {
        // Assert
        var expectedError = this.mockValidationLocalizer["LogoMustMatchUrl"];
        var link = this.GetValidSourceLink();
        link.TargetUrl = "https://www.youtube.com/watch?v=dQw4w9WgXcQ&pp=ygUXbmV2ZXIgZ29ubmEgZ2l2ZSB5b3UgdXA%3D";
        link.LogoType = LogoType.Facebook;

        // Act
        var result = this.validator.TestValidate(link);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x)
            .WithErrorMessage(expectedError);
    }

    private CreatePartnerSourceLinkDto GetValidSourceLink()
    {
        return new CreatePartnerSourceLinkDto()
        {
            Id = 1,
            LogoType = LogoType.Facebook,
            TargetUrl = "https://www.facebook.com/UAMON/",
        };
    }
}