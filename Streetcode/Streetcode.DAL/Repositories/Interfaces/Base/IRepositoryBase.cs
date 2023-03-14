using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Streetcode.DAL.Repositories.Interfaces.Base;

public interface IRepositoryBase<T>
    where T : class
{
    IQueryable<T> FindAll(Expression<Func<T, bool>>? predicate = default);

    Task<Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<T>> CreateAsync(T entity);

    T Create(T entity);

    Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<T> Update(T entity);

    void Delete(T entity);

    void Attach(T entity);

    Task CreateRangeAsync(IEnumerable<T> items);

    IQueryable<T> Include(params Expression<Func<T, object>>[] includes);

    Task<IEnumerable<T>?> GetAllAsync(
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