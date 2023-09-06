using Microsoft.EntityFrameworkCore;
using RPG.Application.Models.FightDtos;
using RPG.Application.Services.Contracts;
using RPG.Domain.Models;
using RPG.Infrastructure.Data.Paging;
using RPG.Infrastructure.Data.Repositories.Contracts;
using RPG.Infrastructure.Data.Services;
using RPG.Infrastructure.Extensions;

namespace RPG.Application.Services;

public class FightService : IFightService
{
    private readonly ICharacterRepository _characterRepository;
    private readonly List<Character> _updateList = new ();

    public FightService(ICharacterRepository characterRepository)
    {
        _characterRepository = characterRepository;
    }

    public async Task<ServiceResponse<AttackResultDto>> WeaponAttack(int attackerId, int opponentId)
    {
        var response = new ServiceResponse<AttackResultDto>();

        try
        {
            var attacker = await _characterRepository.GetById(attackerId);
            var opponent = await _characterRepository.GetById(opponentId);

            CheckCharactersForNull(attacker, opponent, response);
            if (!response.Success) return response;

            _updateList.AddRange(new [] { attacker!, opponent! });
            FightWithWeapon(attacker!, opponent!, out var attackResult, out var opponentDead);
            IncreaseFights(attacker!, opponent!);
            if (opponentDead) IncreaseVictoriesAndDefeats(attacker!, opponent!);
            await UpdateCharactersInDb();

            response.Message = attackResult;
            response.Data = new AttackResultDto()
            {
                AttackerName = attacker!.Name,
                OpponentName = opponent!.Name,
                OpponentHp = opponent.Health,
            };
        }
        catch (Exception e)
        {
            response.Message = e.Message;
            response.Success = false;
        }

        return response;
    }
    
    public async Task<ServiceResponse<AttackResultDto>> SkillAttack(int attackerId, int opponentId, int skillId)
    {
        var response = new ServiceResponse<AttackResultDto>();
        
        try
        {
            var attacker = await _characterRepository.GetById(attackerId);
            var opponent = await _characterRepository.GetById(opponentId);

            CheckCharactersForNull(attacker, opponent, response);
            if (!response.Success) return response;

            _updateList.AddRange(new[] { attacker!, opponent! });
            FightWithSkill(attacker!, skillId, opponent!, out var attackResult, out var opponentDead);
            IncreaseFights(attacker!, opponent!);
            if (opponentDead) IncreaseVictoriesAndDefeats(attacker!, opponent!);
            await UpdateCharactersInDb();

            response.Message = attackResult;
            response.Data = new AttackResultDto()
            {
                AttackerName = attacker!.Name,
                OpponentName = opponent!.Name,
                OpponentHp = opponent.Health
            };
        }
        catch (Exception e)
        {
            response.Message = e.Message;
            response.Success = false;
        }

        return response;
    }

    public async Task<ServiceResponse<FightResultDto>> Fight(List<int>? characterIds)
    {
        var response = new ServiceResponse<FightResultDto>();

        if (characterIds == null || characterIds.Count < 2)
        {
            response.Message = "There must be at least 2 characters for a fight";
            response.Success = false;
            return response;
        }
        
        try
        {
            var characters = await _characterRepository
                .Filter(c => characterIds.Contains(c.Id))
                .ToListAsync();
            
            CheckCharactersValidityForFight(characters, response);
            if (!response.Success) return response;

            _updateList.AddRange(characters);
            var fightEnded = false;
            response.Data = new FightResultDto();
            while (!fightEnded)
            {
                var weaponAttack = new Random().Next(2) == 0;
                SelectRandomFightersForFight(characters, out var attacker, out var opponent);

                if (weaponAttack)
                {
                    FightWithWeapon(attacker, opponent, out var attackResult, out var opponentDead);
                    IncreaseFights(attacker, opponent);
                    if (opponentDead)
                    {
                        IncreaseVictoriesAndDefeats(attacker, opponent);
                        characters.Remove(opponent);
                    }
                    response.Data.Log.Add(attackResult);
                }
                else
                {
                    var skillId = attacker.Skills![new Random().Next(attacker.Skills.Count)].Id;
                    FightWithSkill(attacker, skillId, opponent, out var attackResult, out var opponentDead);
                    IncreaseFights(attacker, opponent);
                    if (opponentDead)
                    {
                        IncreaseVictoriesAndDefeats(attacker, opponent);
                        characters.Remove(opponent);
                    }
                    response.Data.Log.Add(attackResult);
                }

                if (characters.Count >= 2) continue;
                fightEnded = true;
                await UpdateCharactersInDb();
            }
        }
        catch (Exception e)
        {
            response.Message = e.Message;
            response.Success = false;
        }

        return response;
    }

    public async Task<ServiceResponse<PagedList<Character>>> GetHighScores(PagingParam? pagingParam)
    {
        var response = new ServiceResponse<PagedList<Character>>();
        try
        {
            var characters = await _characterRepository
                .Filter(c => true)
                .OrderByDescending(c => c.Victories)
                .ThenBy(c => c.Defeats)
                .CalculatePaging(pagingParam);

            response.Data = characters;
        }
        catch (Exception e)
        {
            response.Success = false;
            response.Message = e.Message;
        }
        
        return response;
    }

    private void IncreaseVictoriesAndDefeats(Character attacker, Character opponent)
    {
        attacker.Victories++;
        opponent.Defeats++;
    }

    private void IncreaseFights(Character attacker, Character opponent)
    {
        attacker.Fights++;
        opponent.Fights++;
    }

    private void FightWithWeapon(Character attacker, Character opponent, out string attackResult, out bool opponentDead)
    {
        if (attacker.Weapon == null)
        {
            attackResult = "Attacker has no Weapon";
            opponentDead = false;
            return;
        }

        var damage = (attacker.Weapon.Damage + new Random().NextDouble() * attacker.Strength) -
                     (new Random().NextDouble() * opponent.Defence);
        
        attackResult = $"{attacker.Name} dealt {damage} damage to {opponent.Name} with their weapon {attacker.Weapon.Name}.";
        if (damage > 0)
        {
            opponent.Health -= (float)damage;
            attackResult += $"now {opponent.Name}'s HP is {opponent.Health}";
        }
        if (opponent.Health <= 0)
        {
            attackResult += $" and they are defeated";
            opponent.Health = 150;
            opponentDead = true;
        }
        else opponentDead = false;
    }

    private void FightWithSkill(Character attacker, int skillId, Character opponent, out string attackResult, out bool opponentDead)
    {
        var skill = attacker.Skills!.FirstOrDefault(s => s.Id == skillId);
        if (skill == null)
        {
            attackResult = $"{attacker.Name} doesn't have this Skill";
            opponentDead = false;
            return;
        }
        
        var damage = (float)(skill.Damage - opponent.Defence * new Random().NextDouble());
        attackResult = $"{attacker.Name} dealt {damage} damage to {opponent.Name} with their Skill {skill.Name}.";

        if (damage > 0)
        {
            opponent.Health -= damage;
            attackResult += $"now {opponent.Name}'s HP is {opponent.Health}";
        }
        if (opponent.Health <= 0)
        {
            attackResult += $" and they are defeated";
            opponent.Health = 150;
            opponentDead = true;
        }
        else opponentDead = false;
    }

    private static void CheckCharactersValidityForFight(List<Character> characters, ServiceResponse<FightResultDto> response)
    {
        if (characters.Count < 2)
        {
            response.Message = "There must be at least 2 valid Characters for a fight";
            response.Success = false;
        }
        else if (characters.All(c => c.Weapon == null) && characters.All(c => c.Skills!.Count == 0))
        {
            response.Message = "All characters have no Weapon and no Skills so F*** Fight lets be friends";
            response.Success = false;
        }
    }

    private void SelectRandomFightersForFight(List<Character> characters, out Character attacker, out Character opponent)
    {
        do
        {
            attacker = characters[new Random().Next(characters.Count)];
            opponent = characters[new Random().Next(characters.Count)];
        } while (attacker == opponent);
    }

    private void CheckCharactersForNull(Character? attacker, Character? opponent, ServiceResponse<AttackResultDto> response)
    {
        if (attacker == null)
        {
            response.Message = "Wrong Attacker ID";
            response.Success = false;
        }
        else if (opponent == null)
        {
            response.Message = "Wrong Opponent ID";
            response.Success = false;
        }
    }
    
    private async Task UpdateCharactersInDb()
    {
        await _characterRepository.GroupUpdate(_updateList);
        _updateList.Clear();
    }
}