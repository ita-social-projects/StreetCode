using FluentResults;
using MediatR;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.CreateFavourite
{
    public record CreateFavouriteStreetcodeCommand(int StreetcodeId)
    : IRequest<Result<Unit>>;
}
