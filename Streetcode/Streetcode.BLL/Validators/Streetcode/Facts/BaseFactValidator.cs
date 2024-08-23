using FluentValidation;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;

namespace Streetcode.BLL.Validators.Streetcode.Facts;

public class BaseFactValidator : AbstractValidator<FactUpdateCreateDto>
{
    public const int TitleMaxLength = 68;
    public const int ContentMaxLength = 600;
    public const int ImageDescriptionMaxLength = 200;
    public BaseFactValidator()
    {
        RuleFor(dto => dto.Title)
            .NotEmpty().WithMessage("Title cannot be empty")
            .MaximumLength(TitleMaxLength).WithMessage($"Maximum length of title is {TitleMaxLength}");

        RuleFor(dto => dto.FactContent)
            .NotEmpty().WithMessage("FactContent cannot be empty")
            .MaximumLength(ContentMaxLength).WithMessage($"Maximum length of fact content is {ContentMaxLength}");

        RuleFor(dto => dto.ImageDescription)
            .MaximumLength(ImageDescriptionMaxLength)
            .WithMessage($"Maximum length of image description is {ImageDescriptionMaxLength}");
    }
}