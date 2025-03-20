using FluentResults;
using MediatR;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Media.Art.StreetcodeArtSlide.GetAllCountByStreetcodeId
{
    public record GetAllCountByStreetcodeIdQuerry(uint StreetcodeId, UserRole? UserRole)
        : IRequest<Result<int>>;
}
