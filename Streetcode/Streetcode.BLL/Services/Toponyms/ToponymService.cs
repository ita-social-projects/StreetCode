
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