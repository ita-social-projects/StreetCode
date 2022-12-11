
using Repositories.Interfaces;


namespace Repositories.Realizations
{
    public class TransactLinksRepository : RepositoryBase , ITransactLinksRepository 
    {

        public TransactLinksRepository(StreetcodeDBContext _streetcodeDBContext) 
        {
        }

    }
}