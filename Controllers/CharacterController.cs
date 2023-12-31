﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RPG.Application.Models;
using RPG.Application.Models.CharacterDtos;
using RPG.Application.Models.CharacterSkillDtos;
using RPG.Application.Models.SkillDtos;
using RPG.Domain.Models;
using RPG.Infrastructure.Data.Paging;
using RPG.Infrastructure.Data.Repositories.Contracts;

namespace RPG.Controllers;

[Authorize]
[ApiController]
[Route("API/[controller]")]
public class CharacterController : ControllerBase
{
    private readonly ICharacterRepository _repository;
    private readonly IMapper _mapper;
    public CharacterController(ICharacterRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetOwnedCharacterDto>> GetCharacter([FromRoute]int id)
    {
        var response = await _repository.GetCharacterById(id);
        if (!response.Success) return BadRequest(response.Message);

        var result = _mapper.Map<GetOwnedCharacterDto>(response.Data);
        return Ok(result);
    }

    [HttpGet("{searchText}")]
    public async Task<ActionResult<IEnumerable<GetOwnedCharacterDto>>> Search([FromRoute]string searchText, [FromQuery] PagingParam? pagingParam, [FromQuery] SortDto? sortDto)
    {
        var response = await _repository.Search(searchText, sortDto, pagingParam);
        if (!response.Success) return BadRequest(response.Message);
        
        var result = _mapper.Map<IEnumerable<GetOwnedCharacterDto>>(response.Data!.ToList());
        Response.Headers.Add("X_TotalCount", response.Data!.TotalCount.ToString());
        return Ok(result);
    }
    
    [HttpGet("Universal/{searchText}")]
    public async Task<ActionResult<IEnumerable<GetUniversalCharacterDto>>> UniversalSearch([FromRoute]string searchText, [FromQuery] PagingParam? pagingParam, [FromQuery] SortDto? sortDto)
    {
        var response = await _repository.UniversalCharacterSearch(searchText, sortDto, pagingParam);
        if (!response.Success) return BadRequest(response.Message);
        
        Response.Headers.Add("X_TotalCount", response.Data!.TotalCount.ToString());
        return Ok(response.Data);
    }

    [HttpGet]
    [ResponseCache(Duration = 10)]
    public async Task<ActionResult<IEnumerable<GetOwnedCharacterDto>>> GetOwnedCharacters([FromQuery] SortDto? sortDto, [FromQuery] PagingParam? pagingParam = default)
    {
        var response = await _repository.GetOwnedCharacters(sortDto, pagingParam);
        if (!response.Success) return BadRequest(response.Message);

        var result = _mapper.Map<IEnumerable<GetOwnedCharacterDto>>(response.Data!.ToList());
        Response.Headers.Add("X_TotalCount", response.Data!.TotalCount.ToString());
        return Ok(result);
    }
    
    [HttpGet("Universal")]
    [ResponseCache(Duration = 10)]
    public async Task<ActionResult<IEnumerable<GetUniversalCharacterDto>>> GetUniversalCharacters([FromQuery] SortDto? sortDto, [FromQuery] PagingParam? pagingParam = default)
    {
        var response = await _repository.GetUniversalCharacters(sortDto, pagingParam);
        if (!response.Success) return BadRequest(response.Message);
        
        Response.Headers.Add("X_TotalCount", response.Data!.TotalCount.ToString());
        return Ok(response.Data);
    }

    [HttpPost("Filter")]
    public async Task<ActionResult<IEnumerable<GetOwnedCharacterDto>>> FilterCharacters([FromBody] List<FilterDto> filterDtos, 
        [FromQuery] SortDto? sortDto, [FromQuery] PagingParam? pagingParam = default)
    {
        var response = await _repository.FilterCharacter(filterDtos, sortDto, pagingParam);
        if (!response.Success) return BadRequest(response.Message);

        var result = _mapper.Map<IEnumerable<GetOwnedCharacterDto>>(response.Data!.ToList());
        Response.Headers.Add("X_TotalCount", response.Data!.TotalCount.ToString());
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<GetOwnedCharacterDto>> CreateCharacter([FromBody] AddCharacterDto character)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var newCharacter = _mapper.Map<Character>(character);
        var response = await _repository.AddCharacter(newCharacter);
        if (!response.Success) return BadRequest(response.Message);
        var result = _mapper.Map<GetOwnedCharacterDto>(response.Data);
        return Ok(result);
    }
    
    [HttpPut("{id:int}")]
    public async Task<ActionResult<string>> ModifyCharacter([FromRoute] int id, [FromBody] ModifyCharacterDto character)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var toModify = _mapper.Map<Character>(character);
        toModify.Id = id;
        var response = await _repository.ModifyCharacter(toModify);
        if (!response.Success) return BadRequest(response.Message);
        return Ok(response.Message);
    }
    
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<string>> DeleteCharacter([FromRoute] int id)
    {
        var response = await _repository.DeleteCharacter(id);
        if (!response.Success) return BadRequest(response.Message);
        return Ok(response.Message);
    }

    [HttpPost("GroupDelete")]
    public async Task<ActionResult<string>> DeleteCharacters([FromBody] List<int> ids)
    {
        var response = await _repository.DeleteCharacters(ids);
        if (!response.Success) return BadRequest(response.Message);
        return Ok(response.Message);
    }

    [HttpPost("AddSkill")]
    public async Task<ActionResult<List<GetSkillDto>>> AddSkill([FromBody] CharacterSkillDto characterSkill)
    {
        var response = await _repository.AddCharacterSkill(characterSkill.CharacterId, characterSkill.SkillId);
        if (!response.Success) return BadRequest(response.Message);
        var result = _mapper.Map<List<GetSkillDto>>(response.Data);
        return Ok(result);
    }
    
    [HttpPost("RemoveSkill")]
    public async Task<ActionResult<List<GetSkillDto>>> RemoveSkill([FromBody] CharacterSkillDto characterSkill)
    {
        var response = await _repository.RemoveCharacterSkill(characterSkill.CharacterId, characterSkill.SkillId);
        if (!response.Success) return BadRequest(response.Message);
        var result = _mapper.Map<List<GetSkillDto>>(response.Data);
        return Ok(result);
    }
}