using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Partners.Update;
using Streetcode.BLL.MediatR.Partners.Update;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Validators.Partners;

public class UpdatePartnerValidator : AbstractValidator<UpdatePartnerQuery>
{
    private readonly IRepositoryWrapper _repositoryWrapper;

    public UpdatePartnerValidator(
        BasePartnersValidator basePartnersValidator,
        IRepositoryWrapper repositoryWrapper,
        IStringLocalizer<AlreadyExistSharedResource> alreadyExistLocalizer,
        IStringLocalizer<FieldNamesSharedResource> fieldLocalizer)
    {
        _repositoryWrapper = repositoryWrapper;
        RuleFor(c => c.Partner).SetValidator(basePartnersValidator);

        RuleFor(c => c.Partner).MustAsync(BeUniqueImageId).WithMessage(x => alreadyExistLocalizer["PartnerWithFieldAlreadyExist", fieldLocalizer["LogoId"], x.Partner.LogoId]);
    }

    private async Task<bool> BeUniqueImageId(UpdatePartnerDTO dto, CancellationToken cancellationToken)
    {
        var existingNewsByImageId = await _repositoryWrapper.PartnersRepository.GetSingleOrDefaultAsync(n => n.LogoId == dto.LogoId);

        if (existingNewsByImageId is not null)
        {
            return existingNewsByImageId.Id == dto.Id;
        }

        return existingNewsByImageId is null;
    }
}