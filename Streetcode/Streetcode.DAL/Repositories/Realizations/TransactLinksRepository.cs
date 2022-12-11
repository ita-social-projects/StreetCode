
using Repositories.Interfaces;


namespace Repositories.Realizations;

public class TransactLinksRepository : RepositoryBase , ITransactLinksRepository 
{

    public TransactLinksRepository(StreetcodeDBContext _streetcodeDBContext) 
    {
    }
    public string GetTransactLinkAsync()
    {
        return "GetTransactLinkAsync";
        // TODO implement here
    }
}