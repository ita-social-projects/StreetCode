using Repositories.Interfaces;
using Repositories.Realizations;
using Streetcode.DAL.Entities.Toponyms;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Interfaces.Toponyms;

namespace Streetcode.DAL.Repositories.Realizations.Toponyms;

public class ToponymRepository : RepositoryBase<Toponym>, IToponymRepository
{

    public ToponymRepository(StreetcodeDbContext _streetcodeDBContext)
    {
    }

    public string GetToponymByNameAsync()
    {
        return "GetToponymByNameAsync";
        // TODO implement here
    }

    public void GetStreetcodesByToponymAsync()
    {
        // TODO implement here
    }

}