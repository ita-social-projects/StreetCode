using Repositories.Interfaces;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Persistence;
using Repositories.Realizations;
using Streetcode.DAL.Repositories.Interfaces.Streetcode.TextContent;

namespace Streetcode.DAL.Repositories.Realizations.Streetcode.TextContent;

public class TextRepository : RepositoryBase<Text>, ITextRepository
{

    public TextRepository(StreetcodeDbContext _streetcodeDBContext)
    {
    }

    public void GetNext()
    {
        // TODO implement here
    }
    public string GetTextAsync()
    {
        return "GetTextAsync";
    }
}