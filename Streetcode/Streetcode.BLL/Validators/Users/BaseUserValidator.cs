using System.Text.RegularExpressions;
using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Users;
using Streetcode.BLL.SharedResource;
using Streetcode.BLL.Validators.Common;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Validators.Users;

public class BaseUserValidator : AbstractValidator<UpdateUserDTO>
{
    public const int MaxLengthAboutYourself = 500;
    public const int MinLengthName = 2;
    public const int MaxLengthName = 128;
    public const int MinLengthSurname = 2;
    public const int MaxLengthSurname = 128;
    public const int MinLengthUserName = 2;
    public const int MaxLengthUserName = 128;
    public const int MaxExpertiesesCount = 3;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public BaseUserValidator(
        IStringLocalizer<FailedToValidateSharedResource> localizer,
        IStringLocalizer<FieldNamesSharedResource> fieldLocalizer,
        IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
        RuleFor(dto => dto.Expertises)
            .Must(e => e.Count <= MaxExpertiesesCount).WithMessage(localizer["MustContainAtMostThreeExpertises", fieldLocalizer["Expertises"]]);

        RuleFor(dto => dto.AboutYourself)
            .MaximumLength(MaxLengthAboutYourself).WithMessage(localizer["MaxLength", fieldLocalizer["AboutYourself"], MaxLengthAboutYourself]);

        RuleFor(dto => dto.UserName)
            .Matches(@"^[a-zA-Z0-9'\-_]+$").WithMessage(localizer["UserNameFormat"])
            .NotEmpty().WithMessage(localizer["CannotBeEmpty", fieldLocalizer["UserName"]])
            .MinimumLength(MinLengthName).WithMessage(localizer["MinLength", fieldLocalizer["UserName"], MinLengthUserName])
            .MaximumLength(MaxLengthName).WithMessage(localizer["MaxLength", fieldLocalizer["UserName"], MaxLengthUserName]);

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

        RuleFor(x => x.AvatarId)
            .MustAsync((imageId, token) => ValidationExtentions.HasExistingImage(_repositoryWrapper, imageId, token))
            .When(x => x.AvatarId is not null)
            .WithMessage((dto, imgId) => localizer["ImageDoesntExist", imgId]);
    }
}