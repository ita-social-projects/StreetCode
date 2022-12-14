using Streetcode.BLL.Interfaces.Partners;
using StreetCode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Services.Partners;

public class PartnersService : IPartnersService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    public PartnersService(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    public void GetSponsorsAsync()
    {
        // TODO implement here
    }

}