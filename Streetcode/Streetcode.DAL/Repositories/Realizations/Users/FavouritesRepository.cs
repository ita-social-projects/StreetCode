using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Interfaces.Users.Favourites;
using Streetcode.DAL.Repositories.Realizations.Base;
using FavouriteStreetcodes = Streetcode.DAL.Entities.Users.Favourites.Favourites;

namespace Streetcode.DAL.Repositories.Realizations.Users
{
    public class FavouritesRepository : RepositoryBase<FavouriteStreetcodes>, IFavouritesRepository
    {
        public FavouritesRepository(StreetcodeDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}
