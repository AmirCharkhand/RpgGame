using RPG.Application.Services.Contracts;

namespace RPG.MiddleWares;

public class BrowserCheckMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IBrowserCheckService _browserCheckService;

    public BrowserCheckMiddleware(RequestDelegate requestDelegate, IBrowserCheckService browserCheckService)
    {
        _next = requestDelegate;
        _browserCheckService = browserCheckService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (_browserCheckService.IsEdge(context.Request.Headers["sec-ch-ua"]))
            context.Items.Add("IsEdge", true);
        else
        {
            context.Items.Add("IsEdge", false);
            await _next.Invoke(context);
        }
    }
}