using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Jobs;
using Streetcode.BLL.SharedResource;

namespace Streetcode.BLL.Validators.Jobs;

public class BaseJobsValidator : AbstractValidator<CreateUpdateJobDto>
{
    public BaseJobsValidator(IStringLocalizer<FailedToValidateSharedResource> localizer)
    {
        RuleFor(j => j.Title)
            .NotEmpty().WithMessage(localizer["TitleRequired"])
            .MaximumLength(50).WithMessage(localizer["TitleMaxLength"]);
        RuleFor(j => j.Description)
            .MaximumLength(3000).WithMessage(localizer["DescriptionMaxLength"]);
        RuleFor(j => j.Salary)
            .NotEmpty().WithMessage(localizer["SalaryRequired"])
            .MaximumLength(15).WithMessage(localizer["SalaryMaxLength"]);
    }
}