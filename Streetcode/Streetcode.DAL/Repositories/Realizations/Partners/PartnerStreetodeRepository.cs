using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Interfaces.Partners;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.DAL.Repositories.Realizations.Partners
{
    public class PartnerStreetodeRepository : RepositoryBase<StreetcodePartner>, IPartnerStreetcodeRepository
    {
        public PartnerStreetodeRepository(StreetcodeDbContext context)
            : base(context)
        {
        }
    }
}
