using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Interfaces.AdditionalContent;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.DAL.Repositories.Realizations.AdditionalContent
{
    internal class StreetcodeTagIndexRepository : RepositoryBase<StreetcodeTagIndex>, IStreetcodeTagIndexRepository
    {
        public StreetcodeTagIndexRepository(StreetcodeDbContext context)
            : base(context)
        {
        }
    }
}
