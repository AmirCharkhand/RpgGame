using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RPG.Application.Models.FightDtos;
using RPG.Application.Services.Contracts;
using RPG.Infrastructure.Data.Paging;
using RPG.Infrastructure.Data.Services;

namespace RPG.Controllers;

[ApiController]
[Route("API/[controller]")]
public class FightController : ControllerBase
{
    private readonly IFightService _service;
    private readonly IMapper _mapper;

    public FightController(IFightService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpPost("WeaponAttack")]
    public async Task<ActionResult<ServiceResponse<AttackResultDto>>> WeaponAttack([FromBody] WeaponAttackDto request)
    {
        var response = await _service.WeaponAttack(request.AttackerId, request.OpponentId);
        if (!response.Success) return BadRequest(response.Message);
        return Ok(response);
    }
    
    [HttpPost("SkillAttack")]
    public async Task<ActionResult<ServiceResponse<AttackResultDto>>> SkillAttack([FromBody] SkillAttackDto request)
    {
        var response = await _service.SkillAttack(request.AttackerId, request.OpponentId, request.SkillId);
        if (!response.Success) return BadRequest(response.Message);
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<FightResultDto>>> Fight([FromBody] FightRequestDto request)
    {
        var response = await _service.Fight(request.CharacterIds);
        if (!response.Success) return BadRequest(response.Message);
        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetHighScoreDto>>> GetHighScores([FromQuery] PagingParam? pagingParam = default)
    {
        var response = await _service.GetHighScores(pagingParam);
        if (!response.Success) return BadRequest(response.Message);
        var result = _mapper.Map<IEnumerable<GetHighScoreDto>>(response.Data!.ToList());
        Response.Headers.Add("X_TotalCount", response.Data!.TotalCount.ToString());
        return Ok(result);
    }
}