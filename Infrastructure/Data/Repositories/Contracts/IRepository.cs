using System.Linq.Expressions;
using RPG.Application.Models;
using RPG.Domain.Core;
using RPG.Infrastructure.Data.Paging;
using RPG.Infrastructure.Data.Services;

namespace RPG.Infrastructure.Data.Repositories.Contracts;

public interface IRepository<T,TKey> where T : Entity<TKey>
{
    Task<T?> GetById(TKey id);
    Task<List<T>> GetAll();
    IQueryable<T> Filter(Expression<Func<T, bool>> expression);
    Task<bool> IsExist(Expression<Func<T, bool>> expression);
    Task<ServiceResponse<PagedList<T>>> Search(string searchText, SortDto? sortDto, PagingParam? pagingParam = default);
    Task Add(T entity);
    Task Update(T entity);
    Task GroupUpdate(List<T> entities);
    Task Delete(T entity);
    Task GroupDelete(List<T> entities);
}