using AutoMapper;
using RPG.Application.Models.FightDtos;
using RPG.Domain.Models;

namespace RPG.Application.Mapping;

public class FightProfile : Profile
{
    public FightProfile()
    {
        CreateMap<Character, GetHighScoreDto>();
    }
}