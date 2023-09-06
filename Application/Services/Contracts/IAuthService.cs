using RPG.Infrastructure.Data.Services;

namespace RPG.Application.Services.Contracts;

public interface IAuthService
{
    public Task<ServiceResponse<string>> Login(string userName, string password);
}