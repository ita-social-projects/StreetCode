using System.Linq.Expressions;
using Streetcode.BLL.Interfaces.EntityAccessManager;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.Services.EntityAccessManager;

public static class AccessPredicateExtender
{
    public static Expression<Func<TBaseEntity, bool>>? ExtendWithAccessPredicate<TBaseEntity, TAccessEntity>(
        this Expression<Func<TBaseEntity, bool>>? basePredicate,
        IEntityAccessManager<TAccessEntity> accessManager,
        UserRole? userRole,
        Expression<Func<TBaseEntity, TAccessEntity?>>? baseToAccessEntitySelector = null)
    {
        var accessPredicate = accessManager.GetAccessPredicate(userRole);
        if (accessPredicate is null)
        {
            return basePredicate;
        }

        Expression replaceBody = accessPredicate.Parameters[0];
        var parameter = accessPredicate.Parameters[0];
        if (baseToAccessEntitySelector is not null)
        {
            replaceBody = baseToAccessEntitySelector.Body;
            parameter = baseToAccessEntitySelector.Parameters[0];
        }

        var rightVisitor = new ReplaceExpVisitor(replaceBody);
        var right = rightVisitor.Visit(accessPredicate.Body);

        if (basePredicate is null)
        {
            return Expression.Lambda<Func<TBaseEntity, bool>>(right, parameter);
        }

        var leftVisitor = new ReplaceExpVisitor(parameter);
        var left = leftVisitor.Visit(basePredicate.Body);
        var body = Expression.AndAlso(left, right);

        return Expression.Lambda<Func<TBaseEntity, bool>>(body, parameter);
    }

    public static Expression<Func<TBaseEntity, bool>>? ExtendWithAccessPredicate<TBaseEntity, TCollectionEntity, TAccessEntity>(
        this Expression<Func<TBaseEntity, bool>>? basePredicate,
        IEntityAccessManager<TAccessEntity> accessManager,
        UserRole? userRole,
        Expression<Func<TBaseEntity, IEnumerable<TCollectionEntity?>>> baseToCollectionSelector,
        Expression<Func<TCollectionEntity, TAccessEntity>>? collectionToAccessEntitySelector = null)
    {
        var accessPredicate = accessManager.GetAccessPredicate(userRole);
        if (accessPredicate is null)
        {
            return basePredicate;
        }

        ParameterExpression mainParameter = baseToCollectionSelector.Parameters[0];
        Expression collectionSelectorWithValidParams = baseToCollectionSelector;
        if (basePredicate is not null)
        {
            mainParameter = basePredicate.Parameters[0];
            if (mainParameter != baseToCollectionSelector.Parameters[0])
            {
                var collectionSelectorVisitor = new ReplaceExpVisitor(mainParameter);
                collectionSelectorWithValidParams = collectionSelectorVisitor.Visit(baseToCollectionSelector.Body);
            }
        }

        Expression accessExpressionWithValidParams = accessPredicate;
        if (collectionToAccessEntitySelector is not null)
        {
            var accessExpressionVisitor = new ReplaceExpVisitor(collectionToAccessEntitySelector.Body);
            accessExpressionWithValidParams = accessExpressionVisitor.Visit(accessPredicate);
        }

        var typeAnyMethod = typeof(Enumerable).GetMethods()
            .First(m => m.Name == "Any" && m.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(TAccessEntity));

        var typeAnyExpression = Expression.Call(
            null, typeAnyMethod, collectionSelectorWithValidParams, accessExpressionWithValidParams);

        var typeAnyPredicate = Expression.Lambda<Func<TBaseEntity, bool>>(typeAnyExpression, mainParameter);

        if (basePredicate is null)
        {
            return typeAnyPredicate;
        }

        var resultBinaryExpression = Expression.AndAlso(basePredicate.Body, typeAnyPredicate.Body);

        return Expression.Lambda<Func<TBaseEntity, bool>>(resultBinaryExpression, mainParameter);
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
