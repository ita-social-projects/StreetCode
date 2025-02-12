using FluentResults;
using MediatR;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.CreateFavourite
{
    public record CreateFavouriteStreetcodeCommand(int streetcodeId, string userId)
    : IRequest<Result<Unit>>;
}
