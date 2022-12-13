using Repositories.Interfaces;
using Streetcode.DAL.Persistence;

namespace Repositories.Realizations;

public class PartnersRepository : RepositoryBase , IPartnersRepository 
{

    public PartnersRepository(StreetcodeDbContext _streetcodeDBContext) 
    {
    }

    public string GetSponsorsAsync() 
    {
        return "GetSponsorsAsync";
        // TODO implement here
    }

}