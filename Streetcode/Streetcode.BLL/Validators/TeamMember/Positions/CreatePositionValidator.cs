using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.MediatR.Team.Create;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Validators.TeamMember.Positions;

public class CreatePositionValidator : AbstractValidator<CreatePositionQuery>
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    public CreatePositionValidator(
        IRepositoryWrapper repositoryWrapper,
        BasePositionValidator basePositionValidator,
        IStringLocalizer<FailedToValidateSharedResource> localizer,
        IStringLocalizer<FieldNamesSharedResource> fieldLocalizer)
    {
        _repositoryWrapper = repositoryWrapper;
        RuleFor(c => c.position).SetValidator(basePositionValidator);
        RuleFor(c => c.position)
            .MustAsync(BeUniqueAsync)
            .WithMessage(localizer["MustBeUnique", fieldLocalizer["Position"]]);
    }

    private async Task<bool> BeUniqueAsync(PositionCreateDto position, CancellationToken cancellationToken)
    {
        var existingPosition = await _repositoryWrapper.PositionRepository.GetFirstOrDefaultAsync(j => j.Position == position.Position);
        return existingPosition == null;
    }
}