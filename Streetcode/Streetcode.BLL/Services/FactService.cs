
using Services.Interfaces;
using StreetCode.DAL.Repositories.Interfaces.Base;

namespace Services.Services;

public class FactService : IFactService 
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    public FactService(IRepositoryWrapper repositoryWrapper) 
    {
        _repositoryWrapper = repositoryWrapper;
    }

    public string GetFactsByStreetcode() 
    {
        return _repositoryWrapper.FactRepository.GetFactsByStreetcode();
    }

}