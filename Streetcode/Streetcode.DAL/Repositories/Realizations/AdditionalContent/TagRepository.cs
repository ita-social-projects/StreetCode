using Repositories.Realizations;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Interfaces.AdditionalContent;

namespace Streetcode.DAL.Repositories.Realizations.AdditionalContent;

public class TagRepository : RepositoryBase<Tag>, ITagRepository
{
    public TagRepository(StreetcodeDbContext streetcodeDbContext)
        : base(streetcodeDbContext)
    {
    }
}