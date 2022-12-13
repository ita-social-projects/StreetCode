using Streetcode.DAL.Persistence;
using StreetCode.DAL.Repositories.Interfaces.Base;

namespace Repositories.Realizations;

public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{

    public RepositoryBase() 
    {
    }

    protected StreetcodeDbContext StreetcodeDBContext;

    public void Create()
    {
        throw new NotImplementedException();
    }

    public Task CreateAsync()
    {
        throw new NotImplementedException();
    }

    public void GetById()
    {
        throw new NotImplementedException();
    }

    public IQueryable<T> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<T>> GetAllAsync()
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