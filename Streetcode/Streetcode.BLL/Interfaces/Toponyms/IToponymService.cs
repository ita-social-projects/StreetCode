namespace Streetcode.BLL.Interfaces.Toponyms;

public interface IToponymService
{
    public void GetToponymByNameAsync();
    public void GetStreetcodesByToponymAsync();
}