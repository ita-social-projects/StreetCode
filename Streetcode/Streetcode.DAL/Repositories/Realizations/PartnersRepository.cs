
using Repositories.Interfaces;

namespace Repositories.Realizations
{
    public class PartnersRepository : RepositoryBase , IPartnersRepository 
    {

        public PartnersRepository(StreetcodeDBContext _streetcodeDBContext) 
        {
        }

        public void GetSponsorsAsync() 
        {
            // TODO implement here
        }

    }
}