using RPG.Application.Services;
using RPG.Application.Services.Contracts;

namespace RPG.Application.Extensions;

public static class ExpressionBuilderServiceExtension
{
    public static IServiceCollection AddExpressionBuilderExtension(this IServiceCollection services)
    {
        services.AddScoped<IExpressionBuilder, ExpressionBuilderService>();
        return services;
    }
}