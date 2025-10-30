namespace MET.Service.Application.DTOs.Paging;

public sealed record PagedList<T>(
    List<T> Items,
    int PageNumber,
    int PageSize,
    int TotalCount)
{
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}

public sealed record PageQuery(int PageNumber = 1, int PageSize = 50)
{
    public int Skip => (PageNumber - 1) * PageSize;
    public int Take => PageSize;
}