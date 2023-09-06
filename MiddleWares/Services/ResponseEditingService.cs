using RPG.MiddleWares.Services.Contracts;

namespace RPG.MiddleWares.Services;

public class ResponseEditingService : IResponseEditingService
{
    public async Task WriteBadResponseMessage(HttpResponse response, string message)
    {
        response.StatusCode = 400;
        await response.WriteAsJsonAsync(message);
    }
}