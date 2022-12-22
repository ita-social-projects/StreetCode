using Repositories.Realizations;
using Streetcode.DAL.Entities.Transactions;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Interfaces.Transactions;

namespace Streetcode.DAL.Repositories.Realizations.Transactions;

public class TransactLinksRepository : RepositoryBase<TransactionLink>, ITransactLinksRepository
{
    public TransactLinksRepository(StreetcodeDbContext streetcodeDbContext)
        : base(streetcodeDbContext)
    {
    }
}