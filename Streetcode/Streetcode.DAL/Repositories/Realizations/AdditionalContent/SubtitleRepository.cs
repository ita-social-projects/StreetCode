using Repositories.Realizations;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Interfaces.AdditionalContent;

namespace Streetcode.DAL.Repositories.Realizations.AdditionalContent;

public class SubtitleRepository : RepositoryBase<Subtitle>, ISubtitleRepository
{
    public SubtitleRepository(StreetcodeDbContext streetcodeDBContext) : base(streetcodeDBContext)
    {
    }
}