using Repositories.Realizations;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Interfaces.Streetcode;
using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.DAL.Repositories.Realizations.Streetcode;

public class StreetcodeTimelineItemRepository : RepositoryBase<StreetcodeTimelineItem>, IStreetcodeTimelineItemRepository
{
    public StreetcodeTimelineItemRepository(StreetcodeDbContext streetcodeDBContext) : base(streetcodeDBContext)
    {
    }
}
