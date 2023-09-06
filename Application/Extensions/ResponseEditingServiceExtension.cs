using RPG.MiddleWares.Services;
using RPG.MiddleWares.Services.Contracts;

namespace RPG.Application.Extensions;

public static class ResponseEditingServiceExtension
{
    public static IServiceCollection AddResponseEditingServiceExtension(this IServiceCollection services)
    {
        //For using services in a middleware you have to add the Service as A Singleton!
        services.AddSingleton<IResponseEditingService, ResponseEditingService>();
        return services;
    }
}