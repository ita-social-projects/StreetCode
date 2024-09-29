using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Interfaces.Team;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.DAL.Repositories.Realizations.Team
{
    public class TeamPositionRepository : RepositoryBase<TeamMemberPositions>, ITeamPositionRepository
    {
        public TeamPositionRepository(StreetcodeDbContext context)
            : base(context)
        {
        }
    }
}
