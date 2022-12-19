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
<<<<<<< HEAD

    public void GetToponymByNameAsync()
=======
    
    public string GetToponymByNameAsync()
>>>>>>> remotes/origin/nuke-integration
    {
        // TODO implement here
    }

    public void GetStreetcodesByToponymAsync()
    {
        // TODO implement here
    }
}