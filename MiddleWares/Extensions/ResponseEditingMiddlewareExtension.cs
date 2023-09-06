namespace RPG.MiddleWares.Extensions;

public static class ResponseEditingMiddlewareExtension
{
    public static WebApplication UseResponseEditingMiddleware(this WebApplication app)
    {
        app.UseMiddleware<ResponseEditingMiddleware>();
        return app;
    }
}