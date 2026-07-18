using ExpenseControl.Application.DTOs.Totals;
using ExpenseControl.Application.Interfaces;
using ExpenseControl.Domain.Enums;
using ExpenseControl.Domain.Repositories;

namespace ExpenseControl.Application.Services.Implementations;

public sealed class TotalsService : ITotalsService
{
    private readonly IPersonRepository _personRepository;
    private readonly ITransactionRepository _transactionRepository;

    public TotalsService(
        IPersonRepository personRepository,
        ITransactionRepository transactionRepository)
    {
        _personRepository = personRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task<TotalsResponseDto> GetAsync()
    {
        var persons = await _personRepository.GetAllAsync();
        var transactions = await _transactionRepository.GetAllAsync();

        var personTotals = persons.Select(person =>
        {
            var personTransactions = transactions
                .Where(transaction => transaction.PersonId == person.Id)
                .ToList();

            var income = personTransactions
                .Where(transaction => transaction.Type == TransactionType.Income)
                .Sum(transaction => transaction.Amount);

            var expense = personTransactions
                .Where(transaction => transaction.Type == TransactionType.Expense)
                .Sum(transaction => transaction.Amount);

            return new PersonTotalsDto
            {
                PersonId = person.Id,
                PersonName = person.Name,
                TotalIncome = income,
                TotalExpense = expense
            };
        }).ToList();

        var general = new GeneralTotalsDto
        {
            TotalIncome = personTotals.Sum(person => person.TotalIncome),
            TotalExpense = personTotals.Sum(person => person.TotalExpense)
        };

        return new TotalsResponseDto
        {
            Persons = personTotals,
            General = general
        };
    }
}