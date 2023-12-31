﻿using AutoMapper;
using RPG.Application.Models.CharacterDtos;
using RPG.Domain.Models;

namespace RPG.Application.Mapping;

public class CharacterProfile : Profile
{
    public CharacterProfile()
    {
        CreateMap<Character, GetOwnedCharacterDto>();
        CreateMap<Character, GetUniversalCharacterDto>();
        CreateMap<AddCharacterDto, Character>();
        CreateMap<ModifyCharacterDto, Character>();
    }
}