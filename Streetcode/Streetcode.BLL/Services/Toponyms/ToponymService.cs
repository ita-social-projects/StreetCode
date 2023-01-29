using Streetcode.BLL.Interfaces.Toponyms;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.BLL.DTO.Toponyms;
using Streetcode.DAL.Entities.Toponyms;

namespace Streetcode.BLL.Services.Toponyms;

public class ToponymService : IToponymService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    public ToponymService(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    public void GetToponymByNameAsync()
    {
        // TODO implement here
    }

    public void GetStreetcodesByToponymAsync()
    {
        // TODO implement here
    }
}