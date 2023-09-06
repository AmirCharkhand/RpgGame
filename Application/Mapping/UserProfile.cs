using AutoMapper;
using RPG.Application.Models.UserDtos;
using RPG.Domain.Models;

namespace RPG.Application.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<NewUserDto, User>();
    }
}