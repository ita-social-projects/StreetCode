using FluentResults;
using MediatR;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetFavouriteStatus
{
    public record GetFavouriteStatusQuery(int StreetcodeId)
        : IRequest<Result<bool>>;
}
