namespace MET.Service.Domain.Enums;

public enum ExpenseType : int
{
    Unknown = 0,
    Groceries = 1,
    EatOut = 2,
    Transportation = 3,
    Insurance = 4
}

public static class ExpenseTypeExtensions
{
    public static string ToDisplayKey(this ExpenseType type) =>
        type switch
        {
            ExpenseType.Unknown => "expense.type.unknown",
            ExpenseType.EatOut => "expense.type.eatOut",
            ExpenseType.Groceries => "expense.type.groceries",
            ExpenseType.Transportation => "expense.type.transportation",
            ExpenseType.Insurance => "expense.type.insurance",
            _ => "expense.type.unknown"
        };
}