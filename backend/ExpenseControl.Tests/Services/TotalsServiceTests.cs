using ExpenseControl.Application.Services.Implementations;
using ExpenseControl.Domain.Entities;
using ExpenseControl.Domain.Enums;
using ExpenseControl.Domain.Repositories;
using ExpenseControl.Tests.Support;
using Moq;

namespace ExpenseControl.Tests.Services;

public sealed class TotalsServiceTests
{
    private readonly Mock<IPersonRepository> _personRepositoryMock;
    private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
    private readonly TotalsService _totalsService;

    public TotalsServiceTests()
    {
        _personRepositoryMock = new Mock<IPersonRepository>();
        _transactionRepositoryMock = new Mock<ITransactionRepository>();

        _totalsService = new TotalsService(
            _personRepositoryMock.Object,
            _transactionRepositoryMock.Object);
    }

    [Fact]
    public async Task GetAsync_ShouldCalculatePersonAndGeneralTotals()
    {
        var people = new List<Person>
        {
            TestEntityFactory.CreatePerson(
                id: 1,
                name: "Carlos Silva",
                age: 30),

            TestEntityFactory.CreatePerson(
                id: 2,
                name: "Lucas Souza",
                age: 16),

            TestEntityFactory.CreatePerson(
                id: 3,
                name: "Ana Santos",
                age: 25)
        };

        var transactions = new List<Transaction>
        {
            TestEntityFactory.CreateTransaction(
                id: 1,
                description: "Salário",
                amount: 3000m,
                type: TransactionType.Income,
                personId: 1),

            TestEntityFactory.CreateTransaction(
                id: 2,
                description: "Conta de energia",
                amount: 250.50m,
                type: TransactionType.Expense,
                personId: 1),

            TestEntityFactory.CreateTransaction(
                id: 3,
                description: "Material escolar",
                amount: 80m,
                type: TransactionType.Expense,
                personId: 2)
        };

        _personRepositoryMock
            .Setup(repository => repository.GetAllAsync())
            .ReturnsAsync(people);

        _transactionRepositoryMock
            .Setup(repository => repository.GetAllAsync())
            .ReturnsAsync(transactions);

        var result = await _totalsService.GetAsync();

        Assert.Equal(3, result.Persons.Count);

        var carlosTotals = result.Persons[0];

        Assert.Equal(1, carlosTotals.PersonId);
        Assert.Equal("Carlos Silva", carlosTotals.PersonName);
        Assert.Equal(3000m, carlosTotals.TotalIncome);
        Assert.Equal(250.50m, carlosTotals.TotalExpense);
        Assert.Equal(2749.50m, carlosTotals.Balance);

        var lucasTotals = result.Persons[1];

        Assert.Equal(2, lucasTotals.PersonId);
        Assert.Equal("Lucas Souza", lucasTotals.PersonName);
        Assert.Equal(0m, lucasTotals.TotalIncome);
        Assert.Equal(80m, lucasTotals.TotalExpense);
        Assert.Equal(-80m, lucasTotals.Balance);

        var anaTotals = result.Persons[2];

        Assert.Equal(3, anaTotals.PersonId);
        Assert.Equal("Ana Santos", anaTotals.PersonName);
        Assert.Equal(0m, anaTotals.TotalIncome);
        Assert.Equal(0m, anaTotals.TotalExpense);
        Assert.Equal(0m, anaTotals.Balance);

        Assert.Equal(3000m, result.General.TotalIncome);
        Assert.Equal(330.50m, result.General.TotalExpense);
        Assert.Equal(2669.50m, result.General.Balance);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnEmptyTotals_WhenThereAreNoPeople()
    {
        _personRepositoryMock
            .Setup(repository => repository.GetAllAsync())
            .ReturnsAsync([]);

        _transactionRepositoryMock
            .Setup(repository => repository.GetAllAsync())
            .ReturnsAsync([]);

        var result = await _totalsService.GetAsync();

        Assert.Empty(result.Persons);
        Assert.Equal(0m, result.General.TotalIncome);
        Assert.Equal(0m, result.General.TotalExpense);
        Assert.Equal(0m, result.General.Balance);
    }
}