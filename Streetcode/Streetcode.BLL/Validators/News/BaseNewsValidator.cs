using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.SharedResource;
using Streetcode.BLL.Validators.Common;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Validators.News;

public class BaseNewsValidator : AbstractValidator<CreateUpdateNewsDto>
{
    public const int TitleMaxLength = 100;
    public const int TextMaxLength = 25000;
    public const int UrlMaxLength = 200;
    private const int ImageIdMinValue = 0;
    private readonly IRepositoryWrapper _repositoryWrapper;
    public BaseNewsValidator(
        IStringLocalizer<FailedToValidateSharedResource> localizer,
        IStringLocalizer<FieldNamesSharedResource> fieldLocalizer,
        IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;

        RuleFor(x => x.Title)
                .NotEmpty().WithMessage(x => localizer["CannotBeEmpty", fieldLocalizer["Title"]])
                .MaximumLength(TitleMaxLength).WithMessage(x => localizer["MaxLength", fieldLocalizer["Title"], TitleMaxLength]);

        RuleFor(x => x.Text)
            .NotEmpty().WithMessage(localizer["CannotBeEmpty", fieldLocalizer["Text"]])
            .MaximumLength(TextMaxLength).WithMessage(x => localizer["MaxLength", fieldLocalizer["Text"], TextMaxLength]);

        RuleFor(x => x.ImageId)
                .NotEmpty().WithMessage(localizer["CannotBeEmpty", fieldLocalizer["ImageId"]])
                .GreaterThan(ImageIdMinValue).WithMessage(x => localizer["Invalid", fieldLocalizer["ImageId"]])
                .MustAsync((imageId, token) => ValidationExtentions.HasExistingImage(_repositoryWrapper, imageId, token)).WithMessage(x => localizer["ImageDoesntExist", x.ImageId]);

        RuleFor(x => x.CreationDate)
                .NotEmpty().WithMessage(x => localizer["IsRequired", fieldLocalizer["CreationDate"]]);

        RuleFor(x => x.URL)
            .NotEmpty().WithMessage(x => localizer["CannotBeEmpty", fieldLocalizer["TargetUrl"]])
            .MaximumLength(UrlMaxLength).WithMessage(localizer["MaxLength", fieldLocalizer["TargetUrl"], UrlMaxLength])
            .Matches(@"^[a-z0-9-]*$").WithMessage(x => localizer["InvalidNewsUrl"]);
    }
}