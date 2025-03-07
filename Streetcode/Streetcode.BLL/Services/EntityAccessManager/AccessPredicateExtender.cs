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

        ParameterExpression mainParameter = accessPredicate.Parameters[0];
        Expression accessEntitySelectorWithValidParams = accessPredicate.Parameters[0];
        if (baseToAccessEntitySelector is not null)
        {
            mainParameter = baseToAccessEntitySelector.Parameters[0];
        }

        if (basePredicate is not null)
        {
            mainParameter = basePredicate.Parameters[0];
            accessEntitySelectorWithValidParams = basePredicate.Parameters[0];
        }

        if (baseToAccessEntitySelector is not null)
        {
            accessEntitySelectorWithValidParams = baseToAccessEntitySelector;

            if (accessEntitySelectorWithValidParams != mainParameter)
            {
                var accessEntitySelectorVisitor = new ReplaceExpVisitor(mainParameter);
                accessEntitySelectorWithValidParams = accessEntitySelectorVisitor.Visit(baseToAccessEntitySelector.Body);
            }
        }

        var accessExpressionVisitor = new ReplaceExpVisitor(accessEntitySelectorWithValidParams);
        var accessExpressionWithValidParams = accessExpressionVisitor.Visit(accessPredicate.Body);

        if (basePredicate is null)
        {
            return Expression.Lambda<Func<TBaseEntity, bool>>(accessExpressionWithValidParams, mainParameter);
        }

        var resultBinaryExpression = Expression.AndAlso(basePredicate.Body, accessExpressionWithValidParams);

        return Expression.Lambda<Func<TBaseEntity, bool>>(resultBinaryExpression, mainParameter);
    }

    public static Expression<Func<TBaseEntity, bool>>? ExtendWithAccessPredicate<TBaseEntity, TCollectionEntity, TAccessEntity>(
        this Expression<Func<TBaseEntity, bool>>? basePredicate,
        IEntityAccessManager<TAccessEntity> accessManager,
        UserRole? userRole,
        Expression<Func<TBaseEntity, IEnumerable<TCollectionEntity?>>> baseToCollectionSelector,
        Expression<Func<TCollectionEntity, TAccessEntity>>? collectionToAccessEntitySelector = null)
    {
        Expression<Func<TAccessEntity, bool>>? accessPredicate = accessManager.GetAccessPredicate(userRole);
        if (accessPredicate is null)
        {
            return basePredicate;
        }

        ParameterExpression mainParameter = baseToCollectionSelector.Parameters[0];
        Expression collectionSelectorWithValidParams = baseToCollectionSelector.Body;
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
            accessExpressionWithValidParams = accessExpressionVisitor.Visit(accessPredicate.Body);

            accessExpressionWithValidParams = Expression.Lambda<Func<TCollectionEntity, bool>>(accessExpressionWithValidParams, collectionToAccessEntitySelector.Parameters[0]);
        }

        var typeAnyMethod = typeof(Enumerable).GetMethods()
            .First(m => m.Name == "Any" && m.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(TCollectionEntity));

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
