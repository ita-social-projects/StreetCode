using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Repositories.Realizations;

public abstract class RepositoryBase<T> : IRepositoryBase<T>
    where T : class
{
    private readonly StreetcodeDbContext _streetcodeDbContext;
    public RepositoryBase(StreetcodeDbContext streetcodeDbContext)
    {
        _streetcodeDbContext = streetcodeDbContext;
    }

    public IQueryable<T> FindAll()
    {
        return _streetcodeDbContext.Set<T>().AsNoTracking();
    }

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
    {
        return _streetcodeDbContext.Set<T>().Where(expression).AsNoTracking();
    }

    public void Create(T entity)
    {
        _streetcodeDbContext.Set<T>().Add(entity);
    }

    public async Task CreateAsync(T entity)
    {
        await _streetcodeDbContext.Set<T>().AddAsync(entity);
    }

    public void Update(T entity)
    {
        _streetcodeDbContext.Set<T>().Update(entity);
    }

    public void Delete(T entity)
    {
        _streetcodeDbContext.Set<T>().Remove(entity);
    }

    public void Attach(T entity)
    {
        _streetcodeDbContext.Set<T>().Attach(entity);
    }

    public IQueryable<T> Include(params Expression<Func<T, object>>[] includes)
    {
        IIncludableQueryable<T, object> query = null;

        if (includes.Length > 0)
        {
            query = _streetcodeDbContext.Set<T>().Include(includes[0]);
        }

        for (int queryIndex = 1; queryIndex < includes.Length; ++queryIndex)
        {
            query = query.Include(includes[queryIndex]);
        }

        return query == null ? _streetcodeDbContext.Set<T>() : (IQueryable<T>)query;
    }

    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
    {
        return await GetQuery(predicate, include).ToListAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, T>> selector, Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
    {
        return await GetQuery(predicate, include, selector).ToListAsync();
    }

    public async Task<T> GetFirstAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
    {
        var query = GetQuery(predicate, include);
        return await query.FirstAsync();
    }

    public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
    {
        return await GetQuery(predicate, include).FirstOrDefaultAsync();
    }

    public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, T>> selector, Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
    {
        return await GetQuery(predicate, include, selector).FirstOrDefaultAsync();
    }

    public async Task<T> GetLastAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
    {
        return await GetQuery(predicate, include).LastAsync();
    }

    private IQueryable<T> GetQuery(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, Expression<Func<T, T>> selector = null)
    {
        var query = _streetcodeDbContext.Set<T>().AsNoTracking();
        if (include != null)
        {
            query = include(query);
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (selector != null)
        {
            query = query.Select(selector);
        }

        return query;
    }
}