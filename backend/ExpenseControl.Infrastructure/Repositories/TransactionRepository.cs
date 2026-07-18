using ExpenseControl.Domain.Entities;
using ExpenseControl.Domain.Repositories;
using ExpenseControl.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseControl.Infrastructure.Repositories;

public sealed class TransactionRepository : ITransactionRepository
{
    private readonly ExpenseControlDbContext _context;

    public TransactionRepository(ExpenseControlDbContext context)
    {
        _context = context;
    }

    public async Task<Transaction> AddAsync(Transaction transaction)
    {
        await _context.Transactions.AddAsync(transaction);
        await _context.SaveChangesAsync();

        return transaction;
    }

    public async Task<List<Transaction>> GetAllAsync()
    {
        return await _context.Transactions
            .AsNoTracking()
            .OrderByDescending(transaction => transaction.Id)
            .ToListAsync();
    }

    public async Task<List<Transaction>> GetByPersonIdAsync(int personId)
    {
        return await _context.Transactions
            .AsNoTracking()
            .Where(transaction => transaction.PersonId == personId)
            .ToListAsync();
    }
}