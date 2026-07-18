namespace ExpenseControl.Application.DTOs.Totals;

public sealed class GeneralTotalsDto
{
    public decimal TotalIncome { get; set; }

    public decimal TotalExpense { get; set; }

    public decimal Balance => TotalIncome - TotalExpense;
}