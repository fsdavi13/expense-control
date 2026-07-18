using System.ComponentModel.DataAnnotations;
using ExpenseControl.Domain.Enums;

namespace ExpenseControl.Application.DTOs.Transaction;

public sealed class CreateTransactionDto
{
    [Required(ErrorMessage = "A descrição é obrigatória.")]
    [MaxLength(
        200,
        ErrorMessage = "A descrição deve possuir no máximo 200 caracteres.")]
    public string Description { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    [EnumDataType(
        typeof(TransactionType),
        ErrorMessage = "Informe um tipo de transação válido.")]
    public TransactionType Type { get; set; }

    [Range(
        1,
        int.MaxValue,
        ErrorMessage = "Informe uma pessoa válida.")]
    public int PersonId { get; set; }
}