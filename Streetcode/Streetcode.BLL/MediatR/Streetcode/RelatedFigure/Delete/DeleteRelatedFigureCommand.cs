using FluentResults;
using MediatR;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedFigure.Delete;

public record DeleteRelatedFigureCommand(int id1, int id2) : IRequest<Result<Unit>>;