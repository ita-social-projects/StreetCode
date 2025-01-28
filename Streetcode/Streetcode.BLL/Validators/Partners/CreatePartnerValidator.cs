using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.MediatR.Partners.Create;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Validators.Partners;

public class CreatePartnerValidator : AbstractValidator<CreatePartnerQuery>
{
    private readonly IRepositoryWrapper _repositoryWrapper;

    public CreatePartnerValidator(
        BasePartnersValidator basePartnersValidator,
        IRepositoryWrapper repositoryWrapper,
        IStringLocalizer<FieldNamesSharedResource> fieldLocalizer,
        IStringLocalizer<FailedToValidateSharedResource> localizer)
    {
        _repositoryWrapper = repositoryWrapper;
        RuleFor(c => c.newPartner).SetValidator(basePartnersValidator);

        RuleFor(c => c.newPartner.LogoId).MustAsync(BeUniqueImageId).WithMessage(x => localizer["MustBeUnique", fieldLocalizer["LogoId"]]);
    }

    private async Task<bool> BeUniqueImageId(int imageId, CancellationToken cancellationToken)
    {
        var existingNewsByImageId = await _repositoryWrapper.PartnersRepository.GetSingleOrDefaultAsync(n => n.LogoId == imageId);

        return existingNewsByImageId is null;
    }
}