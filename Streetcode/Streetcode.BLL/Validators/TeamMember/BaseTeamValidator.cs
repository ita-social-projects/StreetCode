using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.SharedResource;
using Streetcode.BLL.Validators.TeamMember.Positions;
using Streetcode.BLL.Validators.TeamMember.TeamMemberLInk;

namespace Streetcode.BLL.Validators.TeamMember;

public class BaseTeamValidator : AbstractValidator<TeamMemberCreateUpdateDTO>
{
    public const int NameMaxLength = 41;
    public const int DescriptionMaxLength = 70;
    public BaseTeamValidator(IStringLocalizer<FailedToValidateSharedResource> localizer, IStringLocalizer<FieldNamesSharedResource> fieldLocalizer)
    {
        RuleFor(dto => dto.ImageId)
            .NotNull().WithMessage(localizer["IsRequired", fieldLocalizer["ImageId"]])
            .NotEqual(0).WithMessage(localizer["Invalid", fieldLocalizer["ImageId"]]);

        RuleFor(dto => dto.Name)
            .NotEmpty().WithMessage(localizer["CannotBeEmpty", fieldLocalizer["Name"]])
            .MaximumLength(NameMaxLength).WithMessage(localizer["MaxLength", fieldLocalizer["Name"], NameMaxLength]);

        RuleFor(dto => dto.Description)
            .MaximumLength(DescriptionMaxLength).WithMessage(localizer["MaxLength", fieldLocalizer["Description"], DescriptionMaxLength]);

        RuleFor(dto => dto.IsMain)
            .NotNull().WithMessage(localizer["IsRequired", fieldLocalizer["IsMain"]]);

        RuleForEach(dto => dto.Positions)
            .ChildRules(position =>
            {
                position.RuleFor(p => p.Position)
                    .NotEmpty().WithMessage(localizer["CannotBeEmpty", fieldLocalizer["Position"]])
                    .MaximumLength(BasePositionValidator.MaxPositionLength).WithMessage(localizer["MaxLength", fieldLocalizer["Position"], BasePositionValidator.MaxPositionLength]);
            });
    }
}