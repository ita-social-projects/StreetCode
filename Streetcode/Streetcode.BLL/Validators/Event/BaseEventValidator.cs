using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Event.CreateUpdate;
using Streetcode.BLL.SharedResource;

namespace Streetcode.BLL.Validators.Event;

public class BaseEventValidator : AbstractValidator<CreateUpdateEventDto>
{
    public const int TitleMinLength = 2;
    public const int TitleMaxLength = 100;
    public const int DescriptionMaxLength = 500;
    public const int LocationMaxLength = 200;
    public const int OrganizerMaxLength = 200;

    public BaseEventValidator(IStringLocalizer<FailedToValidateSharedResource> localizer, IStringLocalizer<FieldNamesSharedResource> fieldLocalizer)
    {
        RuleFor(e => e.Title)
            .NotEmpty().WithMessage(localizer["CannotBeEmpty", fieldLocalizer["Title"]])
            .Length(TitleMinLength, TitleMaxLength).WithMessage(localizer["LengthBetween", TitleMinLength, TitleMaxLength]);

        RuleFor(e => e.Date)
            .NotEmpty().WithMessage(localizer["CannotBeEmpty", fieldLocalizer["Date"]]);

        RuleFor(e => e.Description)
            .MaximumLength(DescriptionMaxLength).WithMessage(localizer["MaxLength", fieldLocalizer["Description"], DescriptionMaxLength]);

        RuleFor(e => e.Location)
            .MaximumLength(LocationMaxLength).WithMessage(localizer["MaxLength", fieldLocalizer["Location"], LocationMaxLength]);

        RuleFor(e => e.Organizer)
            .MaximumLength(OrganizerMaxLength).WithMessage(localizer["MaxLength", fieldLocalizer["Organizer"], OrganizerMaxLength]);
    }
}
