using RPG.Application.Services;
using RPG.Application.Services.Contracts;

namespace RPG.Application.Extensions;

public static class FightServiceExtension
{
    public static IServiceCollection AddFightServiceExtension(this IServiceCollection services)
    {
        services.AddScoped<IFightService, FightService>();
        return services;
    }
}