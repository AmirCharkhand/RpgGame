namespace RPG.Infrastructure.Data.Paging;

public class PagedList<T> : List<T>
{
    public int TotalCount { get; }

    public PagedList(IEnumerable<T> items, int totalCount)
    {
        AddRange(items);
        TotalCount = totalCount;
    }
}