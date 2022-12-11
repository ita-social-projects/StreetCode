
using Repositories.Interfaces;


namespace Repositories.Realizations
{
    public class TextRepository : RepositoryBase , ITextRepository 
    {

        public TextRepository(StreetcodeDBContext _streetcodeDBContext) 
        {
        }

        public void GetNext() 
        {
            // TODO implement here
        }

    }
}