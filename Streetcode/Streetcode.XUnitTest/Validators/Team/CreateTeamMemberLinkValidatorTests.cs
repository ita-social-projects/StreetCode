using FluentValidation;
using FluentValidation.Results;
using Moq;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.DTO.Team.Abstractions;
using Streetcode.BLL.MediatR.Team.TeamMembersLinks.Create;
using Streetcode.BLL.Validators.TeamMember.TeamMemberLInk;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.Validators.Team;

public class CreateTeamMemberLinkValidatorTests
{
    private readonly Mock<BaseTeamMemberLinkValidator> _mockBaseTeamMemberLinkValidator;

    public CreateTeamMemberLinkValidatorTests()
    {
        MockFailedToValidateLocalizer mockValidationLocalizer = new MockFailedToValidateLocalizer();
        MockFieldNamesLocalizer mockNamesLocalizer = new MockFieldNamesLocalizer();
        _mockBaseTeamMemberLinkValidator = new Mock<BaseTeamMemberLinkValidator>(mockValidationLocalizer, mockNamesLocalizer);
        _mockBaseTeamMemberLinkValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<TeamMemberLinkCreateUpdateDTO>>()))
            .Returns(new ValidationResult());
    }

    [Fact]
    public void ShouldCallBaseValidator()
    {
        // Arrange
        var query = new CreateTeamLinkQuery(new TeamMemberLinkCreateDTO());
        var createValidator = new CreateTeamMemberLinkValidator(_mockBaseTeamMemberLinkValidator.Object);

        // Act
        createValidator.Validate(query);

        // Assert
        _mockBaseTeamMemberLinkValidator.Verify(x => x.Validate(It.IsAny<ValidationContext<TeamMemberLinkCreateUpdateDTO>>()), Times.Once);
    }
}