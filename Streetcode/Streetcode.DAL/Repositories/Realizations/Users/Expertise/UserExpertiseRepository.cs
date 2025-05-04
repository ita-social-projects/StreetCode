using Streetcode.DAL.Entities.Users.Expertise;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Interfaces.Users.Expertise;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.DAL.Repositories.Realizations.Users.Expertise;

public class UserExpertiseRepository : RepositoryBase<UserExpertise>, IUserExpertiseRepository
{
    public UserExpertiseRepository(StreetcodeDbContext context)
        : base(context)
    {
    }
}