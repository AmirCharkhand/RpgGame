using AutoMapper;
using RPG.Application.Models.WeaponDtos;
using RPG.Domain.Models;

namespace RPG.Application.Mapping;

public class WeaponProfile : Profile
{
    public WeaponProfile()
    {
        CreateMap<AddWeaponDto, Weapon>();
        CreateMap<Weapon, GetWeaponDto>();
    }
}