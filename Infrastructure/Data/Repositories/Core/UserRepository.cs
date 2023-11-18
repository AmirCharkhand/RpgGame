using Microsoft.EntityFrameworkCore;
using RPG.Application.Services.Contracts;
using RPG.Domain.Models;
using RPG.Infrastructure.Data.Repositories.Contracts;
using RPG.Infrastructure.Data.Services;

namespace RPG.Infrastructure.Data.Repositories.Core;

public class UserRepository : Repository<User,int> , IUserRepository
{
    private readonly IHashService _hashService;
    public UserRepository(DataContext dbContext, IHttpContextAccessor contextAccessor, IHashService hashService) : base(dbContext, contextAccessor)
    {
        _hashService = hashService;
    }

    public async Task<ServiceResponse<int>> AddUser(string userName, string password)
    {
        var response = new ServiceResponse<int>();
        
        try
        {
            if (await IsExist(u => string.Equals(u.Username,userName)))
            {
                response.Success = false;
                response.Message = "This User Name Already Exists";
                return response;
            }
            
            var newUser = new User { Username = userName };
            _hashService.CreateHashWithSalt(password, out byte[] passwordHash, out byte[] passwordSalt);
            newUser.PasswordHash = passwordHash;
            newUser.PasswordSalt = passwordSalt;
            await Add(newUser);
            response.Data = newUser.Id;
        }
        catch (Exception e)
        {
            response.Message = "Couldn't create the user " + e.Message;
            response.Success = false;
        }

        return response;
    }

    public async Task<ServiceResponse<User>> GetUser(string userName)
    {
        var response = new ServiceResponse<User>();
        try
        {
            var user = await Filter(u => string.Equals(u.Username.ToLower(), userName.ToLower()))
                .FirstOrDefaultAsync();
            if (user==null)
            {
                response.Success = false;
                response.Message = "User Doesn't Exists";
            }
            else 
                response.Data = user;
        }
        catch (Exception e)
        {
            response.Success = false;
            response.Message = "Couldn't find the User" + e.Message;
        }

        return response;
    }
}