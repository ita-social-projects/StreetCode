using Repositories.Interfaces;
using Streetcode.DAL.Persistence;


namespace Repositories.Realizations;

public class TagRepository : RepositoryBase , ITagRepository 
{

    public TagRepository(StreetcodeDbContext _streetcodeDBContext) 
    {
    }
    public string GetTagByNameAsync()
    {
        return "GetTagByNameAsync";
    }
}