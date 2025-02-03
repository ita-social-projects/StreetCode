using Streetcode.DAL.Repositories.Interfaces.Base;
using FavouriteStreetcodes = Streetcode.DAL.Entities.Users.Favourites.Favourites;

namespace Streetcode.DAL.Repositories.Interfaces.Streetcode.Favourites
{
    public interface IFavouritesRepository : IRepositoryBase<FavouriteStreetcodes>
    {
    }
}
