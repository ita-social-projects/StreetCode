using System.Linq.Expressions;
using Streetcode.BLL.Interfaces.EntityAccessManager;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.Services.EntityAccessManager;

public static class AccessPredicateExtender
{
    public static Expression<Func<TEntity, bool>>? ExtendWithAccessPredicate<TEntity, TAccessEntity>(
        this Expression<Func<TEntity, bool>>? predicate,
        IEntityAccessManager<TAccessEntity> accessManager,
        UserRole? userRole,
        Expression<Func<TEntity, TAccessEntity?>>? property = null)
    {
        var accessPredicate = accessManager.GetAccessPredicate(userRole);
        if (accessPredicate is null)
        {
            return predicate;
        }

        Expression replaceBody = accessPredicate.Parameters[0];
        var parameter = accessPredicate.Parameters[0];
        if (property is not null)
        {
            replaceBody = property.Body;
            parameter = property.Parameters[0];
        }

        var rightVisitor = new ReplaceExpVisitor(replaceBody);
        var right = rightVisitor.Visit(accessPredicate.Body);

        if (predicate is null)
        {
            return Expression.Lambda<Func<TEntity, bool>>(right, parameter);
        }

        var leftVisitor = new ReplaceExpVisitor(parameter);
        var left = leftVisitor.Visit(predicate.Body);
        var body = Expression.AndAlso(left, right);

        return Expression.Lambda<Func<TEntity, bool>>(body, parameter);
    }
}

public class ReplaceExpVisitor : ExpressionVisitor
{
    private readonly Expression _newExpression;

    public ReplaceExpVisitor(Expression newExpression)
    {
        _newExpression = newExpression;
    }

    protected override Expression VisitParameter(ParameterExpression node)
    {
        return _newExpression;
    }
}
