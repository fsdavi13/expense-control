using ExpenseControl.Domain.Entities;

namespace ExpenseControl.Domain.Repositories;

public interface ITransactionRepository
{
    Task<Transaction> AddAsync(Transaction transaction);

    Task<List<Transaction>> GetAllAsync();

    Task<List<Transaction>> GetByPersonIdAsync(int personId);

    Task<Transaction?> GetByIdAsync(int id);

    Task DeleteAsync(Transaction transaction);
}