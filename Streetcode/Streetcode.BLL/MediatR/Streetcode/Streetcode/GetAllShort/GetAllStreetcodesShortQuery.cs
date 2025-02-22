using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetAllShort
{
    public record GetAllStreetcodesShortQuery(UserRole? userRole) : IRequest<Result<IEnumerable<StreetcodeShortDTO>>>;
}
