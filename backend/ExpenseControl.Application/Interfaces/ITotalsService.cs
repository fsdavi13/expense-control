using ExpenseControl.Application.DTOs.Totals;

namespace ExpenseControl.Application.Interfaces;

public interface ITotalsService
{
    Task<TotalsResponseDto> GetAsync();
}