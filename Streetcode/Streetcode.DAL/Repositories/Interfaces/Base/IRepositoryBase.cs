using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace Streetcode.DAL.Repositories.Interfaces.Base;

public interface IRepositoryBase<T>
{
    IQueryable<T> FindAll();
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
    void Create(T entity);
    Task CreateAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    void Attach(T entity);
    IQueryable<T> Include(params Expression<Func<T, object>>[] includes);
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, T>> selector, Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
    Task<T> GetFirstAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
    Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
    Task<T> GetFirstOrDefaultAsync(Expression<Func<T, T>> selector, Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
    Task<T> GetLastAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
}