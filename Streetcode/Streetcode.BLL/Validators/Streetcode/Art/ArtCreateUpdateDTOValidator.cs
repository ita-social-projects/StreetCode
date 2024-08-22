using FluentValidation;
using Streetcode.BLL.DTO.Media.Create;

namespace Streetcode.BLL.Validators.Streetcode.Art;

public class ArtCreateUpdateDTOValidator : AbstractValidator<ArtCreateUpdateDTO>
{
    public ArtCreateUpdateDTOValidator()
    {
        RuleFor(dto => dto.Description)
            .NotEmpty()
            .MaximumLength(400);

        RuleFor(dto => dto.Title)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(dto => dto.ModelState)
            .IsInEnum();
    }
}