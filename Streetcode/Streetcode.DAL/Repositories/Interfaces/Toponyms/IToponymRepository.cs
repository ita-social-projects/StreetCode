namespace Streetcode.DAL.Repositories.Interfaces.Toponyms;

public interface IToponymRepository
{
    public string GetToponymByNameAsync();

    public void GetStreetcodesByToponymAsync();

}