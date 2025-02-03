using FluentValidation;
using FluentValidation.Results;
using Moq;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.DTO.Team.Abstractions;
using Streetcode.BLL.MediatR.Partners.Create;
using Streetcode.BLL.MediatR.Team.TeamMembersLinks.Create;
using Streetcode.BLL.Validators.Partners;
using Streetcode.BLL.Validators.TeamMember;
using Streetcode.BLL.Validators.TeamMember.TeamMemberLInk;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.Validators.Team;

public class CreateTeamMemberLinkValidatorTests
{
    private readonly Mock<BaseTeamMemberLinkValidator> mockBaseTeamMemberLinkValidator;

    public CreateTeamMemberLinkValidatorTests()
    {
        MockFailedToValidateLocalizer mockValidationLocalizer = new MockFailedToValidateLocalizer();
        MockFieldNamesLocalizer mockNamesLocalizer = new MockFieldNamesLocalizer();
        this.mockBaseTeamMemberLinkValidator = new Mock<BaseTeamMemberLinkValidator>(mockValidationLocalizer, mockNamesLocalizer);
        this.mockBaseTeamMemberLinkValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<TeamMemberLinkCreateUpdateDto>>()))
            .Returns(new ValidationResult());
    }

    [Fact]
    public void ShouldCallBaseValidator()
    {
        // Arrange
        var query = new CreateTeamLinkQuery(new TeamMemberLinkCreateDto());
        var createValidator = new CreateTeamMemberLinkValidator(this.mockBaseTeamMemberLinkValidator.Object);

        // Act
        createValidator.Validate(query);

        // Assert
        this.mockBaseTeamMemberLinkValidator.Verify(x => x.Validate(It.IsAny<ValidationContext<TeamMemberLinkCreateUpdateDto>>()), Times.Once);
    }
}