using Streetcode.BLL.Interfaces.AdditionalContent;
using StreetCode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Services.AdditionalContent;

public class TagService : ITagService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    public TagService(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    public void GetTagByNameAsync()
    {
        // TODO implement here
    }
}