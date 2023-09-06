
using RPG.MiddleWares.Services.Contracts;

namespace RPG.MiddleWares;

public class ResponseEditingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IResponseEditingService _responseEditingService;

    public ResponseEditingMiddleware(RequestDelegate next, IResponseEditingService responseEditingService)
    {
        _next = next;
        _responseEditingService = responseEditingService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _next.Invoke(context);
        if (context.Items["IsEdge"] is not bool isEdge)
        {
            await _responseEditingService.WriteBadResponseMessage(context.Response, "Could NOT Determine Client Browser");
            return;
        }
        if (isEdge)
            await _responseEditingService.WriteBadResponseMessage(context.Response, "Edge is NOT Supported");
    }
}