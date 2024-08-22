using FluentValidation;
using Streetcode.BLL.DTO.Media.Images;

namespace Streetcode.BLL.Validators.Streetcode.ImageDetails;

public class ImageDetailsValidator : AbstractValidator<ImageDetailsDto>
{
    public ImageDetailsValidator()
    {
        RuleFor(dto => dto.Title)
            .MaximumLength(100);
        RuleFor(dto => dto.Alt)
            .MaximumLength(200);
    }
}