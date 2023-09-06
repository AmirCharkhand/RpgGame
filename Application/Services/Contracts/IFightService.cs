using RPG.Application.Models.FightDtos;
using RPG.Domain.Models;
using RPG.Infrastructure.Data.Paging;
using RPG.Infrastructure.Data.Services;

namespace RPG.Application.Services.Contracts;

public interface IFightService
{
    Task<ServiceResponse<AttackResultDto>> WeaponAttack(int attackerId, int opponentId);
    Task<ServiceResponse<AttackResultDto>> SkillAttack(int attackerId, int opponentId, int skillId);
    Task<ServiceResponse<FightResultDto>> Fight(List<int>? charactersIds);
    Task<ServiceResponse<PagedList<Character>>> GetHighScores(PagingParam? pagingParam);
}