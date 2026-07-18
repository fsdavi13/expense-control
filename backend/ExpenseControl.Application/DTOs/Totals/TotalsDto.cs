namespace ExpenseControl.Application.DTOs.Totals;

public sealed class TotalsDto
{
    public List<PersonTotalsDto> Persons { get; set; } = [];

    public decimal TotalIncome { get; set; }

    public decimal TotalExpense { get; set; }

    public decimal NetBalance { get; set; }
}