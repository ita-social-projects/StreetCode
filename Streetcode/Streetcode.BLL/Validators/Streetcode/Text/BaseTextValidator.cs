using FluentValidation;
using Streetcode.BLL.DTO.Streetcode.TextContent.Text;

namespace Streetcode.BLL.Validators.Streetcode.Text;

public class BaseTextValidator : AbstractValidator<BaseTextDTO>
{
    public const int TitleMaxLength = 50;
    public const int TextMaxLength = 20000;
    public const int AdditionalTextMaxLength = 200;
    public BaseTextValidator()
    {
        RuleFor(dto => dto.Title)
            .MaximumLength(TitleMaxLength).WithMessage($"Maximum length of title is {TitleMaxLength}");
        RuleFor(dto => dto.TextContent)
            .MaximumLength(TextMaxLength).WithMessage($"Maximum length of text is {TextMaxLength}");
        RuleFor(dto => dto.AdditionalText)
            .MaximumLength(AdditionalTextMaxLength).WithMessage($"Maximum length of additional text is {AdditionalTextMaxLength}");

        RuleFor(dto => dto)
            .Must(HaveTextWithTitle).WithMessage("The 'title' key for the text is empty or missing")
            .Must(HaveAdditionalTextWithMainText)
            .WithMessage("The 'text' key for the additional text is empty or missing");
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