using Streetcode.DAL.Persistence;
using StreetCode.DAL.Repositories.Interfaces.Base;

namespace Repositories.Realizations;

public class RepositoryBase : IRepositoryBase 
{

    public RepositoryBase() 
    {
    }

    protected StreetcodeDbContext StreetcodeDBContext;

    public void Create()
    {
        throw new NotImplementedException();
    }

    public void CreateAsync()
    {
        throw new NotImplementedException();
    }

    public void GetById()
    {
        throw new NotImplementedException();
    }

    public void GetAll()
    {
        throw new NotImplementedException();
    }

    public void GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public void Update()
    {
        throw new NotImplementedException();
    }

    public void Delete()
    {
        throw new NotImplementedException();
    }
}