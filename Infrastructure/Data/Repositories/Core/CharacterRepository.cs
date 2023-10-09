using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RPG.Application.Models;
using RPG.Application.Models.CharacterDtos;
using RPG.Application.Services.Contracts;
using RPG.Domain.Models;
using RPG.Infrastructure.Data.Paging;
using RPG.Infrastructure.Data.Repositories.Contracts;
using RPG.Infrastructure.Data.Services;
using RPG.Infrastructure.Extensions;

namespace RPG.Infrastructure.Data.Repositories.Core;

public class CharacterRepository : Repository<Character,int> , ICharacterRepository
{
    private readonly IExpressionBuilder _expressionBuilder;
    private readonly IUserRepository _userRepository;
    private readonly ISkillRepository _skillRepository;
    private readonly IMapper _mapper;
    public CharacterRepository(DataContext dbContext, IHttpContextAccessor contextAccessor, IExpressionBuilder expressionBuilder, IUserRepository userRepository, ISkillRepository skillRepository, IMapper mapper) : base(dbContext, contextAccessor)
    {
        _expressionBuilder = expressionBuilder;
        _userRepository = userRepository;
        _skillRepository = skillRepository;
        _mapper = mapper;
    }

    public override async Task<ServiceResponse<PagedList<Character>>> Search(string searchText,SortDto? sortDto, PagingParam? pagingParam = default)
    {
        var response = new ServiceResponse<PagedList<Character>>();
        try
        {
            var result = await Filter(c => EF.Functions.Like(c.Name, $"%{searchText}%"))
                .Include(c => c.Weapon)
                .Where(c => c.User!.Id == UserId)
                .Sort(sortDto)
                .CalculatePaging(pagingParam);
            response.Data = result;
        }
        catch (Exception e)
        {
            response.Message = e.Message;
            response.Success = false;
        }

        return response;
    }

    public async Task<ServiceResponse<PagedList<Character>>> GetOwnedCharacters(SortDto? sortDto, PagingParam? pagingParam = default)
    {
        var response = new ServiceResponse<PagedList<Character>>();
        try
        {
            var result = await Filter(c => c.User!.Id == UserId)
                .Include(c => c.Weapon)
                .Sort(sortDto)
                .CalculatePaging(pagingParam);
            response.Data = result;
        }
        catch (Exception e)
        {
            response.Success = false;
            response.Message = "Couldn't fetch data from Database! " + e.Message;
        }
        return response;
    }

    public async Task<ServiceResponse<PagedList<GetUniversalCharacterDto>>> GetUniversalCharacters(SortDto? sortDto, PagingParam? pagingParam)
    {
        var response = new ServiceResponse<PagedList<GetUniversalCharacterDto>>();
        try
        {
            var characters = await Filter(c => true)
                .Include(c => c.User)
                .Include(c => c.Weapon)
                .Sort(sortDto)
                .CalculatePaging(pagingParam);
            response.Data = DistinguishOwnedCharacters(characters);
        }
        catch (Exception e)
        {
            response.Success = false;
            response.Message = "Couldn't fetch data from Database! " + e.Message;
        }

        return response;
    }

    public async Task<ServiceResponse<PagedList<GetUniversalCharacterDto>>> UniversalCharacterSearch(string searchText, SortDto? sortDto, PagingParam? pagingParam)
    {
        var response = new ServiceResponse<PagedList<GetUniversalCharacterDto>>();
        try
        {
            var characters = await Filter(c => EF.Functions.Like(c.Name, $"%{searchText}%"))
                .Include(c => c.User)
                .Include(c => c.Weapon)
                .Sort(sortDto)
                .CalculatePaging(pagingParam);
            response.Data = DistinguishOwnedCharacters(characters);
        }
        catch (Exception e)
        {
            response.Success = false;
            response.Message = "Couldn't fetch data from Database! " + e.Message;
        }

        return response;
    }

    private PagedList<GetUniversalCharacterDto> DistinguishOwnedCharacters(PagedList<Character> characters)
    {
        var universalCharacters = new List<GetUniversalCharacterDto>();
        foreach (var character in characters)
        {
            var universalCharacter = _mapper.Map<GetUniversalCharacterDto>(character);
            if (character.User!.Id == UserId) universalCharacter.Owned = true;
            universalCharacters.Add(universalCharacter);
        }

        return new PagedList<GetUniversalCharacterDto>(universalCharacters, characters.TotalCount);
    }

    public async Task<ServiceResponse<Character>> GetCharacterById(int id)
    {
        var response = new ServiceResponse<Character>();
        
        try
        {
            var character = await Filter(c => c.User!.Id == UserId)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (character == null)
            {
                response.Message = "Wrong Character ID";
                response.Success = false;
                return response;
            }

            response.Data = character;
        }
        catch (Exception e)
        {
            response.Message = e.Message;
            response.Success = false;
        }

        return response;
    }

    public async Task<ServiceResponse<PagedList<Character>>> FilterCharacter(List<FilterDto> filterDtos, SortDto? sortDto, PagingParam? pagingParam = default)
    {
        var response = new ServiceResponse<PagedList<Character>>();
        var characterParameter = Expression.Parameter(typeof(Character));
        Expression? expression = null;
        foreach (var filterDto in filterDtos)
        {
            switch (filterDto.PropertyName)
            {
                case "Health" or "Strength" or "Attack" or "Defence":
                {
                    var value = float.Parse(filterDto.Value);
                    expression = _expressionBuilder.BuildExpression(value, filterDto.OperatorType, filterDto.PropertyName, characterParameter, expression);
                    break;
                }
                case "Fights" or "Victories" or "Defeats":
                {
                    var value = int.Parse(filterDto.Value);
                    expression = _expressionBuilder.BuildExpression(value, filterDto.OperatorType, filterDto.PropertyName, characterParameter, expression);
                    break;
                }
                case "CharacterType":
                {
                    var value = Enum.Parse<CharacterType>(filterDto.Value, true);
                    expression = _expressionBuilder.BuildExpression(value, filterDto.OperatorType, filterDto.PropertyName, characterParameter, expression);
                    break;
                }
                default:
                {
                    response.Success = false;
                    response.Message += $" {filterDto.PropertyName} is not a valid Property for Character";
                    break;
                }
            }
        }
        if (!response.Success) return response;
        var func = Expression.Lambda<Func<Character, bool>>(expression!, false, new List<ParameterExpression>() { characterParameter });

        try
        {
            var characters = await Filter(func)
                .Include(c => c.Weapon)
                .Where(c => c.User!.Id == UserId)
                .Sort(sortDto)
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
    
    public async Task<ServiceResponse<Character>> AddCharacter(Character newCharacter)
    {
        var response = new ServiceResponse<Character>();
        try
        {
            var user = await _userRepository.Filter(u => u.Id == UserId).FirstAsync();
            newCharacter.User = user;
            await Add(newCharacter);
            response.Data = newCharacter;
        }
        catch (Exception e)
        {
            response.Success = false;
            response.Message = "Couldn't add the Character! " + e.Message;
        }

        return response;
    }
    
    public async Task<ServiceResponse<Character>> ModifyCharacter(Character toModify)
    {
        var response = new ServiceResponse<Character>();
        try
        {
            if (!await Set.AnyAsync(c => c.User!.Id == UserId))
            {
                response.Message = "this character is not belong to current User!";
                response.Success = false;
                return response;
            }

            await Update(toModify);
            response.Data = toModify;
            response.Message = "Character successfully Modified";
        }
        catch (Exception e)
        {
            response.Message = e.Message;
            response.Success = false;
        }

        return response;
    }
    
    public async Task<ServiceResponse<Character>> DeleteCharacter(int id)
    {
        var response = new ServiceResponse<Character>();
        try
        {
            var character = await GetACharacter(id, response);
            if (!response.Success) return response;
            
            await Delete(character!);
            response.Message = "Character successfully Removed";
        }
        catch (Exception e)
        {
            response.Message = e.Message;
            response.Success = false;
        }

        return response;
    }

    public async Task<ServiceResponse<Character>> DeleteCharacters(List<int> ids)
    {
        var response = new ServiceResponse<Character>();
        try
        {
            var characters = await Filter(c => c.User!.Id == UserId)
                .Where(c => ids.Contains(c.Id))
                .ToListAsync();
            if (characters.IsNullOrEmpty())
            {
                response.Success = false;
                response.Message = "The Given Characters doesn't Exist or Don't Belong to Current User";
            }
            else
            {
                await GroupDelete(characters);
                response.Message = "Characters successfully Removed";
            }
        }
        catch (Exception e)
        {
            response.Message = e.Message;
            response.Success = false;
        }

        return response;
    }

    public async Task<ServiceResponse<IEnumerable<Skill>>> AddCharacterSkill(int characterId, int skillId)
    {
        var response = new ServiceResponse<IEnumerable<Skill>>();
        try
        {
            var character = await GetACharacter(characterId, response);
            var skill = await GetSkill(skillId, response);
            if (!response.Success) return response;
            
            if (character!.Skills!.Contains(skill!))
            {
                response.Message = "This Character Already has the requested Skill!";
                response.Success = false;
                return response;
            }

            character.Skills!.Add(skill!);
            await Update(character);
            response.Data = character.Skills;
        }
        catch (Exception e)
        {
            response.Message += e.Message;
            response.Success = false;
        }

        return response;
    }

    public async Task<ServiceResponse<IEnumerable<Skill>>> RemoveCharacterSkill(int characterId, int skillId)
    {
        var response = new ServiceResponse<IEnumerable<Skill>>();
        try
        {
            var character = await GetACharacter(characterId, response);
            var skill = await GetSkill(skillId, response);
            if (!response.Success) return response;

            character!.Skills!.Remove(skill!);
            await Update(character);
            response.Data = character.Skills;
        }
        catch (Exception e)
        {
            response.Message += e.Message;
            response.Success = false;
        }

        return response;
    }

    private async Task<Skill?> GetSkill<T>(int skillId, ServiceResponse<T> response)
    {
        var skill = await _skillRepository.GetById(skillId);

        if (skill == null)
        {
            response.Message += " This Skill doesn't exists.";
            response.Success = false;
        }

        return skill;
    }

    private async Task<Character?> GetACharacter<T>(int characterId, ServiceResponse<T> response)
    {
        var character = await Filter(c => c.User!.Id == UserId)
            .FirstOrDefaultAsync(c => c.Id == characterId);
        
        if (character == null)
        {
            response.Message += " This Character doesn't belong to the current User or doesn't exists.";
            response.Success = false;
        }

        return character;
    }
}