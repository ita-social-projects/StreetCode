using Streetcode.DAL.Entities.Transactions;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Interfaces.Transactions;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.DAL.Repositories.Realizations.Transactions;

public class TransactLinksRepository : RepositoryBase<TransactionLink>, ITransactLinksRepository
{
    public TransactLinksRepository(StreetcodeDbContext dbContext)
        : base(dbContext)
    {
    }
}