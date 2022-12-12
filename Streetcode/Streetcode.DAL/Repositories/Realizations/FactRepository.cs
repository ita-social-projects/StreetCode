using Repositories.Interfaces;
using Streetcode.DAL.Persistence;

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