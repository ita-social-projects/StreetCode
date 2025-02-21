using System.Linq.Expressions;
using Streetcode.BLL.Interfaces.EntityAccessManager;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.Services.EntityAccessManagerService;

public static class AccessPredicateExtender
{
    public static Expression<Func<TEntity, bool>>? ExtendWithAccessPredicate<TEntity>(this Expression<Func<TEntity, bool>>? predicate, IEntityAccessManager<TEntity> accessManager, UserRole? userRole)
    {
        var accessPredicate = accessManager.GetAccessPredicate(userRole);

        if (predicate is null)
        {
            return accessPredicate;
        }

        if (accessPredicate is null)
        {
            return predicate;
        }

        var parameter = Expression.Parameter(typeof(TEntity), "sc");

        // Replacing the parameters in both bodies to ensure they are using the same parameter
        var left = Expression.Invoke(predicate, parameter);
        var right = Expression.Invoke(accessPredicate, parameter);

        var body = Expression.AndAlso(left, right);

        return Expression.Lambda<Func<TEntity, bool>>(body, parameter);
    }
}