using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Users;
using Streetcode.BLL.SharedResource;

namespace Streetcode.BLL.Validators.Users;

public class BaseUserValidator : AbstractValidator<UpdateUserDTO>
{
    public const int MaxLengthAboutYourself = 500;
    public const int MinLengthName = 2;
    public const int MaxLengthName = 128;
    public const int MinLengthSurname = 2;
    public const int MaxLengthSurname = 128;
    public const int MaxExpertiesesCount = 3;

    public BaseUserValidator(IStringLocalizer<FailedToValidateSharedResource> localizer, IStringLocalizer<FieldNamesSharedResource> fieldLocalizer)
    {
        RuleFor(dto => dto.Expertises)
            .Must(e => e.Count <= MaxExpertiesesCount).WithMessage(localizer["MustContainAtMostThreeExpertises", fieldLocalizer["Expertises"]]);

        RuleFor(dto => dto.AboutYourself)
            .MaximumLength(MaxLengthAboutYourself).WithMessage(localizer["MaxLength", fieldLocalizer["AboutYourself"], MaxLengthAboutYourself]);

        RuleFor(dto => dto.Name)
            .Matches(@"^[a-zA-Zа-яА-ЯґҐєЄіІїЇ'-]+$").WithMessage(localizer["NameFormat", fieldLocalizer["Name"], MinLengthSurname])
            .NotEmpty().WithMessage(localizer["CannotBeEmpty", fieldLocalizer["Name"]])
            .MinimumLength(MinLengthName).WithMessage(localizer["MinLength", fieldLocalizer["Name"], MinLengthName])
            .MaximumLength(MaxLengthName).WithMessage(localizer["MaxLength", fieldLocalizer["Name"], MaxLengthName]);

        RuleFor(dto => dto.Surname)
            .Matches(@"^[a-zA-Zа-яА-ЯґҐєЄіІїЇ'-]+$").WithMessage(localizer["SurnameFormat", fieldLocalizer["Surname"], MinLengthSurname])
            .NotEmpty().WithMessage(localizer["CannotBeEmpty", fieldLocalizer["Surname"]])
            .MinimumLength(MinLengthSurname).WithMessage(localizer["MinLength", fieldLocalizer["Surname"], MinLengthSurname])
            .MaximumLength(MaxLengthSurname).WithMessage(localizer["MaxLength", fieldLocalizer["Surname"], MaxLengthSurname]);
    }
}