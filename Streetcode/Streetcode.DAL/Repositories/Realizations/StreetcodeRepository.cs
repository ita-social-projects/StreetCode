
using EFTask.Persistence;
using Repositories.Interfaces;

namespace Repositories.Realizations;

public class StreetcodeRepository : RepositoryBase, IStreetcodeRepository
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
