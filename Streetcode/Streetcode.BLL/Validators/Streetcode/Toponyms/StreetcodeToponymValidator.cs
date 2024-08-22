using FluentValidation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Streetcode.BLL.DTO.Toponyms;

namespace Streetcode.BLL.Validators.Streetcode.Toponyms;

public class StreetcodeToponymValidator : AbstractValidator<StreetcodeToponymCreateUpdateDTO>
{
    public StreetcodeToponymValidator()
    {
        RuleFor(dto => dto.StreetName)
            .NotNull().WithMessage("{PropertyName} is required.")
            .NotEmpty().WithMessage("{PropertyName} should not be empty")
            .MaximumLength(150).WithMessage("The maximum length of {PropertyName} is 150 characters.");
        RuleFor(dto => dto.ModelState)
            .IsInEnum();
    }
}