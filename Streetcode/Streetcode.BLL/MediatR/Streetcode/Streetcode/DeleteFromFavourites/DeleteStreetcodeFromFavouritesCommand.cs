using FluentResults;
using MediatR;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.DeleteFromFavourites
{
    public record DeleteStreetcodeFromFavouritesCommand(int StreetcodeId)
    : IRequest<Result<Unit>>;
}
