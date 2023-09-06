using AutoMapper;
using RPG.Application.Models.SkillDtos;
using RPG.Domain.Models;

namespace RPG.Application.Mapping;

public class SkillProfile : Profile
{
    public SkillProfile()
    {
        CreateMap<Skill, GetSkillDto>();
    }
}