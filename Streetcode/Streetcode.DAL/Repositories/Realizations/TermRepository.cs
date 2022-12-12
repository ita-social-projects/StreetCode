
using EFTask.Persistence;
using Repositories.Interfaces;


namespace Repositories.Realizations;

public class TermRepository : RepositoryBase , ITermRepository 
{

    public TermRepository(StreetcodeDbContext _streetcodeDBContext) 
    {

    }
    public string GetTermAsync()
    {
        return "GetTermAsync";
    }
}