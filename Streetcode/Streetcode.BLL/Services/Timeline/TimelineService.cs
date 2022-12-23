using Streetcode.BLL.Interfaces.Timeline;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Services.Timeline;

public class TimelineService : ITimelineService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    public TimelineService(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    public void GetTimelineItemsAsync()
    {
        // TODO implement here
    }
}