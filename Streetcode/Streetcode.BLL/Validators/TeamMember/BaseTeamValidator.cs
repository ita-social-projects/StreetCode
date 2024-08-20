using FluentValidation;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Validators.TeamMember.TeamMemberLInk;

namespace Streetcode.BLL.Validators.TeamMember;

public class BaseTeamValidator<T> : AbstractValidator<TeamMemberCreateUpdateDTO<T>>
where T : TeamMemberLinkCreateUpdateDTO
{
    public const int NameMaxLength = 50;
    public const int DescriptionMaxLength = 150;
    public BaseTeamValidator(BaseTeamMemberLinkValidator teamMemberLinkValidator)
    {
        RuleFor(dto => dto.ImageId)
            .NotEqual(0).WithMessage("Invalid ImageId Value");

        RuleFor(dto => dto.Name)
            .NotEmpty().WithMessage("Name cannot be empty")
            .MaximumLength(NameMaxLength).WithMessage($"Maximum length of name is {NameMaxLength}");

        RuleFor(dto => dto.Description)
            .NotEmpty().WithMessage("Description cannot be empty")
            .MaximumLength(DescriptionMaxLength).WithMessage($"Maximum length of description is {DescriptionMaxLength}");

        RuleForEach(dto => dto.TeamMemberLinks).SetValidator(teamMemberLinkValidator);
        RuleFor(dto => dto.TeamMemberLinks)
            .NotNull().WithMessage("TeamMemberLinks is required");
    }
}