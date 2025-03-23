using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Interfaces.Users.Expertise;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.DAL.Repositories.Realizations.Users.Expertise;

public class ExpertiseRepository : RepositoryBase<Entities.Users.Expertise.Expertise>, IExpertiseRepository
{
    public ExpertiseRepository(StreetcodeDbContext context)
        : base(context)
    {
    }
}