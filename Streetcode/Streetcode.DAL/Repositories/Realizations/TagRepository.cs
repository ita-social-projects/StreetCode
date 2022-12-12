
using EFTask.Persistence;
using Repositories.Interfaces;


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