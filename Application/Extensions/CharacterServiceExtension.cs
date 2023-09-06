using RPG.Infrastructure.Data.Repositories.Contracts;
using RPG.Infrastructure.Data.Repositories.Core;

namespace RPG.Application.Extensions;

public static class CharacterServiceExtension
{
    public static IServiceCollection AddCharacterServiceExtension(this IServiceCollection services)
    {
        services.AddScoped<ICharacterRepository, CharacterRepository>();
        return services;
    }
}