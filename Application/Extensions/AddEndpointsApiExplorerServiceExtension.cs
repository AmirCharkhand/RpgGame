using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace RPG.Application.Extensions;

public static class AddEndpointsApiExplorerServiceExtension
{
    public static IServiceCollection AddEndPointsApiExplorerService(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer()
            .AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme()
                {
                    Description = "Standard Authorization Header using the bearer scheme, e.g. \"bearer {token} \"",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });
        return services;
    }
}