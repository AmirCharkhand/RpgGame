using RPG.Application.Models;
using RPG.Application.Services.Contracts;
using RPG.Infrastructure.Data.Repositories.Contracts;
using RPG.Infrastructure.Data.Services;

namespace RPG.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IHashService _hashService;

    public AuthService(IUserRepository userRepository, IHashService hashService)
    {
        _userRepository = userRepository;
        _hashService = hashService;
    }
    
    public async Task<ServiceResponse<AuthDto>> Login(string userName, string password)
    {
        var response = new ServiceResponse<AuthDto>();
        var userResponse = await _userRepository.GetUser(userName);
        if (!userResponse.Success)
        {
            response.Success = false;
            response.Message = userResponse.Message;
            return response;
        }
        
        if (_hashService.VerifyHash(password, userResponse.Data!.PasswordHash, userResponse.Data.PasswordSalt))
        {
            var loginDto = new AuthDto()
            {
                Jwt = _hashService.CreateJwt(userResponse.Data),
                UserName = userResponse.Data.Username
            };
            response.Data = loginDto;
            response.Message = "Logged in";
        }
        else
        {
            response.Message = "Wrong Password";
            response.Success = false;
        }

        return response;
    }
}