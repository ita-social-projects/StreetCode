
namespace Services.Interfaces;

public interface IToponymService 
{
    public string GetToponymByNameAsync();
    public void GetStreetcodesByToponymAsync();
}