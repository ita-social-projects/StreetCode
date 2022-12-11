
using Repositories.Interfaces;


namespace Repositories.Realizations;

public class TagRepository : RepositoryBase , ITagRepository 
{

    public TagRepository(StreetcodeDBContext _streetcodeDBContext) 
    {
    }
    public string GetTagByNameAsync()
    {
        return "GetTagByNameAsync";
    }
}