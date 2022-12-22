using Streetcode.BLL.Interfaces.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Services.Streetcode;

public class StreetcodeService : IStreetcodeService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    public StreetcodeService(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    public void GetStreetcodeByNameAsync()
    {
        // TODO implement here
    }

    public void GetStreetcodesByTagAsync()
    {
        // TODO implement here
    }

    public void GetByCodeAsync()
    {
        // TODO implement here
    }

    public void GetTagsByStreecodeIdAsync()
    {
        // TODO implement here
    }

    public void GetEventsAsync()
    {
        // TODO implement here
    }

    public void GetPersonsAsync()
    {
        // TODO implement here
    }
}