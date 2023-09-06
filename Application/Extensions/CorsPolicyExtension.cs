namespace RPG.Application.Extensions;

public static class CorsPolicyExtension
{
    public static IServiceCollection AddCorsExtension(this IServiceCollection services)
    {
        return services.AddCors(options =>
        {
            options.AddDefaultPolicy(policyBuilder =>
            {
                policyBuilder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithExposedHeaders("X_TotalCount")
                    .WithOrigins("https://localhost:7166");
            });
        });
    }
}