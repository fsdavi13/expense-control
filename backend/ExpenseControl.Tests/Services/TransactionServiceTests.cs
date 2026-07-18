using ExpenseControl.Application.DTOs.Transaction;
using ExpenseControl.Application.Exceptions;
using ExpenseControl.Application.Services.Implementations;
using ExpenseControl.Domain.Entities;
using ExpenseControl.Domain.Enums;
using ExpenseControl.Domain.Repositories;
using ExpenseControl.Tests.Support;
using Moq;

namespace ExpenseControl.Tests.Services;

public sealed class TransactionServiceTests
{
    private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
    private readonly Mock<IPersonRepository> _personRepositoryMock;
    private readonly TransactionService _transactionService;

    public TransactionServiceTests()
    {
        _transactionRepositoryMock = new Mock<ITransactionRepository>();
        _personRepositoryMock = new Mock<IPersonRepository>();

        _transactionService = new TransactionService(
            _transactionRepositoryMock.Object,
            _personRepositoryMock.Object);
    }

    [Theory]
    [InlineData(TransactionType.Expense)]
    [InlineData(TransactionType.Income)]
    public async Task CreateAsync_ShouldCreateTransaction_WhenAdultAndDataIsValid(
        TransactionType type)
    {
        var person = TestEntityFactory.CreatePerson(
            id: 1,
            name: "Carlos Silva",
            age: 30);

        Transaction? createdTransaction = null;

        _personRepositoryMock
            .Setup(repository => repository.GetByIdAsync(1))
            .ReturnsAsync(person);

        _transactionRepositoryMock
            .Setup(repository =>
                repository.AddAsync(It.IsAny<Transaction>()))
            .Returns((Transaction transaction) =>
            {
                createdTransaction = transaction;

                return Task.FromResult(transaction);
            });

        var dto = new CreateTransactionDto
        {
            Description = "  Pagamento mensal  ",
            Amount = 500m,
            Type = type,
            PersonId = 1
        };

        var result = await _transactionService.CreateAsync(dto);

        Assert.NotNull(createdTransaction);
        Assert.Equal("Pagamento mensal", createdTransaction!.Description);
        Assert.Equal(500m, createdTransaction.Amount);
        Assert.Equal(type, createdTransaction.Type);
        Assert.Equal(1, createdTransaction.PersonId);

        Assert.Equal("Pagamento mensal", result.Description);
        Assert.Equal(500m, result.Amount);
        Assert.Equal(type, result.Type);
        Assert.Equal(1, result.PersonId);

        _transactionRepositoryMock.Verify(
            repository =>
                repository.AddAsync(It.IsAny<Transaction>()),
            Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldAllowExpense_WhenPersonIsMinor()
    {
        var person = TestEntityFactory.CreatePerson(
            id: 2,
            name: "Lucas Souza",
            age: 16);

        _personRepositoryMock
            .Setup(repository => repository.GetByIdAsync(2))
            .ReturnsAsync(person);

        _transactionRepositoryMock
            .Setup(repository =>
                repository.AddAsync(It.IsAny<Transaction>()))
            .ReturnsAsync((Transaction transaction) => transaction);

        var dto = new CreateTransactionDto
        {
            Description = "Material escolar",
            Amount = 80m,
            Type = TransactionType.Expense,
            PersonId = 2
        };

        var result = await _transactionService.CreateAsync(dto);

        Assert.Equal("Material escolar", result.Description);
        Assert.Equal(80m, result.Amount);
        Assert.Equal(TransactionType.Expense, result.Type);
        Assert.Equal(2, result.PersonId);

        _transactionRepositoryMock.Verify(
            repository =>
                repository.AddAsync(It.IsAny<Transaction>()),
            Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowBusinessException_WhenMinorRegistersIncome()
    {
        var person = TestEntityFactory.CreatePerson(
            id: 2,
            name: "Lucas Souza",
            age: 16);

        _personRepositoryMock
            .Setup(repository => repository.GetByIdAsync(2))
            .ReturnsAsync(person);

        var dto = new CreateTransactionDto
        {
            Description = "Pagamento",
            Amount = 500m,
            Type = TransactionType.Income,
            PersonId = 2
        };

        var exception = await Assert.ThrowsAsync<BusinessException>(
            () => _transactionService.CreateAsync(dto));

        Assert.Equal(
            "Menores de idade não podem cadastrar receitas.",
            exception.Message);

        _transactionRepositoryMock.Verify(
            repository =>
                repository.AddAsync(It.IsAny<Transaction>()),
            Times.Never);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowNotFoundException_WhenPersonDoesNotExist()
    {
        _personRepositoryMock
            .Setup(repository => repository.GetByIdAsync(999))
            .ReturnsAsync((Person?)null);

        var dto = new CreateTransactionDto
        {
            Description = "Conta de energia",
            Amount = 200m,
            Type = TransactionType.Expense,
            PersonId = 999
        };

        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => _transactionService.CreateAsync(dto));

        Assert.Equal(
            "Pessoa não encontrada.",
            exception.Message);

        _transactionRepositoryMock.Verify(
            repository =>
                repository.AddAsync(It.IsAny<Transaction>()),
            Times.Never);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task CreateAsync_ShouldThrowBusinessException_WhenDescriptionIsInvalid(
        string invalidDescription)
    {
        var dto = new CreateTransactionDto
        {
            Description = invalidDescription,
            Amount = 100m,
            Type = TransactionType.Expense,
            PersonId = 1
        };

        var exception = await Assert.ThrowsAsync<BusinessException>(
            () => _transactionService.CreateAsync(dto));

        Assert.Equal(
            "A descrição da transação é obrigatória.",
            exception.Message);

        _personRepositoryMock.Verify(
            repository => repository.GetByIdAsync(It.IsAny<int>()),
            Times.Never);

        _transactionRepositoryMock.Verify(
            repository =>
                repository.AddAsync(It.IsAny<Transaction>()),
            Times.Never);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task CreateAsync_ShouldThrowBusinessException_WhenAmountIsInvalid(
        decimal invalidAmount)
    {
        var dto = new CreateTransactionDto
        {
            Description = "Conta de energia",
            Amount = invalidAmount,
            Type = TransactionType.Expense,
            PersonId = 1
        };

        var exception = await Assert.ThrowsAsync<BusinessException>(
            () => _transactionService.CreateAsync(dto));

        Assert.Equal(
            "O valor da transação deve ser maior que zero.",
            exception.Message);

        _personRepositoryMock.Verify(
            repository => repository.GetByIdAsync(It.IsAny<int>()),
            Times.Never);

        _transactionRepositoryMock.Verify(
            repository =>
                repository.AddAsync(It.IsAny<Transaction>()),
            Times.Never);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowBusinessException_WhenTypeIsInvalid()
    {
        var dto = new CreateTransactionDto
        {
            Description = "Conta de energia",
            Amount = 100m,
            Type = (TransactionType)999,
            PersonId = 1
        };

        var exception = await Assert.ThrowsAsync<BusinessException>(
            () => _transactionService.CreateAsync(dto));

        Assert.Equal(
            "O tipo da transação é inválido.",
            exception.Message);

        _personRepositoryMock.Verify(
            repository => repository.GetByIdAsync(It.IsAny<int>()),
            Times.Never);

        _transactionRepositoryMock.Verify(
            repository =>
                repository.AddAsync(It.IsAny<Transaction>()),
            Times.Never);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowBusinessException_WhenPersonIdIsInvalid()
    {
        var dto = new CreateTransactionDto
        {
            Description = "Conta de energia",
            Amount = 100m,
            Type = TransactionType.Expense,
            PersonId = 0
        };

        var exception = await Assert.ThrowsAsync<BusinessException>(
            () => _transactionService.CreateAsync(dto));

        Assert.Equal(
            "Informe uma pessoa válida.",
            exception.Message);

        _personRepositoryMock.Verify(
            repository => repository.GetByIdAsync(It.IsAny<int>()),
            Times.Never);

        _transactionRepositoryMock.Verify(
            repository =>
                repository.AddAsync(It.IsAny<Transaction>()),
            Times.Never);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnMappedTransactions()
    {
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
                personId: 1)
        };

        _transactionRepositoryMock
            .Setup(repository => repository.GetAllAsync())
            .ReturnsAsync(transactions);

        var result = await _transactionService.GetAllAsync();

        Assert.Equal(2, result.Count);

        Assert.Equal(1, result[0].Id);
        Assert.Equal("Salário", result[0].Description);
        Assert.Equal(3000m, result[0].Amount);
        Assert.Equal(TransactionType.Income, result[0].Type);
        Assert.Equal(1, result[0].PersonId);

        Assert.Equal(2, result[1].Id);
        Assert.Equal("Conta de energia", result[1].Description);
        Assert.Equal(250.50m, result[1].Amount);
        Assert.Equal(TransactionType.Expense, result[1].Type);
        Assert.Equal(1, result[1].PersonId);
    }
}