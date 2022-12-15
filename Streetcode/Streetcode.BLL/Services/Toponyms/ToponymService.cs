
using Repositories.Realizations;
using Streetcode.BLL.Interfaces.Toponyms;
using StreetCode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Services.Toponyms;

public class ToponymService : IToponymService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    public ToponymService(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

#pragma warning disable IDE0051 // Remove unused private members
    private readonly RepositoryWrapper RepositoryWrapper;
#pragma warning restore IDE0051 // Remove unused private members

    public string GetToponymByNameAsync()
    {
        // TODO implement here
        return _repositoryWrapper.ToponymRepository.GetToponymByNameAsync();
    }

    public void GetStreetcodesByToponymAsync()
    {
        // TODO implement here
    }

}