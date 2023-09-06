using Microsoft.Extensions.Primitives;
using RPG.Application.Services.Contracts;

namespace RPG.Application.Services;

public class BrowserCheckService : IBrowserCheckService
{
    public bool IsEdge(StringValues values)
    {
        return values.Any(s => s.Contains("Microsoft Edge"));
    }
}