using Streetcode.BLL.Interfaces.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Services.Streetcode.TextContent;

public class TextService : ITextService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    public TextService(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    public void GetNextAsync()
    {
        // TODO implement here
    }

    public void GetTextAsync()
    {
        // TODO implement here
    }
}