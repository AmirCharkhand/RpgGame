using RPG.Domain.Models;
using RPG.Infrastructure.Data.Services;

namespace RPG.Infrastructure.Data.Repositories.Contracts;

public interface IUserRepository : IRepository<User,int>
{
    Task<ServiceResponse<int>> AddUser(string userName, string password);
    Task<ServiceResponse<User>> GetUser(string userName);
}