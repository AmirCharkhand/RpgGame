using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RPG.Application.Models;
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
    public CharacterRepository(DataContext dbContext, IHttpContextAccessor contextAccessor, IExpressionBuilder expressionBuilder, IUserRepository userRepository, ISkillRepository skillRepository) : base(dbContext, contextAccessor)
    {
        _expressionBuilder = expressionBuilder;
        _userRepository = userRepository;
        _skillRepository = skillRepository;
    }

    public override async Task<ServiceResponse<PagedList<Character>>> Search(string searchText,SortDto? sortDto, PagingParam? pagingParam = default)
    {
        var response = new ServiceResponse<PagedList<Character>>();
        try
        {
            var result = await Filter(c => EF.Functions.Like(c.Name, $"%{searchText}%"))
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

    public async Task<ServiceResponse<PagedList<Character>>> GetAll(SortDto? sortDto, PagingParam? pagingParam = default)
    {
        var response = new ServiceResponse<PagedList<Character>>();
        try
        {
            var result = await Filter(c => c.User!.Id == UserId)
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
            var character = await Filter(c => c.User!.Id == UserId)
                    .FirstOrDefaultAsync(c => c.Id == id);
            if (character == null)
            {
                response.Success = false;
                response.Message = "The Given Character doesn't Exist or Doesn't Belong to Current User";
            }
            else
            {
                await Delete(character);
                response.Message = "Character successfully Removed";
            }
        }
        catch (Exception e)
        {
            response.Message = e.Message;
            response.Success = false;
        }

        return response;
    }
    
    public async Task<ServiceResponse<Character>> AddCharacterSkill(int characterId, int skillId)
    {
        var response = new ServiceResponse<Character>();
        try
        {
            var character = await Filter(c => c.User!.Id == UserId)
                .FirstOrDefaultAsync(c => c.Id == characterId);
            var skill = await _skillRepository.GetById(skillId);
            
            if (character == null)
            {
                response.Message = "This Character doesn't belong to the current User or doesn't exists";
                response.Success = false;
                return response;
            }
            if (skill == null)
            {
                response.Message = "This Skill doesn't exists";
                response.Success = false;
                return response;
            }

            character.Skills!.Add(skill);
            await Update(character);
            response.Data = character;
        }
        catch (Exception e)
        {
            response.Message = e.Message;
            response.Success = false;
        }

        return response;
    }
}