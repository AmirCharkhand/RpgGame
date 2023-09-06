using System.Linq.Expressions;
using RPG.Application.Models;
using RPG.Application.Services.Contracts;

namespace RPG.Application.Services;

public class ExpressionBuilderService : IExpressionBuilder
{
    public Expression BuildExpression<T>(T value, ExpressionOperatorType operatorType, string propertyName, ParameterExpression parameterExpression, Expression? currentExpression)
    {
        var valueToTest = Expression.Constant(value);
        var parameterProperty = Expression.Property(parameterExpression, propertyName);
        Expression? operatorExpression;

        switch (operatorType)
        {
            case ExpressionOperatorType.Equal:
                operatorExpression = Expression.Equal(parameterProperty, valueToTest);
                break;
            case ExpressionOperatorType.GraterThan:
                operatorExpression = Expression.GreaterThan(parameterProperty, valueToTest);
                break;
            case ExpressionOperatorType.LessThan:
                operatorExpression = Expression.LessThan(parameterProperty, valueToTest);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(operatorType), operatorType, null);
        }

        return currentExpression == null ? operatorExpression : Expression.And(currentExpression, operatorExpression);
    }
}