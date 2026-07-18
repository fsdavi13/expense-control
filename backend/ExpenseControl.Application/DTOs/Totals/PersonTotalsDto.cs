namespace ExpenseControl.Application.DTOs.Totals;

public sealed class PersonTotalsDto
{
    public int PersonId { get; set; }

    public string PersonName { get; set; } = string.Empty;

    public decimal TotalIncome { get; set; }

    public decimal TotalExpense { get; set; }

    public decimal Balance => TotalIncome - TotalExpense;
}