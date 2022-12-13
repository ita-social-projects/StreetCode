
namespace Repositories.Interfaces;

public interface IToponymRepository 
{
    public string GetToponymByNameAsync();

    public void GetStreetcodesByToponymAsync();
   
}