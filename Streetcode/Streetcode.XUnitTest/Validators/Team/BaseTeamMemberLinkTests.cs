using FluentValidation.TestHelper;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.DTO.Team.Abstractions;
using Streetcode.BLL.Validators.TeamMember.TeamMemberLInk;
using Streetcode.DAL.Enums;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.Validators.Team;

public class BaseTeamMemberLinkTests
{
    private readonly MockFieldNamesLocalizer _mockNamesLocalizer;
    private readonly MockFailedToValidateLocalizer _mockValidationLocalizer;
    private readonly BaseTeamMemberLinkValidator _validator;

    public BaseTeamMemberLinkTests()
    {
        _mockNamesLocalizer = new MockFieldNamesLocalizer();
        _mockValidationLocalizer = new MockFailedToValidateLocalizer();
        _validator = new BaseTeamMemberLinkValidator(_mockValidationLocalizer, _mockNamesLocalizer);
    }

    [Fact]
    public void ShouldReturnSuccess_WhenAllFieldsAreValid()
    {
        // Arrange
        var link = GetValidTeamMemberLink();

        // Act
        var result = _validator.Validate(link);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ShouldReturnError_WhenLogotypeIsInvalid()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["Invalid", _mockNamesLocalizer["LogoType"]];
        var link = GetValidTeamMemberLink();
        link.LogoType = (LogoType)99;

        // Act
        var result = _validator.TestValidate(link);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.LogoType)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenTargetUrlIsEmpty()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["CannotBeEmpty", _mockNamesLocalizer["SourceLinkUrl"]];
        var link = GetValidTeamMemberLink();
        link.TargetUrl = string.Empty;

        // Act
        var result = _validator.TestValidate(link);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.TargetUrl)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenTargetUrlIsMoreThan255Characters()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["MaxLength", _mockNamesLocalizer["SourceLinkUrl"], BaseTeamMemberLinkValidator.MaxTeamMemberLinkLength];
        var link = GetValidTeamMemberLink();
        link.TargetUrl = new string('x', BaseTeamMemberLinkValidator.MaxTeamMemberLinkLength + 1);

        // Act
        var result = _validator.TestValidate(link);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.TargetUrl)
            .WithErrorMessage(expectedError);
    }

    [Theory]
    [InlineData("sdfsdf")]
    [InlineData("s.df./sdf")]
    [InlineData("//site.com")]
    [InlineData("ftp://site.com")]
    public void ShouldReturnError_WhenTargetUrlIsInvalidUrl(string invalidUrl)
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["ValidUrl_UrlDisplayed", _mockNamesLocalizer["SourceLinkUrl"], invalidUrl];
        var link = GetValidTeamMemberLink();
        link.TargetUrl = invalidUrl;

        // Act
        var result = _validator.TestValidate(link);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.TargetUrl)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenLogotypeDoesntMatchUrl()
    {
        // Assert
        var expectedError = _mockValidationLocalizer["LogoMustMatchUrl"];
        var link = GetValidTeamMemberLink();
        link.TargetUrl = "https://www.instagram.com/";
        link.LogoType = LogoType.Facebook;

        // Act
        var result = _validator.TestValidate(link);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x)
            .WithErrorMessage(expectedError);
    }

    private static TeamMemberLinkCreateUpdateDTO GetValidTeamMemberLink()
    {
        return new TeamMemberLinkDTO()
        {
            TeamMemberId = 1,
            Id = 4,
            LogoType = LogoType.Instagram,
            TargetUrl = "https://www.instagram.com/",
        };
    }
}