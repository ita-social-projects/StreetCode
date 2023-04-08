using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetAllShort
{
    public record GetAllStreetcodesShortQuery : IRequest<Result<IEnumerable<StreetcodeShortDTO>>>;
}
