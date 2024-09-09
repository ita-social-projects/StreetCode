using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.MediatR.Team.Position.Update;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Validators.TeamMember.Positions;

public class UpdatePositionValidator : AbstractValidator<UpdateTeamPositionCommand>
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    public UpdatePositionValidator(
        IRepositoryWrapper repositoryWrapper,
        BasePositionValidator basePositionValidator,
        IStringLocalizer<FailedToValidateSharedResource> localizer,
        IStringLocalizer<FieldNamesSharedResource> fieldLocalizer)
    {
        _repositoryWrapper = repositoryWrapper;
        RuleFor(c => c.positionDto).SetValidator(basePositionValidator);
        RuleFor(c => c.positionDto)
            .MustAsync(BeUniqueAsync)
            .WithMessage(localizer["MustBeUnique", fieldLocalizer["Position"]]);
    }

    private async Task<bool> BeUniqueAsync(PositionDTO position, CancellationToken token)
    {
        var existingPosition = await _repositoryWrapper.PositionRepository.GetFirstOrDefaultAsync(p => p.Position == position.Position);
        if (existingPosition != null)
        {
            return existingPosition.Id == position.Id;
        }

        return true;
    }
}