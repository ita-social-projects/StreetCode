using Streetcode.BLL.Interfaces.Streetcode.TextContent;
using StreetCode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Services.Streetcode.TextContent;

public class TermService : ITermService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    public TermService(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    public void GetTermAsync()
    {
        // TODO implement here
    }
}