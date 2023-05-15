using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Interfaces.AdditionalContent;
using Streetcode.DAL.Repositories.Interfaces.Team;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.DAL.Repositories.Realizations.Team
{
    public class TeamRepository : RepositoryBase<TeamMember>, ITeamRepository
    {
        public TeamRepository(StreetcodeDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}
