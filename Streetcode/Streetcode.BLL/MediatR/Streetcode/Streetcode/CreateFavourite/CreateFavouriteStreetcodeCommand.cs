using FluentResults;
using MediatR;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.UpdateToFavourites
{
    public record CreateFavouriteStreetcodeCommand(int streetcodeId, string userId)
    : IRequest<Result<Unit>>;
}
