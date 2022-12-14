using Repositories.Interfaces;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Entities.Media;

namespace Repositories.Realizations;

public class AudioRepository : RepositoryBase<Audio>, IAudioRepository
{

    public AudioRepository(StreetcodeDbContext _streetcodeDBContext) : base(_streetcodeDBContext)
    {
    }
  

}