namespace MET.Service.Application.DTOs;

public class ExpenseDto
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public decimal? Amount { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
}