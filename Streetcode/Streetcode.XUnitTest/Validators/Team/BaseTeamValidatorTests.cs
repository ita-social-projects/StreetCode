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
    private readonly MockFieldNamesLocalizer _mockNamesLocalizer;
    private readonly MockFailedToValidateLocalizer _mockValidationLocalizer;
    private readonly BaseTeamValidator _validator;

    public BaseTeamValidatorTests()
    {
        _mockNamesLocalizer = new MockFieldNamesLocalizer();
        _mockValidationLocalizer = new MockFailedToValidateLocalizer();
        _validator = new BaseTeamValidator(_mockValidationLocalizer, _mockNamesLocalizer);
    }

    [Fact]
    public void ShouldReturnSuccessResult_WhenAllFieldsAreValid()
    {
        // Arrange
        var member = GetValidTeamMember();

        // Act
        var result = _validator.Validate(member);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ShouldReturnError_WhenNameIsEmpty()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["CannotBeEmpty", _mockNamesLocalizer["Name"]];
        var member = GetValidTeamMember();
        member.Name = string.Empty;

        // Act
        var result = _validator.TestValidate(member);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenNameLengthIsMoreThan41()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["MaxLength", _mockNamesLocalizer["Name"], BaseTeamValidator.NameMaxLength];
        var member = GetValidTeamMember();
        member.Name = new string('*', BaseTeamValidator.NameMaxLength + 1);

        // Act
        var result = _validator.TestValidate(member);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenImageIdIsNull()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["IsRequired", _mockNamesLocalizer["ImageId"]];
        var member = GetValidTeamMember();
        member.ImageId = null;

        // Act
        var result = _validator.TestValidate(member);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ImageId)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenImageIdIs0()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["Invalid", _mockNamesLocalizer["ImageId"]];
        var member = GetValidTeamMember();
        member.ImageId = 0;

        // Act
        var result = _validator.TestValidate(member);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ImageId)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenDescriptionLengthIsMoreThan70()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["MaxLength", _mockNamesLocalizer["Description"], BaseTeamValidator.DescriptionMaxLength];
        var member = GetValidTeamMember();
        member.Description = new string('*', BaseTeamValidator.DescriptionMaxLength + 1);

        // Act
        var result = _validator.TestValidate(member);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenIsMainIsNull()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["IsRequired", _mockNamesLocalizer["IsMain"]];
        var member = GetValidTeamMember();
        member.IsMain = null;

        // Act
        var result = _validator.TestValidate(member);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.IsMain)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenPositionIsEmpty()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["CannotBeEmpty", _mockNamesLocalizer["Position"]];
        var member = GetValidTeamMember();
        member.Positions![0].Position = string.Empty;

        // Act
        var result = _validator.TestValidate(member);

        // Assert
        result.ShouldHaveValidationErrorFor("Positions[0].Position")
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenPositionLengthIsMoreThan50()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["MaxLength", _mockNamesLocalizer["Position"], BasePositionValidator.MaxPositionLength];
        var member = GetValidTeamMember();
        member.Positions![0].Position = new string('p', BasePositionValidator.MaxPositionLength + 1);

        // Act
        var result = _validator.TestValidate(member);

        // Assert
        result.ShouldHaveValidationErrorFor("Positions[0].Position")
            .WithErrorMessage(expectedError);
    }

    private static TeamMemberCreateUpdateDTO GetValidTeamMember()
    {
        return new TeamMemberCreateDTO()
        {
            Name = "John Doe",
            Description = "Description",
            ImageId = 85,
            IsMain = true,
            Positions = new List<PositionDTO>()
            {
                new ()
                {
                    Id = 5,
                    Position = "CEO"
                },
            },
            TeamMemberLinks = new List<TeamMemberLinkCreateDTO>()
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