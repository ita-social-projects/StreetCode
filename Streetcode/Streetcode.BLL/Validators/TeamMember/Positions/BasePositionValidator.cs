using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Validators.TeamMember.Positions;

public class BasePositionValidator : AbstractValidator<PositionCreateUpdateDTO>
{
    public const int MaxPositionLength = 50;
    public BasePositionValidator(
        IStringLocalizer<FailedToValidateSharedResource> localizer,
        IStringLocalizer<FieldNamesSharedResource> fieldLocalizer)
    {
        RuleFor(p => p.Position)
            .NotEmpty().WithMessage(localizer["CannotBeEmpty", fieldLocalizer["Position"]])
            .MaximumLength(MaxPositionLength)
            .WithMessage(localizer["MaxLength", fieldLocalizer["Position"], MaxPositionLength]);
    }
}