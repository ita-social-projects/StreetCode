using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.MediatR.Streetcode.Term.Create;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Validators.Streetcode.Text.Term
{
    public class CreateTermValidator : AbstractValidator<CreateTermCommand>
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        public CreateTermValidator(
            IRepositoryWrapper repositoryWrapper,
            BaseTermValidator baseTermValidator,
            IStringLocalizer<FailedToValidateSharedResource> localizer,
            IStringLocalizer<FieldNamesSharedResource> fieldLocalizer)
        {
            _repositoryWrapper = repositoryWrapper;
            RuleFor(c => c.Term).SetValidator(baseTermValidator);
        }
    }
}
