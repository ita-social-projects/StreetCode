<<<<<<< HEAD
=======
using Repositories.Realizations;
>>>>>>> remotes/origin/nuke-integration
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

<<<<<<< HEAD
    public void GetTransactLinkAsync()
=======
    public string GetTransactLinkAsync()
>>>>>>> remotes/origin/nuke-integration
    {
        // TODO implement here
    }
}