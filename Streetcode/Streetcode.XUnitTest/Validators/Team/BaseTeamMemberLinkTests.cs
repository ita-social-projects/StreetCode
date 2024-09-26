using FluentValidation.TestHelper;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Validators.TeamMember;
using Streetcode.BLL.Validators.TeamMember.TeamMemberLInk;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Enums;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.Validators.Team;

public class BaseTeamMemberLinkTests
{
    private readonly MockFieldNamesLocalizer mockNamesLocalizer;
    private readonly MockFailedToValidateLocalizer mockValidationLocalizer;
    private readonly BaseTeamMemberLinkValidator validator;

    public BaseTeamMemberLinkTests()
    {
        this.mockNamesLocalizer = new MockFieldNamesLocalizer();
        this.mockValidationLocalizer = new MockFailedToValidateLocalizer();
        this.validator = new BaseTeamMemberLinkValidator(this.mockValidationLocalizer, this.mockNamesLocalizer);
    }

    [Fact]
    public void ShouldReturnSuccess_WhenAllFieldsAreValid()
    {
        // Arrange
        var link = this.GetValidTeamMemberLink();

        // Act
        var result = this.validator.Validate(link);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ShouldReturnError_WhenLogotypeIsInvalid()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["Invalid", this.mockNamesLocalizer["LogoType"]];
        var link = this.GetValidTeamMemberLink();
        link.LogoType = (LogoType)99;

        // Act
        var result = this.validator.TestValidate(link);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.LogoType)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenTargetUrlIsEmpty()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["CannotBeEmpty", this.mockNamesLocalizer["SourceLinkUrl"]];
        var link = this.GetValidTeamMemberLink();
        link.TargetUrl = string.Empty;

        // Act
        var result = this.validator.TestValidate(link);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.TargetUrl)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenTargetUrlIsMoreThan255Characters()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["MaxLength", this.mockNamesLocalizer["SourceLinkUrl"], BaseTeamMemberLinkValidator.MaxTeamMemberLinkLength];
        var link = this.GetValidTeamMemberLink();
        link.TargetUrl = new string('x', BaseTeamMemberLinkValidator.MaxTeamMemberLinkLength + 1);

        // Act
        var result = this.validator.TestValidate(link);

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
        var expectedError = this.mockValidationLocalizer["ValidUrl_UrlDisplayed", this.mockNamesLocalizer["SourceLinkUrl"], invalidUrl];
        var link = this.GetValidTeamMemberLink();
        link.TargetUrl = invalidUrl;

        // Act
        var result = this.validator.TestValidate(link);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.TargetUrl)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenLogotypeDoesntMatchUrl()
    {
        // Assert
        var expectedError = this.mockValidationLocalizer["LogoMustMatchUrl"];
        var link = this.GetValidTeamMemberLink();
        link.TargetUrl = "https://www.instagram.com/";
        link.LogoType = LogoType.Facebook;

        // Act
        var result = this.validator.TestValidate(link);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x)
            .WithErrorMessage(expectedError);
    }

    private TeamMemberLinkCreateUpdateDTO GetValidTeamMemberLink()
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