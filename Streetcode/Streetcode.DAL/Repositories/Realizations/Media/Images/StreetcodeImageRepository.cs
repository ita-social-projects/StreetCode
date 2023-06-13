using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Interfaces.Media.Images;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.DAL.Repositories.Realizations.Media.Images
{
	public class StreetcodeImageRepository : RepositoryBase<StreetcodeImage>, IStreetcodeImageRepository
	{
		public StreetcodeImageRepository(StreetcodeDbContext context)
			: base(context)
		{
		}
	}
}
