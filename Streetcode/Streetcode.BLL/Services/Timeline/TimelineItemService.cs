using Streetcode.BLL.Interfaces.Timeline;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Services.Timeline;

public class TimelineItemService : ITimelineItemService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    public TimelineItemService(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    public void GetTimelineItemsAsync()
    {
        // TODO implement here
    }
}