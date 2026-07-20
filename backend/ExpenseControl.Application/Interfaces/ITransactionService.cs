using ExpenseControl.Application.DTOs.Transaction;

namespace ExpenseControl.Application.Interfaces;

public interface ITransactionService
{
    Task<TransactionResponseDto> CreateAsync(
        CreateTransactionDto dto);

    Task<List<TransactionResponseDto>> GetAllAsync();

    Task DeleteAsync(int id);
}