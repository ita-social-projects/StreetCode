using FluentValidation.TestHelper;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.DTO.Team.Abstractions;
using Streetcode.BLL.Validators.TeamMember;
using Streetcode.BLL.Validators.TeamMember.Positions;
using Streetcode.DAL.Enums;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.Validators.Team;

public class BaseTeamValidatorTests
{
    private readonly MockFieldNamesLocalizer mockNamesLocalizer;
    private readonly MockFailedToValidateLocalizer mockValidationLocalizer;
    private readonly BaseTeamValidator validator;

    public BaseTeamValidatorTests()
    {
        this.mockNamesLocalizer = new MockFieldNamesLocalizer();
        this.mockValidationLocalizer = new MockFailedToValidateLocalizer();
        this.validator = new BaseTeamValidator(this.mockValidationLocalizer, this.mockNamesLocalizer);
    }

    [Fact]
    public void ShouldReturnSuccessResult_WhenAllFieldsAreValid()
    {
        // Arrange
        var member = this.GetValidTeamMember();

        // Act
        var result = this.validator.Validate(member);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ShouldReturnError_WhenNameIsEmpty()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["CannotBeEmpty", this.mockNamesLocalizer["Name"]];
        var member = this.GetValidTeamMember();
        member.Name = string.Empty;

        // Act
        var result = this.validator.TestValidate(member);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenNameLengthIsMoreThan41()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["MaxLength", this.mockNamesLocalizer["Name"], BaseTeamValidator.NameMaxLength];
        var member = this.GetValidTeamMember();
        member.Name = new string('*', BaseTeamValidator.NameMaxLength + 1);

        // Act
        var result = this.validator.TestValidate(member);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenImageIdIsNull()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["IsRequired", this.mockNamesLocalizer["ImageId"]];
        var member = this.GetValidTeamMember();
        member.ImageId = null;

        // Act
        var result = this.validator.TestValidate(member);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ImageId)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenImageIdIs0()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["Invalid", this.mockNamesLocalizer["ImageId"]];
        var member = this.GetValidTeamMember();
        member.ImageId = 0;

        // Act
        var result = this.validator.TestValidate(member);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ImageId)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenDescriptionLengthIsMoreThan70()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["MaxLength", this.mockNamesLocalizer["Description"], BaseTeamValidator.DescriptionMaxLength];
        var member = this.GetValidTeamMember();
        member.Description = new string('*', BaseTeamValidator.DescriptionMaxLength + 1);

        // Act
        var result = this.validator.TestValidate(member);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenIsMainIsNull()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["IsRequired", this.mockNamesLocalizer["IsMain"]];
        var member = this.GetValidTeamMember();
        member.IsMain = null;

        // Act
        var result = this.validator.TestValidate(member);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.IsMain)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenPositionIsEmpty()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["CannotBeEmpty", this.mockNamesLocalizer["Position"]];
        var member = this.GetValidTeamMember();
        member.Positions![0].Position = string.Empty;

        // Act
        var result = this.validator.TestValidate(member);

        // Assert
        result.ShouldHaveValidationErrorFor("Positions[0].Position")
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenPositionLengthIsMoreThan50()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["MaxLength", this.mockNamesLocalizer["Position"], BasePositionValidator.MaxPositionLength];
        var member = this.GetValidTeamMember();
        member.Positions![0].Position = new string('p', BasePositionValidator.MaxPositionLength + 1);

        // Act
        var result = this.validator.TestValidate(member);

        // Assert
        result.ShouldHaveValidationErrorFor("Positions[0].Position")
            .WithErrorMessage(expectedError);
    }

    private TeamMemberCreateUpdateDto GetValidTeamMember()
    {
        return new TeamMemberCreateDto()
        {
            Name = "John Doe",
            Description = "Description",
            ImageId = 85,
            IsMain = true,
            Positions = new List<PositionDto>()
            {
                new ()
                {
                    Id = 5,
                    Position = "CEO"
                },
            },
            TeamMemberLinks = new List<TeamMemberLinkCreateDto>()
            {
                new ()
                {
                    TeamMemberId = 5,
                    LogoType = LogoType.Instagram,
                    TargetUrl = "https://www.instagram.com/"
                },
            },
        };
    }
}