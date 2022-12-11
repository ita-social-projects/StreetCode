
using Repositories.Interfaces;

namespace Repositories.Realizations;

public class PartnersRepository : RepositoryBase , IPartnersRepository 
{

    public PartnersRepository(StreetcodeDBContext _streetcodeDBContext) 
    {
    }

    public string GetSponsorsAsync() 
    {
        return "GetSponsorsAsync";
        // TODO implement here
    }

}