using RPG.Infrastructure.Data.Repositories.Contracts;
using RPG.Infrastructure.Data.Repositories.Core;

namespace RPG.Application.Extensions;

public static class WeaponServiceExtension
{
    public static IServiceCollection AddWeaponServiceExtension(this IServiceCollection services)
    {
        services.AddScoped<IWeaponRepository, WeaponRepository>();
        return services;
    }
}