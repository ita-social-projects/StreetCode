using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.MediatR.Newss.Create;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Validators.News;

public class CreateNewsValidator : AbstractValidator<CreateNewsCommand>
{
    private readonly IRepositoryWrapper _repositoryWrapper;

    public CreateNewsValidator(BaseNewsValidator baseNewsValidator)
    {
        RuleFor(n => n.newNews).SetValidator(baseNewsValidator);
    }
}