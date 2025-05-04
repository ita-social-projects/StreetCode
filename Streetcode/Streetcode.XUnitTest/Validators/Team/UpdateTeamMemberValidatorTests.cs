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
    private readonly Mock<BaseTeamValidator> _mockBaseTeamMemberValidator;
    private readonly Mock<BaseTeamMemberLinkValidator> _mockBaseTeamMemberLinkValidator;

    public UpdateTeamMemberValidatorTests()
    {
        MockFailedToValidateLocalizer mockValidationLocalizer = new MockFailedToValidateLocalizer();
        MockFieldNamesLocalizer mockNamesLocalizer = new MockFieldNamesLocalizer();
        _mockBaseTeamMemberValidator = new Mock<BaseTeamValidator>(mockValidationLocalizer, mockNamesLocalizer);
        _mockBaseTeamMemberValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<TeamMemberCreateUpdateDTO>>()))
            .Returns(new ValidationResult());

        _mockBaseTeamMemberLinkValidator = new Mock<BaseTeamMemberLinkValidator>(mockValidationLocalizer, mockNamesLocalizer);
        _mockBaseTeamMemberLinkValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<TeamMemberLinkCreateUpdateDTO>>()))
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
        var updateTeamValidator = new UpdateTeamValidator(_mockBaseTeamMemberValidator.Object, _mockBaseTeamMemberLinkValidator.Object);

        // Act
        updateTeamValidator.Validate(query);

        // Assert
        _mockBaseTeamMemberValidator.Verify(x => x.Validate(It.IsAny<ValidationContext<TeamMemberCreateUpdateDTO>>()), Times.Once);
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
        var updateTeamValidator = new UpdateTeamValidator(_mockBaseTeamMemberValidator.Object, _mockBaseTeamMemberLinkValidator.Object);

        // Act
        updateTeamValidator.Validate(query);

        // Assert
        _mockBaseTeamMemberLinkValidator.Verify(x => x.Validate(It.IsAny<ValidationContext<TeamMemberLinkCreateUpdateDTO>>()), Times.Once);
    }
}