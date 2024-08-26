using FluentResults;
using MediatR;

namespace Streetcode.BLL.MediatR.Media.Art.StreetcodeArtSlide.GetAllCountByStreetcodeId
{
    public record GetAllCountByStreetcodeIdQuerry(uint StreetcodeId): IRequest<Result<int>>;
}
