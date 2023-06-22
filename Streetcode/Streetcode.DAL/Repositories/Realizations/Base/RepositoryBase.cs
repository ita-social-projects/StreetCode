using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using MimeKit;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.DAL.Repositories.Realizations.Base;

public abstract class RepositoryBase<T> : IRepositoryBase<T>
    where T : class
{
    private readonly StreetcodeDbContext _dbContext;

    protected RepositoryBase(StreetcodeDbContext context)
    {
        _dbContext = context;
    }

    public IQueryable<T> FindAll(Expression<Func<T, bool>>? predicate = default)
    {
        return GetQueryable(predicate).AsNoTracking();
    }

    public T Create(T entity)
    {
        return _dbContext.Set<T>().Add(entity).Entity;
    }

    public async Task<T> CreateAsync(T entity)
    {
        var tmp = await _dbContext.Set<T>().AddAsync(entity);
        return tmp.Entity;
    }

    public Task CreateRangeAsync(IEnumerable<T> items)
    {
        return _dbContext.Set<T>().AddRangeAsync(items);
    }

    public EntityEntry<T> Update(T entity)
    {
        return _dbContext.Set<T>().Update(entity);
    }

    public void UpdateRange(IEnumerable<T> items)
    {
        _dbContext.Set<T>().UpdateRange(items);
    }

    public void Delete(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
    }

    public void DeleteRange(IEnumerable<T> items)
    {
        _dbContext.Set<T>().RemoveRange(items);
    }

    public void Attach(T entity)
    {
        _dbContext.Set<T>().Attach(entity);
    }

    public EntityEntry<T> Entry(T entity)
    {
        return _dbContext.Entry(entity);
    }

    public void Detach(T entity)
    {
        _dbContext.Entry(entity).State = EntityState.Detached;
    }

    public Task ExecuteSqlRaw(string query)
    {
        return _dbContext.Database.ExecuteSqlRawAsync(query);
    }

    public IQueryable<T> Include(params Expression<Func<T, object>>[] includes)
    {
        IIncludableQueryable<T, object>? query = default;

        if (includes.Any())
        {
            query = _dbContext.Set<T>().Include(includes[0]);
        }

        for (int queryIndex = 1; queryIndex < includes.Length; ++queryIndex)
        {
            query = query!.Include(includes[queryIndex]);
        }

        return (query is null) ? _dbContext.Set<T>() : query.AsQueryable();
    }

    public async Task<IEnumerable<T>> GetAllAsync(
        Expression<Func<T, bool>>? predicate = default,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default)
    {
        return await GetQueryable(predicate, include).ToListAsync();
    }

    public async Task<IEnumerable<T>?> GetAllAsync(
        Expression<Func<T, T>> selector,
        Expression<Func<T, bool>>? predicate = default,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default)
    {
        return await GetQueryable(predicate, include, selector).ToListAsync() ?? new List<T>();
    }

    public async Task<T?> GetSingleOrDefaultAsync(
        Expression<Func<T, bool>>? predicate = default,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default)
    {
        return await GetQueryable(predicate, include).SingleOrDefaultAsync();
    }

    public async Task<T?> GetFirstOrDefaultAsync(
        Expression<Func<T, bool>>? predicate = default,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default)
    {
        return await GetQueryable(predicate, include).FirstOrDefaultAsync();
    }

    public async Task<T?> GetFirstOrDefaultAsync(
        Expression<Func<T, T>> selector,
        Expression<Func<T, bool>>? predicate = default,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default)
    {
        return await GetQueryable(predicate, include, selector).FirstOrDefaultAsync();
    }

    private IQueryable<T> GetQueryable(
        Expression<Func<T, bool>>? predicate = default,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default,
        Expression<Func<T, T>>? selector = default)
    {
        var query = _dbContext.Set<T>().AsNoTracking();

        if (include is not null)
        {
            query = include(query);
        }

        if (predicate is not null)
        {
            query = query.Where(predicate);
        }

        if (selector is not null)
        {
            query = query.Select(selector);
        }

        return query.AsNoTracking();
    }
}
