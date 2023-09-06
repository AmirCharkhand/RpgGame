using System.Linq.Expressions;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using RPG.Application.Models;
using RPG.Domain.Core;
using RPG.Infrastructure.Data.Paging;
using RPG.Infrastructure.Data.Repositories.Contracts;
using RPG.Infrastructure.Data.Services;

namespace RPG.Infrastructure.Data.Repositories.Core;

public class Repository<T, TKey> : IRepository<T, TKey> where T : Entity<TKey>
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly DataContext _dbContext;
    protected readonly DbSet<T> Set;

    protected int UserId => int.Parse(_contextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier));

    protected Repository(DataContext dbContext, IHttpContextAccessor contextAccessor)
    {
        _dbContext = dbContext;
        _contextAccessor = contextAccessor;
        Set = dbContext.Set<T>();
    }

    public async Task<T?> GetById(TKey id)
    {
        return await Set.FindAsync(id);
    }

    public IQueryable<T> Filter(Expression<Func<T, bool>> expression)
    {
        return Set
            .Where(expression);
    }

    public virtual Task<ServiceResponse<PagedList<T>>> Search(string searchText,SortDto? sortDto, PagingParam? pagingParam = default)
    {
        throw new NotImplementedException();
    }

    public async Task Add(T entity)
    {
        Set.Add(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Update(T entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task GroupUpdate(List<T> entities)
    {
        foreach (T entity in entities) _dbContext.Entry(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task Delete(T entity)
    {
        Set.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }
}