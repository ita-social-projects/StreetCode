using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent.Coordinates.Types;

namespace Streetcode.BLL.MediatR.AdditionalContent.Coordinate.GetByStreetcodeId
{
    public record GetCoordinatesByStreetcodeIdQuery(int streetcodeId) : IRequest<Result<IEnumerable<StreetcodeCoordinateDTO>>>;
}
