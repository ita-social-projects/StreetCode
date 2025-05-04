using FluentValidation;
using FluentValidation.Results;
using Moq;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.DTO.Team.Abstractions;
using Streetcode.BLL.MediatR.Team.Update;
using Streetcode.BLL.Validators.TeamMember;
using Streetcode.BLL.Validators.TeamMember.TeamMemberLInk;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.Validators.Team;

public class UpdateTeamMemberValidatorTests
{
    private readonly MockFieldNamesLocalizer mockNamesLocalizer;
    private readonly MockFailedToValidateLocalizer mockValidationLocalizer;
    private readonly Mock<BaseTeamValidator> mockBaseTeamMemberValidator;
    private readonly Mock<BaseTeamMemberLinkValidator> mockBaseTeamMemberLinkValidator;

    public UpdateTeamMemberValidatorTests()
    {
        this.mockValidationLocalizer = new MockFailedToValidateLocalizer();
        this.mockNamesLocalizer = new MockFieldNamesLocalizer();
        this.mockBaseTeamMemberValidator = new Mock<BaseTeamValidator>(mockValidationLocalizer, mockNamesLocalizer);
        this.mockBaseTeamMemberValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<TeamMemberCreateUpdateDTO>>()))
            .Returns(new ValidationResult());

        mockBaseTeamMemberLinkValidator = new Mock<BaseTeamMemberLinkValidator>(mockValidationLocalizer, mockNamesLocalizer);
        mockBaseTeamMemberLinkValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<TeamMemberLinkCreateUpdateDTO>>()))
            .Returns(new ValidationResult());
    }

    [Fact]
    public void ShouldCallBaseValidator()
    {
        // Arrange
        var query = new UpdateTeamQuery(new UpdateTeamMemberDTO()
        {
            TeamMemberLinks = new List<TeamMemberLinkDTO>()
            {
                new TeamMemberLinkDTO(),
            },
        });

        var updateTeamValidator = new UpdateTeamValidator(this.mockBaseTeamMemberValidator.Object, this.mockBaseTeamMemberLinkValidator.Object, this.mockValidationLocalizer, this.mockNamesLocalizer);

        // Act
        updateTeamValidator.Validate(query);

        // Assert
        mockBaseTeamMemberValidator.Verify(x => x.Validate(It.IsAny<ValidationContext<TeamMemberCreateUpdateDTO>>()), Times.Once);
    }

    [Fact]
    public void ShouldCallLinkValidator()
    {
        // Arrange
        var query = new UpdateTeamQuery(new UpdateTeamMemberDTO()
        {
            TeamMemberLinks = new List<TeamMemberLinkDTO>()
            {
                new TeamMemberLinkDTO(),
            },
        });

        var updateTeamValidator = new UpdateTeamValidator(this.mockBaseTeamMemberValidator.Object, this.mockBaseTeamMemberLinkValidator.Object, this.mockValidationLocalizer, this.mockNamesLocalizer);

        // Act
        updateTeamValidator.Validate(query);

        // Assert
        mockBaseTeamMemberLinkValidator.Verify(x => x.Validate(It.IsAny<ValidationContext<TeamMemberLinkCreateUpdateDTO>>()), Times.Once);
    }
}