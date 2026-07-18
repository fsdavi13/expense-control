namespace ExpenseControl.Application.DTOs.Totals;

public sealed class TotalsResponseDto
{
    public List<PersonTotalsDto> Persons { get; set; } = [];

    public GeneralTotalsDto General { get; set; } = new();
}