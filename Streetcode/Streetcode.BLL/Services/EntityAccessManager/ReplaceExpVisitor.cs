using System.Linq.Expressions;

namespace Streetcode.BLL.Services.EntityAccessManager;

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