using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Jobs;
using Streetcode.BLL.SharedResource;

namespace Streetcode.BLL.Validators.Jobs;

public class BaseJobsValidator : AbstractValidator<CreateUpdateJobDto>
{
    private const int TitleMaxLength = 50;
    private const int DescriptionMaxLength = 3000;
    private const int SalaryMaxLength = 15;
    public BaseJobsValidator(IStringLocalizer<FailedToValidateSharedResource> localizer, IStringLocalizer<FieldNamesSharedResource> fieldLocalizer)
    {
        RuleFor(j => j.Title)
            .NotEmpty().WithMessage(localizer["IsRequired", fieldLocalizer["Title"]])
            .MaximumLength(50).WithMessage(localizer["MaxLength", fieldLocalizer["Title"], TitleMaxLength]);
        RuleFor(j => j.Description)
            .MaximumLength(3000).WithMessage(localizer["MaxLength", fieldLocalizer["Description"], DescriptionMaxLength]);
        RuleFor(j => j.Salary)
            .NotEmpty().WithMessage(localizer["IsRequired", fieldLocalizer["Salary"]])
            .MaximumLength(SalaryMaxLength).WithMessage(localizer["MaxLength", fieldLocalizer["Salary"], SalaryMaxLength]);
    }
}