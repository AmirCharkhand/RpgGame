using RPG.Application.Services;
using RPG.Application.Services.Contracts;

namespace RPG.Application.Extensions;

public static class BrowserCheckServiceExtension
{
    public static IServiceCollection AddBrowserCheckServiceExtension(this IServiceCollection services)
    {
        //For using services in a middleware you have to add the Service as A Singleton!
        services.AddSingleton<IBrowserCheckService, BrowserCheckService>();
        return services;
    }
}