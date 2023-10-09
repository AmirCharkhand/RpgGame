using RPG.Application.Models;
using RPG.Application.Models.CharacterDtos;
using RPG.Domain.Models;
using RPG.Infrastructure.Data.Paging;
using RPG.Infrastructure.Data.Services;

namespace RPG.Infrastructure.Data.Repositories.Contracts;

public interface ICharacterRepository : IRepository<Character,int>
{
    Task<ServiceResponse<PagedList<Character>>> GetOwnedCharacters(SortDto? sortDto, PagingParam? pagingParam);
    Task<ServiceResponse<PagedList<GetUniversalCharacterDto>>> GetUniversalCharacters(SortDto? sortDto, PagingParam? pagingParam);
    Task<ServiceResponse<PagedList<GetUniversalCharacterDto>>> UniversalCharacterSearch(string searchText, SortDto? sortDto, PagingParam? pagingParam);
    Task<ServiceResponse<Character>> GetCharacterById(int id);
    Task<ServiceResponse<PagedList<Character>>> FilterCharacter(List<FilterDto> filterDtos,SortDto? sortDto, PagingParam? pagingParam);
    Task<ServiceResponse<Character>> AddCharacter(Character newCharacter);
    Task<ServiceResponse<Character>> ModifyCharacter(Character toModify);
    Task<ServiceResponse<Character>> DeleteCharacter(int id);
    Task<ServiceResponse<Character>> DeleteCharacters(List<int> ids);
    Task<ServiceResponse<IEnumerable<Skill>>> AddCharacterSkill(int characterId, int skillId);
    Task<ServiceResponse<IEnumerable<Skill>>> RemoveCharacterSkill(int characterId, int skillId);
}