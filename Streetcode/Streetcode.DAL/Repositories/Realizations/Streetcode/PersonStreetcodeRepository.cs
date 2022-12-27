using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Realizations.Base;
using Streetcode.DAL.Entities.Streetcode.Types;
using Streetcode.DAL.Repositories.Interfaces.Streetcode.Types;

namespace Streetcode.DAL.Repositories.Realizations.Streetcode;

public class PersonStreetcodeRepository : RepositoryBase<PersonStreetcode>, IPersonStreetcodeRepository
{
    public PersonStreetcodeRepository(StreetcodeDbContext dbContext)
        : base(dbContext)
    {
    }
}
