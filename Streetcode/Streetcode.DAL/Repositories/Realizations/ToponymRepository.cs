
using Repositories.Interfaces;

namespace Repositories.Realizations;

public class ToponymRepository : RepositoryBase , IToponymRepository 
{

    public ToponymRepository(StreetcodeDBContext _streetcodeDBContext) 
    {
    }

    public string GetToponymByNameAsync() 
    {
        return "GetToponymByNameAsync";
        // TODO implement here
    }

    public void GetStreetcodesByToponymAsync() 
    {
        // TODO implement here
    }

}