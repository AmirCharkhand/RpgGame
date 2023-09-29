using RPG.Application.Models;
using RPG.Infrastructure.Data.Services;

namespace RPG.Application.Services.Contracts;

public interface IAuthService
{
    public Task<ServiceResponse<AuthDto>> Login(string userName, string password);
}