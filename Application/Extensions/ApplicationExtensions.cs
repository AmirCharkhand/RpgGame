using System.Reflection;
using RPG.MiddleWares.Extensions;

namespace RPG.Application.Extensions;

public static class ApplicationExtensions
{
    public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();

        services
            .AddAuthenticationService(configuration)
            .AddEndPointsApiExplorerService()
            .AddDbContextService(configuration)
            .AddAutoMapper(Assembly.GetExecutingAssembly())
            .AddHttpContextAccessor()
            .AddCorsExtension()
            .AddHashServiceExtension()
            .AddUserServiceExtension()
            .AddAuthServiceExtension()
            .AddCharacterServiceExtension()
            .AddSkillServiceExtension()
            .AddFightServiceExtension()
            .AddWeaponServiceExtension()
            .AddBrowserCheckServiceExtension()
            .AddResponseEditingServiceExtension()
            .AddExpressionBuilderExtension();
    }

    public static void UseBrowserCheck(this WebApplication app)
    {
        app.UseResponseEditingMiddleware()
            .UseBrowserCheckMiddleware();
    }
}