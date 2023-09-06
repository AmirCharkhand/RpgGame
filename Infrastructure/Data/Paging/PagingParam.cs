namespace RPG.Infrastructure.Data.Paging;

public class PagingParam
{
    private int _pageIndex;
    private int _pageSize;
    private const int MaxPageSize = 50;
    private const int DefaultPageSize = 10;

    public PagingParam()
    {
        _pageSize = DefaultPageSize;
        _pageIndex = 1;
    }

    public int PageIndex
    {
        get => _pageIndex;
        set => _pageIndex = value > 0 ? value : 1;
    }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }
}