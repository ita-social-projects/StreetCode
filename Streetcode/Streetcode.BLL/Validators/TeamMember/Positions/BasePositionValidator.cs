using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Validators.TeamMember.Positions;

public class BasePositionValidator : AbstractValidator<PositionCreateUpdateDTO>
{
    public const int MaxPositionLength = 50;
    private readonly IRepositoryWrapper _repository;
    public BasePositionValidator(
        IStringLocalizer<FailedToValidateSharedResource> localizer,
        IStringLocalizer<FieldNamesSharedResource> fieldLocalizer,
        IRepositoryWrapper repositoryWrapper)
    {
        _repository = repositoryWrapper;
        RuleFor(p => p.Position)
            .NotEmpty().WithMessage(localizer["CannotBeEmpty", fieldLocalizer["Position"]])
            .MaximumLength(MaxPositionLength).WithMessage(localizer["MaxLength", fieldLocalizer["Position"], MaxPositionLength])
            .MustAsync(BeUniqueAsync).WithMessage(localizer["MustBeUnique", fieldLocalizer["Position"]]);
    }

    private async Task<bool> BeUniqueAsync(string position, CancellationToken cancellationToken)
    {
        var existingPosition = await _repository.PositionRepository.GetFirstOrDefaultAsync(j => j.Position == position);
        return existingPosition == null;
    }
}