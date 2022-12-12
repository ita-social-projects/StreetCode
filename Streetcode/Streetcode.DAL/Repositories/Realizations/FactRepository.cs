
using EFTask.Persistence;
using Repositories.Interfaces;

namespace Repositories.Realizations;

public class FactRepository : RepositoryBase , IFactRepository 
{

    public FactRepository(StreetcodeDbContext _streetcodeDbContext) 
    {
    }

    public string GetFactsByStreetcode() 
    {
        // TODO implement here
        return "GetFactsByStreetcode";
    }

}