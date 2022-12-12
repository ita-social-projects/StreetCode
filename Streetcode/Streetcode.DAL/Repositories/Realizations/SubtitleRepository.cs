using Repositories.Interfaces;
using Streetcode.DAL.Persistence;

namespace Repositories.Realizations;

public class SubtitleRepository : RepositoryBase , ISubtitleRepository 
{

    public SubtitleRepository(StreetcodeDbContext _streetcodeDBContext) 
    {
    }

    public string GetSubtitlesByStreetcode() 
    {
        return "GetSubtitlesByStreetcode";
        // TODO implement here
    }

}