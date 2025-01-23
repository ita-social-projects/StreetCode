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
        IStringLocalizer<FailedToValidateSharedResource> localizer,
        IStringLocalizer<FieldNamesSharedResource> fieldLocalizer,
        IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;

        RuleFor(c => c.newPartner).SetValidator(basePartnersValidator);

        RuleFor(c => c.newPartner.Title)
            .MustAsync(BeUniqueTitle).WithMessage(x => localizer["MustBeUnique", fieldLocalizer["Title"]]);

        RuleFor(c => c.newPartner.LogoId).MustAsync(BeUniqueImageId).WithMessage(x => localizer["MustBeUnique", fieldLocalizer["LogoId"]]);
    }

    private async Task<bool> BeUniqueTitle(string title, CancellationToken token)
    {
        var existingPartnerByTitle = await _repositoryWrapper.PartnersRepository.GetFirstOrDefaultAsync(n => n.Title == title);

        return existingPartnerByTitle is null;
    }

    private async Task<bool> BeUniqueImageId(int imageId, CancellationToken cancellationToken)
    {
        var existingNewsByImageId = await _repositoryWrapper.PartnersRepository.GetSingleOrDefaultAsync(n => n.LogoId == imageId);

        return existingNewsByImageId is null;
    }
}