using RPG.Application.Services;
using RPG.Application.Services.Contracts;

namespace RPG.Application.Extensions;

public static class AuthServiceExtension
{
    public static IServiceCollection AddAuthServiceExtension(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        return services;
    }
}