using Repositories.Interfaces;
using Streetcode.DAL.Persistence;


namespace Repositories.Realizations;

public class TextRepository : RepositoryBase , ITextRepository 
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