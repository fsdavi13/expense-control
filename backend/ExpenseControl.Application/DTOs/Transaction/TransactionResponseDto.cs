using ExpenseControl.Domain.Enums;

namespace ExpenseControl.Application.DTOs.Transaction;

public sealed class TransactionResponseDto
{
    public int Id { get; set; }

    public string Description { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public TransactionType Type { get; set; }

    public int PersonId { get; set; }
}