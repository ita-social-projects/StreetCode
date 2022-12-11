
namespace Services.Interfaces;

public interface IStreetcodeService 
{
    public string GetStreetcodeByNameAsync();
    public void GetStreetcodesByTagAsync();
    public void GetByCodeAsync();
    public void GetTagsByStreecodeIdAsync();
    public void GetEventsAsync();
    public void GetPersonsAsync();
   
}