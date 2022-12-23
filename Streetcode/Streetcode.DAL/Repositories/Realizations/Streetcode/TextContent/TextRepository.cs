using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Persistence;
using Repositories.Realizations;
using Streetcode.DAL.Repositories.Interfaces.Streetcode.TextContent;

namespace Streetcode.DAL.Repositories.Realizations.Streetcode.TextContent;

public class TextRepository : RepositoryBase<Text>, ITextRepository
{
    public TextRepository(StreetcodeDbContext streetcodeDbContext)
        : base(streetcodeDbContext)
    {
    }
}