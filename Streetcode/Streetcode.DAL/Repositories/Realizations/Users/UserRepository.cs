using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Interfaces.Users;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.DAL.Repositories.Realizations.Users
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(StreetcodeDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}
