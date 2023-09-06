using RPG.Application.Models;
using RPG.Domain.Models;
using RPG.Infrastructure.Data.Paging;
using RPG.Infrastructure.Data.Services;

namespace RPG.Infrastructure.Data.Repositories.Contracts;

public interface ICharacterRepository : IRepository<Character,int>
{
    Task<ServiceResponse<PagedList<Character>>> GetAll(SortDto? sortDto, PagingParam? pagingParam);
    Task<ServiceResponse<Character>> GetCharacterById(int id);
    Task<ServiceResponse<PagedList<Character>>> FilterCharacter(List<FilterDto> filterDtos,SortDto? sortDto, PagingParam? pagingParam);
    Task<ServiceResponse<Character>> AddCharacter(Character newCharacter);
    Task<ServiceResponse<Character>> ModifyCharacter(Character toModify);
    Task<ServiceResponse<Character>> DeleteCharacter(int id);
    Task<ServiceResponse<Character>> AddCharacterSkill(int characterId, int skillId);
}