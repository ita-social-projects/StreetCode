using System.Text.RegularExpressions;
using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Authentication.Register;
using Streetcode.BLL.SharedResource;

namespace Streetcode.BLL.Validators.Authentication;

public class BaseRegisterValidator : AbstractValidator<RegisterRequestDTO>
{
    public const int MinLengthName = 2;
    public const int MaxLengthName = 50;
    public const int MinLengthSurname = 2;
    public const int MaxLengthSurname = 50;
    public const int MinPasswordLength = 8;
    public const int MaxPasswordLength = 20;

    public BaseRegisterValidator(
        IStringLocalizer<FailedToValidateSharedResource> localizer,
        IStringLocalizer<FieldNamesSharedResource> fieldLocalizer)
    {
        RuleFor(dto => dto.Name)
            .Matches(@"^[a-zA-Zа-яА-ЯґҐєЄіІїЇ'-]+$").WithMessage(localizer["NameFormat"])
            .NotEmpty().WithMessage(localizer["CannotBeEmpty", fieldLocalizer["Name"]])
            .MinimumLength(MinLengthName).WithMessage(localizer["MinLength", fieldLocalizer["Name"], MinLengthName])
            .MaximumLength(MaxLengthName).WithMessage(localizer["MaxLength", fieldLocalizer["Name"], MaxLengthName]);

        RuleFor(dto => dto.Surname)
            .Matches(@"^[a-zA-Zа-яА-ЯґҐєЄіІїЇ'-]+$").WithMessage(localizer["SurnameFormat"])
            .NotEmpty().WithMessage(localizer["CannotBeEmpty", fieldLocalizer["Surname"]])
            .MinimumLength(MinLengthSurname).WithMessage(localizer["MinLength", fieldLocalizer["Surname"], MinLengthSurname])
            .MaximumLength(MaxLengthSurname).WithMessage(localizer["MaxLength", fieldLocalizer["Surname"], MaxLengthSurname]);

        RuleFor(dto => dto.Email)
            .NotEmpty().WithMessage(localizer["CannotBeEmpty", fieldLocalizer["Email"]])
            .Matches(@"^(?!.*\.\.)[a-zA-Z0-9_%+-]+(?:\.[a-zA-Z0-9_%+-]+)*@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")
            .WithMessage(localizer["EmailAddressFormat"]);

        RuleFor(dto => dto.Password)
            .NotEmpty().WithMessage(localizer["CannotBeEmpty", fieldLocalizer["Password"]])
            .MinimumLength(MinPasswordLength).WithMessage(localizer["MinLength", fieldLocalizer["Password"], MinPasswordLength])
            .MaximumLength(MaxPasswordLength).WithMessage(localizer["MaxLength", fieldLocalizer["Password"], MaxPasswordLength])
            .Must(password => !password.Contains(" ")).WithMessage(localizer["PasswordNoWhitespace"])
            .Must(password => Regex.IsMatch(password, "\\d")).WithMessage(localizer["PasswordMustContainDigit"])
            .Must(password => Regex.IsMatch(password, "[^a-zA-Z\\d]")).WithMessage(localizer["PasswordMustContainSpecial"])
            .Must(password => !password.Contains('%')).WithMessage(localizer["PasswordNoPercent"])
            .Must(password => Regex.IsMatch(password, "\\p{Lu}")).WithMessage(localizer["PasswordMustContainUpper"])
            .Must(password => Regex.IsMatch(password, "\\p{Ll}")).WithMessage(localizer["PasswordMustContainLower"]);

        RuleFor(dto => dto.PasswordConfirmation).Equal(dto => dto.Password).WithMessage("PasswordConfirmationNotEqual");
    }
}