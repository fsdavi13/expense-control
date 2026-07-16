using ExpenseControl.Domain.Enums;

namespace ExpenseControl.Domain.Entities;

public class Transaction
{
    public int Id { get; private set; }

    public string Description { get; private set; } = string.Empty;

    public decimal Amount { get; private set; }

    public TransactionType Type { get; private set; }

    public int PersonId { get; private set; }

    // Referência para a pessoa responsável pela movimentação.
    // O vínculo garante que toda transação pertença a um cadastro existente.
    public Person Person { get; private set; } = null!;

    private Transaction()
    {
        // Necessário para o Entity Framework Core reconstruir
        // objetos ao consultar os dados persistidos.
    }

    public Transaction(
        string description,
        decimal amount,
        TransactionType type,
        int personId)
    {
        Description = description;
        Amount = amount;
        Type = type;
        PersonId = personId;
    }
}