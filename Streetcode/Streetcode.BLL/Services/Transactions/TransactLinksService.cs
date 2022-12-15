using Repositories.Realizations;
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

#pragma warning disable IDE0051 // Remove unused private members
    private readonly RepositoryWrapper RepositoryWrapper;
#pragma warning restore IDE0051 // Remove unused private members

    public string GetTransactLinkAsync()
    {
        return _repositoryWrapper.TransactLinksRepository.GetTransactLinkAsync();
        // TODO implement here
    }
}