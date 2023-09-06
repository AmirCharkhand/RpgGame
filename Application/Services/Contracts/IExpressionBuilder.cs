using System.Linq.Expressions;
using RPG.Application.Models;

namespace RPG.Application.Services.Contracts;

public interface IExpressionBuilder
{
    Expression BuildExpression<T>(T value, ExpressionOperatorType operatorType, string propertyName, ParameterExpression parameterExpression, Expression? currentExpression);
}