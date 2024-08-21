using FluentValidation;
using Streetcode.BLL.MediatR.Jobs.Create;

namespace Streetcode.BLL.Validators.Jobs;

public class CreateJobsValidator : AbstractValidator<CreateJobCommand>
{
    public CreateJobsValidator(BaseJobsValidator baseJobsValidator)
    {
        RuleFor(c => c.job).SetValidator(baseJobsValidator);
    }
}