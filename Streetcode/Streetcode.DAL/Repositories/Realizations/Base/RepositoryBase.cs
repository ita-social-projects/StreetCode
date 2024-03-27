using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using MimeKit;
using Streetcode.DAL.Helpers;
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

    public IQueryable<T> FindAll(
        Expression<Func<T, bool>>? predicate = default,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default)
    {
        return GetQueryable(predicate, include).AsNoTracking();
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

    public Task<List<T>> ExecuteSelectSqlRaw(string query)
    {
        return _dbContext.Set<T>().FromSqlRaw(query).ToListAsync();
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

    public async Task<IEnumerable<T>?> GetAllSortedAsync(
        Expression<Func<T, T>>? selector,
        Expression<Func<T, bool>>? predicate = default,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default,
        Expression<Func<T, object>>? sortingKeySelector = default)
    {
        return await GetQueryable(predicate, include, selector, orderByASC: sortingKeySelector).ToListAsync() ?? new List<T>();
    }

    public async Task<IEnumerable<T>?> GetAllSortedByDescendingAsync(
        Expression<Func<T, T>>? selector = default,
        Expression<Func<T, bool>>? predicate = default,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default,
        Expression<Func<T, object>>? sortingKeySelector = default)
    {
        return await GetQueryable(predicate, include, selector, orderByDESC: sortingKeySelector).ToListAsync() ?? new List<T>();
    }

    public PaginationResponse<T> GetAllPaginated(
        ushort pageNumber = default,
        ushort pageSize = default,
        Expression<Func<T, T>>? selector = default,
        Expression<Func<T, bool>>? predicate = default,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default,
        Expression<Func<T, object>>? ascendingSortKeySelector = default,
        Expression<Func<T, object>>? descendingSortKeySelector = default)
    {
        IQueryable<T> query = GetQueryable(
            predicate,
            include,
            selector,
            orderByASC: ascendingSortKeySelector,
            orderByDESC: descendingSortKeySelector);
        return PaginationResponse<T>.Create(
            query,
            pageNumber,
            pageSize);
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

    public async Task<T?> GetFirstOrDefaultAsync(
        Expression<Func<T, T>> selector,
        Expression<Func<T, bool>>? predicate = default,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default,
        Expression<Func<T, object>>? orderByASC = default,
        Expression<Func<T, object>>? orderByDESC = default,
        int? offset = null)
    {
        return await GetQueryable(predicate, include, selector, orderByASC, orderByDESC, offset: offset).FirstOrDefaultAsync();
    }

    private IQueryable<T> GetQueryable(
        Expression<Func<T, bool>>? predicate = default,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default,
        Expression<Func<T, T>>? selector = default,
        Expression<Func<T, object>>? orderByASC = default,
        Expression<Func<T, object>>? orderByDESC = default,
        int? limit = null,
        int? offset = null)
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

        if (orderByASC is not null)
        {
            query = query.OrderBy(orderByASC);
        }

        if (orderByDESC is not null)
        {
            query = query.OrderByDescending(orderByDESC);
        }

        if (offset.HasValue && offset.Value >= 0)
        {
            query = query.Skip(offset.Value);
        }

        if (limit.HasValue && limit.Value > 0)
        {
            query = query.Take(limit.Value);
        }

        return query.AsNoTracking();
    }
}