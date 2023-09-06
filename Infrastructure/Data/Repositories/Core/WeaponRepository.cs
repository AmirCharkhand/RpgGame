using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPG.Domain.Models;
using RPG.Infrastructure.Data.Repositories.Contracts;
using RPG.Infrastructure.Data.Services;

namespace RPG.Infrastructure.Data.Repositories.Core;

public class WeaponRepository : Repository<Weapon,int>, IWeaponRepository
{
    private readonly ICharacterRepository _characterRepository;
    public WeaponRepository(DataContext dbContext, IHttpContextAccessor contextAccessor, ICharacterRepository characterRepository) : base(dbContext, contextAccessor)
    {
        _characterRepository = characterRepository;
    }

    public async Task<ServiceResponse<Character>> AddWeapon(Weapon newWeapon)
    {
        var response = new ServiceResponse<Character>();
        try
        {
            var character = await _characterRepository
                .Filter(c => c.User!.Id == UserId)
                .FirstOrDefaultAsync(c => c.Id == newWeapon.CharacterRef);
            if (character == null)
            {
                response.Success = false;
                response.Message = "Wrong Character ID";
            }
            else
            {
                newWeapon.Character = character;
                await Add(newWeapon);
                response.Data = character;
            }
        }
        catch (Exception e)
        {
            response.Message = e.Message;
            response.Success = false;
        }

        return response;
    }
}