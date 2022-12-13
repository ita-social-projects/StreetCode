using Repositories.Interfaces;
using Repositories.Realizations;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Interfaces.Streetcode;
using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.DAL.Repositories.Realizations.Streetcode;

public class StreetcodeRepository : RepositoryBase<StreetcodeContent>, IStreetcodeRepository
{
    public StreetcodeRepository(StreetcodeDbContext _streetcodeDBContext)
    {
    }

    public string GetStreetcodeByNameAsync()
    {
        return "GetStreetcodeByNameAsync";
        // TODO implement here
    }
    public void GetTagsByStreetcodeIdAsync()
    {
        // TODO implement here
    }
    public void GetByCodeAsync()
    {
        // TODO implement here
    }
    public void GetEventsAsync()
    {
        // TODO implement here
    }
    public void GetPersonsAsync()
    {
        // TODO implement here
    }
    public void GetRelatedStreetcodeAsync()
    {
        // TODO implement here
    }
    public void GetStreetcodeByTagAsync()
    {
        // TODO implement here
    }
}
