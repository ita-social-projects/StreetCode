using Streetcode.DAL.Repositories.Interfaces.Base;
using FavouriteStreetcodes = Streetcode.DAL.Entities.Streetcode.Favourites.Favourite;

namespace Streetcode.DAL.Repositories.Interfaces.Streetcode.Favourites
{
    public interface IFavouritesRepository : IRepositoryBase<FavouriteStreetcodes>
    {
    }
}
