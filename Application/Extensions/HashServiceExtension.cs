using RPG.Application.Services;
using RPG.Application.Services.Contracts;

namespace RPG.Application.Extensions;

public static class HashServiceExtension
{
    public static IServiceCollection AddHashServiceExtension(this IServiceCollection services)
    {
        services.AddScoped<IHashService, HashService>();
        return services;
    }
}