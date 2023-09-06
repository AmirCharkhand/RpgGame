using RPG.Infrastructure.Data.Repositories.Contracts;
using RPG.Infrastructure.Data.Repositories.Core;

namespace RPG.Application.Extensions;

public static class SkillServiceExtension
{
    public static IServiceCollection AddSkillServiceExtension(this IServiceCollection services)
    {
        services.AddScoped<ISkillRepository, SkillRepository>();
        return services;
    }
}