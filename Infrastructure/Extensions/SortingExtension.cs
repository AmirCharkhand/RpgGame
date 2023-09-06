using System.Linq.Expressions;
using RPG.Application.Models;

namespace RPG.Infrastructure.Extensions;

public static class SortingExtension
{
    public static IQueryable<T> Sort<T>(this IQueryable<T> query, SortDto? sortDto)
    {
        if (sortDto == null || string.IsNullOrEmpty(sortDto.PropertyName)) return query;
        
        var type = typeof(T);
        var property = type.GetProperty(sortDto.PropertyName);
        var parameter = Expression.Parameter(type, "p");
        var propertyAccess = Expression.MakeMemberAccess(parameter, property);
        var orderByExp = Expression.Lambda(propertyAccess, parameter);
        MethodCallExpression resultExp;
        if (sortDto.Ascending)
        {
            resultExp = Expression.Call(
                typeof(Queryable),
                "OrderBy",
                new Type[] { type, property.PropertyType },
                query.Expression,
                Expression.Quote(orderByExp));
        }
        else
        {
            resultExp = Expression.Call(
                typeof(Queryable),
                "OrderByDescending",
                new Type[] { type, property.PropertyType },
                query.Expression,
                Expression.Quote(orderByExp));
        }
        return query.Provider.CreateQuery<T>(resultExp);
    }
}