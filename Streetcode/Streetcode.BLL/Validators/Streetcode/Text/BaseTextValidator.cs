using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Streetcode.TextContent.Text;
using Streetcode.BLL.SharedResource;

namespace Streetcode.BLL.Validators.Streetcode.Text;

public class BaseTextValidator : AbstractValidator<BaseTextDTO>
{
    public const int TitleMaxLength = 50;
    public const int TextMaxLength = 20000;
    public const int AdditionalTextMaxLength = 200;
    public BaseTextValidator(IStringLocalizer<FailedToValidateSharedResource> localizer, IStringLocalizer<FieldNamesSharedResource> fieldLocalizer)
    {
        RuleFor(dto => dto.Title)
            .MaximumLength(TitleMaxLength).WithMessage(localizer["MaxLength", fieldLocalizer["TextTitle"], TitleMaxLength]);
        RuleFor(dto => dto.TextContent)
            .MaximumLength(TextMaxLength).WithMessage(localizer["MaxLength", fieldLocalizer["TextContent"], TextMaxLength]);
        RuleFor(dto => dto.AdditionalText)
            .MaximumLength(AdditionalTextMaxLength).WithMessage(localizer["MaxLength", fieldLocalizer["AdditionalText"], AdditionalTextMaxLength]);

        RuleFor(dto => dto)
            .Must(HaveTextWithTitle).WithMessage(localizer["CannotBeEmptyWithCondition", fieldLocalizer["TextTitle"], fieldLocalizer["TextContent"]])
            .Must(HaveAdditionalTextWithMainText)
            .WithMessage(localizer["CannotBeEmptyWithCondition", fieldLocalizer["TextContent"], fieldLocalizer["AdditionalText"]]);
    }

    private bool HaveTextWithTitle(BaseTextDTO dto)
    {
        if (!string.IsNullOrWhiteSpace(dto.TextContent))
        {
            return !string.IsNullOrWhiteSpace(dto.Title);
        }

        return true;
    }

    private bool HaveAdditionalTextWithMainText(BaseTextDTO dto)
    {
        if(!string.IsNullOrWhiteSpace(dto.AdditionalText))
        {
            return !string.IsNullOrWhiteSpace(dto.TextContent);
        }

        return true;
    }
}