
using Repositories.Interfaces;


namespace Repositories.Realizations;

public class TermRepository : RepositoryBase , ITermRepository 
{

    public TermRepository(StreetcodeDBContext _streetcodeDBContext) 
    {

    }
    public string GetTermAsync()
    {
        return "GetTermAsync";
    }
}