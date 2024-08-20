using FluentValidation;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Validators.TeamMember.Positions;
using Streetcode.BLL.Validators.TeamMember.TeamMemberLInk;

namespace Streetcode.BLL.Validators.TeamMember;

public class BaseTeamValidator : AbstractValidator<TeamMemberCreateUpdateDTO>
{
    public const int NameMaxLength = 50;
    public const int DescriptionMaxLength = 150;
    public BaseTeamValidator()
    {
        RuleFor(dto => dto.ImageId)
            .NotNull().WithMessage("ImageId is required")
            .NotEqual(0).WithMessage("Invalid ImageId Value");

        RuleFor(dto => dto.Name)
            .NotNull().WithMessage("Name is required")
            .NotEmpty().WithMessage("Name cannot be empty")
            .MaximumLength(NameMaxLength).WithMessage($"Maximum length of name is {NameMaxLength}");

        RuleFor(dto => dto.Description)
            .MaximumLength(DescriptionMaxLength).WithMessage($"Maximum length of description is {DescriptionMaxLength}");

        RuleFor(dto => dto.IsMain)
            .NotNull().WithMessage("IsMain is required");

        RuleForEach(dto => dto.Positions)
            .ChildRules(position =>
            {
                position.RuleFor(p => p.Position)
                    .NotNull().WithMessage("Position is required")
                    .MaximumLength(BasePositionValidator.MaxPositionLength).WithMessage("Maximum length of position is 50");
            });
    }
}