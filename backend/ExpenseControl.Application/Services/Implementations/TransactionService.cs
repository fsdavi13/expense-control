using ExpenseControl.Application.DTOs.Transaction;
using ExpenseControl.Application.Exceptions;
using ExpenseControl.Application.Interfaces;
using ExpenseControl.Domain.Entities;
using ExpenseControl.Domain.Enums;
using ExpenseControl.Domain.Repositories;

namespace ExpenseControl.Application.Services.Implementations;

public sealed class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IPersonRepository _personRepository;

    public TransactionService(
        ITransactionRepository transactionRepository,
        IPersonRepository personRepository)
    {
        _transactionRepository = transactionRepository;
        _personRepository = personRepository;
    }

    public async Task<TransactionResponseDto> CreateAsync(
        CreateTransactionDto dto)
    {
        ValidateTransaction(dto);

        var person = await _personRepository.GetByIdAsync(dto.PersonId);

        if (person is null)
        {
            throw new NotFoundException("Pessoa não encontrada.");
        }

        // Menores de idade podem cadastrar apenas despesas.
        if (person.Age < 18 && dto.Type == TransactionType.Income)
        {
            throw new BusinessException(
                "Menores de idade não podem cadastrar receitas.");
        }

        var transaction = new Transaction(
            dto.Description.Trim(),
            dto.Amount,
            dto.Type,
            dto.PersonId);

        await _transactionRepository.AddAsync(transaction);

        return MapToResponse(transaction);
    }

    public async Task<List<TransactionResponseDto>> GetAllAsync()
    {
        var transactions = await _transactionRepository.GetAllAsync();

        return transactions
            .Select(MapToResponse)
            .ToList();
    }

    private static void ValidateTransaction(CreateTransactionDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Description))
        {
            throw new BusinessException(
                "A descrição da transação é obrigatória.");
        }

        if (dto.Amount <= 0)
        {
            throw new BusinessException(
                "O valor da transação deve ser maior que zero.");
        }

        if (!Enum.IsDefined(dto.Type))
        {
            throw new BusinessException(
                "O tipo da transação é inválido.");
        }

        if (dto.PersonId <= 0)
        {
            throw new BusinessException(
                "Informe uma pessoa válida.");
        }
    }

    private static TransactionResponseDto MapToResponse(
        Transaction transaction)
    {
        return new TransactionResponseDto
        {
            Id = transaction.Id,
            Description = transaction.Description,
            Amount = transaction.Amount,
            Type = transaction.Type,
            PersonId = transaction.PersonId
        };
    }
}