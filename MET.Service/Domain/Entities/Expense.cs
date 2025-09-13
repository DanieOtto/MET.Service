namespace MET.Service.Domain.Entities;

public class Expense
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public decimal? Amount { get; set; }
}