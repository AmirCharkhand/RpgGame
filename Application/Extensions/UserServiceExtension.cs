using RPG.Infrastructure.Data.Repositories.Contracts;
using RPG.Infrastructure.Data.Repositories.Core;

namespace RPG.Application.Extensions;

public static class UserServiceExtension
{
    public static IServiceCollection AddUserServiceExtension(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }
}