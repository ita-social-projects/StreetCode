using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent.Coordinates.Types;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.AdditionalContent.Coordinate.GetByStreetcodeId
{
    public record GetCoordinatesByStreetcodeIdQuery(int StreetcodeId, UserRole? UserRole)
        : IRequest<Result<IEnumerable<StreetcodeCoordinateDTO>>>;
}
