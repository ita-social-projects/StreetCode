using Repositories.Interfaces;
using Streetcode.DAL.Entities.Media;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.DAL.Repositories.Realizations.Media;

public class AudioRepository : RepositoryBase<Audio>, IAudioRepository
{
    public AudioRepository(StreetcodeDbContext dbContext)
        : base(dbContext)
    {
    }
}