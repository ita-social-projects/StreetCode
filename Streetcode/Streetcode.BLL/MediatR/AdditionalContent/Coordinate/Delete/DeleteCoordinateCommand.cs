using FluentResults;
using MediatR;

namespace Streetcode.BLL.MediatR.AdditionalContent.Coordinate.Delete;

public record DeleteCoordinateCommand(int Id) : IRequest<Result<Unit>>;