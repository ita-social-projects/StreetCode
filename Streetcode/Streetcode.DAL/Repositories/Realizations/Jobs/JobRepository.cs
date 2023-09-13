using Streetcode.DAL.Entities.Jobs;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Interfaces.Jobs;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.DAL.Repositories.Realizations.Jobs
{
	public class JobRepository : RepositoryBase<Job>, IJobRepository
	{
		public JobRepository(StreetcodeDbContext context)
			: base(context)
		{
		}
	}
}
