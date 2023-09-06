namespace RPG.MiddleWares.Services.Contracts;

public interface IResponseEditingService
{
    Task WriteBadResponseMessage(HttpResponse response, string message);
}