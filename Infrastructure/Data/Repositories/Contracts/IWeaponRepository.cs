using RPG.Domain.Models;
using RPG.Infrastructure.Data.Services;

namespace RPG.Infrastructure.Data.Repositories.Contracts;

public interface IWeaponRepository : IRepository<Weapon,int>
{
    public Task<ServiceResponse<Character>> AddWeapon(Weapon newWeapon);
}