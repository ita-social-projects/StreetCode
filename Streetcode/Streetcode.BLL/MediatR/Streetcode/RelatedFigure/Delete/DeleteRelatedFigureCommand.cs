using FluentResults;
using MediatR;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedFigure.Delete;

public record DeleteRelatedFigureCommand(int ObserverId, int TargetId) : IRequest<Result<Unit>>;
