
using Repositories.Interfaces;

namespace Repositories.Realizations
{
    public class SubtitleRepository : RepositoryBase , ISubtitleRepository 
    {

        public SubtitleRepository(StreetcodeDBContext _streetcodeDBContext) 
        {
        }

        public void GetSubtitlesByStreetcode() 
        {
            // TODO implement here
        }

    }
}