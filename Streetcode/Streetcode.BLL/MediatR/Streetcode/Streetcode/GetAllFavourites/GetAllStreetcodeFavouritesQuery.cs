using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetAllFavourites
{
    public record GetAllStreetcodeFavouritesQuery(string userId, StreetcodeType? type = null) : IRequest<Result<IEnumerable<StreetcodeFavouriteDto>>>;
}
