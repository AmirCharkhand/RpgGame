using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RPG.Application.Models.SkillDtos;
using RPG.Infrastructure.Data.Repositories.Contracts;

namespace RPG.Controllers;

[ApiController]
[Route("API/[controller]")]
public class SkillController : ControllerBase
{
    private readonly ISkillRepository _repository;
    private readonly IMapper _mapper;

    public SkillController(ISkillRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetSkillDto>>> GetSkills()
    {
        try
        {
            var response = await _repository.GetAll();
            var result = _mapper.Map<List<GetSkillDto>>(response);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest("Couldn't retrieve Skills");
        }
    }
}