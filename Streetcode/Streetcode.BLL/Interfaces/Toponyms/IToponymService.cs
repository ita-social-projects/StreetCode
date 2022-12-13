namespace Streetcode.BLL.Interfaces.Toponyms;

public interface IToponymService
{
    public string GetToponymByNameAsync();
    public void GetStreetcodesByToponymAsync();
}