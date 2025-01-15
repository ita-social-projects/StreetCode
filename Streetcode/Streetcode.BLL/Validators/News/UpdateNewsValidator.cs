using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.MediatR.Newss.Update;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Validators.News;

public class UpdateNewsValidator : AbstractValidator<UpdateNewsCommand>
{
    private readonly IRepositoryWrapper _repositoryWrapper;

    public UpdateNewsValidator(BaseNewsValidator baseNewsValidator)
    {
        RuleFor(n => n.news).SetValidator(baseNewsValidator);
    }
}
