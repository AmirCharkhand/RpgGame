using Microsoft.EntityFrameworkCore;
using RPG.Infrastructure.Data.Paging;

namespace RPG.Infrastructure.Extensions;

public static class PagingExtension
{
    public static async Task<PagedList<T>> CalculatePaging<T>(this IQueryable<T> query, PagingParam? pagingParam)
    {
        pagingParam ??= new PagingParam();
        var totalCount = await query.CountAsync();
        var result = await query
            .Skip(pagingParam.PageSize * (pagingParam.PageIndex - 1))
            .Take(pagingParam.PageSize)
            .ToListAsync();

        return new PagedList<T>(result, totalCount);
    }
}