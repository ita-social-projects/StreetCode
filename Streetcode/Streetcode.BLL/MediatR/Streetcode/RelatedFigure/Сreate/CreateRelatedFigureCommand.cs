using FluentResults;
using MediatR;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedFigure.Сreate;

public record CreateRelatedFigureCommand(int ObserverId, int TargetId)
    : IRequest<Result<Unit>>;
