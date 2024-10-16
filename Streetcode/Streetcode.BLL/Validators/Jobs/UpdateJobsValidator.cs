using FluentValidation;
using Streetcode.BLL.MediatR.Jobs.Update;

namespace Streetcode.BLL.Validators.Jobs;

public class UpdateJobsValidator : AbstractValidator<UpdateJobCommand>
{
    public UpdateJobsValidator(BaseJobsValidator baseJobsValidator)
    {
        RuleFor(c => c.job).SetValidator(baseJobsValidator);
    }
}