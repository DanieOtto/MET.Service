namespace MET.Service.Application.DTOs;

public sealed record ExpenseQuery(
    int PageNumber = 1,
    int PageSize = 50,
    string? Search = null,
    string? Status = null,         
    DateOnly? FromDate = null,
    DateOnly? ToDate = null,
    string? Sort = null            
)
{
    public int Skip => Math.Max(0, (PageNumber - 1) * PageSize);
    public int Take => Math.Clamp(PageSize, 1, 200);
}