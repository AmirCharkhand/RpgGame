using Microsoft.Extensions.Primitives;

namespace RPG.Application.Services.Contracts;

public interface IBrowserCheckService
{
    bool IsEdge(StringValues values);
}