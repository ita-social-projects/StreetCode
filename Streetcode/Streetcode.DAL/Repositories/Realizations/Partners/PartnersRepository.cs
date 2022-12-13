using Streetcode.DAL.Entities.Partners;
using Repositories.Interfaces;
using Repositories.Realizations;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Interfaces.Partners;

namespace Streetcode.DAL.Repositories.Realizations.Partners;

public class PartnersRepository : RepositoryBase<Partner>, IPartnersRepository
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