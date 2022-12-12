
using EFTask.Persistence;
using Repositories.Interfaces;


namespace Repositories.Realizations;

public class TransactLinksRepository : RepositoryBase , ITransactLinksRepository 
{

    public TransactLinksRepository(StreetcodeDbContext _streetcodeDBContext) 
    {
    }
    public string GetTransactLinkAsync()
    {
        return "GetTransactLinkAsync";
        // TODO implement here
    }
}