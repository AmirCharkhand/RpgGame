using Microsoft.AspNetCore.Mvc;
using RPG.Application.Models.UserDtos;
using RPG.Application.Services.Contracts;
using RPG.Infrastructure.Data.Repositories.Contracts;

namespace RPG.Controllers;

[Controller]
[Route("API/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IUserRepository _userRepository;

    public AuthController(IAuthService authService, IUserRepository userRepository)
    {
        _authService = authService;
        _userRepository = userRepository;
    }

    [HttpPost("Register")]
    public async Task<ActionResult<int>> RegisterUser([FromBody]NewUserDto newUser)
    {
        var response = await _userRepository.AddUser(newUser.Username, newUser.Password);
        if (!response.Success) return BadRequest(response.Message);
        return Ok(response.Data);
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login([FromBody] UserLoginDto user)
    {
        var response = await _authService.Login(user.UserName, user.Password);
        if (!response.Success) return BadRequest(response.Message);
        return Ok(response.Data);
    }
}