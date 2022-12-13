using Repositories.Interfaces;
using Streetcode.DAL.Persistence;


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