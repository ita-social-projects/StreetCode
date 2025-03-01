using FluentResults;
using MediatR;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedFigure.Create;

public record CreateRelatedFigureCommand(int ObserverId, int TargetId)
    : IRequest<Result<Unit>>;
