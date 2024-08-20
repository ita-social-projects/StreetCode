using FluentValidation;
using Streetcode.BLL.DTO.Team;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Validators.TeamMember.Positions;

public class BasePositionValidator : AbstractValidator<PositionCreateUpdateDTO>
{
    public const int MaxPositionLength = 50;
    private readonly IRepositoryWrapper _repository;
    public BasePositionValidator(IRepositoryWrapper repositoryWrapper)
    {
        _repository = repositoryWrapper;
        RuleFor(p => p.Position)
            .NotNull().WithMessage("Position is required")
            .MaximumLength(MaxPositionLength).WithMessage("Maximum length of position is 50")
            .MustAsync(BeUniqueAsync).WithMessage("Position must be unique");
    }

    private async Task<bool> BeUniqueAsync(string position, CancellationToken cancellationToken)
    {
        var existingPosition = await _repository.PositionRepository.GetFirstOrDefaultAsync(j => j.Position == position);
        return existingPosition == null;
    }
}