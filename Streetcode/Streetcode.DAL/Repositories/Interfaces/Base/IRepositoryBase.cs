using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using Streetcode.DAL.Helpers;
using Streetcode.DAL.Persistence;

namespace Streetcode.DAL.Repositories.Interfaces.Base;

public interface IRepositoryBase<T>
    where T : class
{
    IQueryable<T> FindAll(
        Expression<Func<T, bool>>? predicate = default,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default);

    T Create(T entity);

    Task<T> CreateAsync(T entity);

    Task CreateRangeAsync(IEnumerable<T> items);

    EntityEntry<T> Update(T entity);

    public void UpdateRange(IEnumerable<T> items);

    void Delete(T entity);

    void DeleteRange(IEnumerable<T> items);

    void Attach(T entity);

    EntityEntry<T> Entry(T entity);

    public Task ExecuteSqlRaw(string query);

    Task<IEnumerable<T>> GetAllAsync(
        Expression<Func<T, bool>>? predicate = default,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default);

    public PaginationResponse<T> GetAllPaginated(
        ushort pageNumber = default,
        ushort pageSize = default,
        Expression<Func<T, T>>? selector = default,
        Expression<Func<T, bool>>? predicate = default,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default,
        Expression<Func<T, object>>? ascendingSortKeySelector = default,
        Expression<Func<T, object>>? descendingSortKeySelector = default);

    Task<T?> GetSingleOrDefaultAsync(
        Expression<Func<T, bool>>? predicate = default,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default);

    Task<T?> GetFirstOrDefaultAsync(
        Expression<Func<T, bool>>? predicate = default,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default);

    Task<T?> GetFirstOrDefaultAsync(
        Expression<Func<T, T>> selector,
        Expression<Func<T, bool>>? predicate = default,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default);
    Task<T?> GetFirstOrDefaultAsync(
        Expression<Func<T, T>> selector,
        Expression<Func<T, bool>>? predicate = default,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default,
        Expression<Func<T, object>>? orderByASC = default,
        Expression<Func<T, object>>? orderByDESC = default,
        int? offset = null);
}