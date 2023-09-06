using Microsoft.EntityFrameworkCore;
using RPG.Infrastructure.Data;

namespace RPG.Application.Extensions;

public static class AddDbContextServiceExtension
{
    public static IServiceCollection AddDbContextService(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddDbContext<DataContext>(options =>
            options
                .UseLazyLoadingProxies()
                .UseSqlServer(configuration["RpgConnection"]));
    }
}