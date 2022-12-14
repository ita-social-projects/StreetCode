using Streetcode.BLL.Interfaces.Transactions;
using StreetCode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Services.Transactions;

public class TransactLinksService : ITransactLinksService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    public TransactLinksService(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    public void GetTransactLinkAsync()
    { 
        // TODO implement here
    }
}