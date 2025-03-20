using System.Linq.Expressions;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.Interfaces.EntityAccessManager;

public interface IEntityAccessManager<TEntity>
{
    Expression<Func<TEntity, bool>>? GetAccessPredicate(UserRole? userRole);
}