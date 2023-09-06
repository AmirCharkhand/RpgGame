namespace RPG.MiddleWares.Extensions;

public static class BrowserCheckMiddlewareExtension
{
    public static WebApplication UseBrowserCheckMiddleware(this WebApplication app)
    {
        app.UseMiddleware<BrowserCheckMiddleware>();
        return app;
    }
}