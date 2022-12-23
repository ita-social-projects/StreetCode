namespace Streetcode.BLL.Interfaces.Streetcode;

public interface IStreetcodeService
{
    public void GetStreetcodeByNameAsync();
    public void GetStreetcodesByTagAsync();
    public void GetByCodeAsync();
    public void GetTagsByStreecodeIdAsync();
    public void GetEventsAsync();
    public void GetPersonsAsync();
}