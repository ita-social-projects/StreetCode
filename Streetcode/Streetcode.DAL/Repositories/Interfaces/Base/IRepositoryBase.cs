using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using Streetcode.DAL.Persistence;

namespace Streetcode.DAL.Repositories.Interfaces.Base;

public interface IRepositoryBase<T>
    where T : class
{
    IQueryable<T> FindAll(Expression<Func<T, bool>>? predicate = default);

    T Create(T entity);

    Task<T> CreateAsync(T entity);

    Task CreateRangeAsync(IEnumerable<T> items);

    EntityEntry<T> Update(T entity);

    public void UpdateRange(IEnumerable<T> items);

    void Delete(T entity);

    void DeleteRange(IEnumerable<T> items);

    void Attach(T entity);

    void Detach(T entity);

    EntityEntry<T> Entry(T entity);

    public Task ExecuteSqlRaw(string query);

    IQueryable<T> Include(params Expression<Func<T, object>>[] includes);

    Task<IEnumerable<T>> GetAllAsync(
        Expression<Func<T, bool>>? predicate = default,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default);

    Task<IEnumerable<T>?> GetAllAsync(
        Expression<Func<T, T>> selector,
        Expression<Func<T, bool>>? predicate = default,
        Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = default);

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
}
