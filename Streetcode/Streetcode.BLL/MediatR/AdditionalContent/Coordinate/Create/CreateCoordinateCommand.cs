using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent.Coordinates.Types;

namespace Streetcode.BLL.MediatR.AdditionalContent.Coordinate.Create;

public record CreateCoordinateCommand(StreetcodeCoordinateDTO StreetcodeCoordinate) : IRequest<Result<Unit>>;