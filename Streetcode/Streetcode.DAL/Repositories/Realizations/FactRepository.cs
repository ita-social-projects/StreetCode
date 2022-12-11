
using Repositories.Interfaces;

namespace Repositories.Realizations;

public class FactRepository : RepositoryBase , IFactRepository 
{

    public FactRepository(StreetcodeDBContext _streetcodeDBContext) 
    {
    }

    public string GetFactsByStreetcode() 
    {
        // TODO implement here
        return "GetFactsByStreetcode";
    }

}