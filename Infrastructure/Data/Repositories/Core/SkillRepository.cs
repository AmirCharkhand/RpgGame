using RPG.Domain.Models;
using RPG.Infrastructure.Data.Repositories.Contracts;

namespace RPG.Infrastructure.Data.Repositories.Core;

public class SkillRepository : Repository<Skill,int> , ISkillRepository
{
    public SkillRepository(DataContext dbContext, IHttpContextAccessor contextAccessor) : base(dbContext, contextAccessor)
    {
    }
}