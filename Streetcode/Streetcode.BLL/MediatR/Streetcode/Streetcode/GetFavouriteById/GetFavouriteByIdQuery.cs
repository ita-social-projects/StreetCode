using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetFavouriteById
{
    public record GetFavouriteByIdQuery(int StreetcodeId)
        : IRequest<Result<StreetcodeFavouriteDto>>;
}
