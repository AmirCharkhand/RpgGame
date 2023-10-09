using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RPG.Application.Models.CharacterDtos;
using RPG.Application.Models.WeaponDtos;
using RPG.Domain.Models;
using RPG.Infrastructure.Data.Repositories.Contracts;

namespace RPG.Controllers;

[Authorize]
[ApiController]
[Route("API/Character/[controller]")]
public class WeaponController : ControllerBase
{
    private readonly IWeaponRepository _repository;
    private readonly IMapper _mapper;

    public WeaponController(IWeaponRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<GetOwnedCharacterDto>> AddWeapon([FromBody] AddWeaponDto newWeapon)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var weapon = _mapper.Map<Weapon>(newWeapon);
        var response = await _repository.AddWeapon(weapon);
        if (!response.Success) return BadRequest(response.Message);
        var result = _mapper.Map<GetOwnedCharacterDto>(response.Data);
        return Ok(result);
    }
}