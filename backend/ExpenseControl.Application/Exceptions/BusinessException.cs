namespace ExpenseControl.Application.Exceptions;

/// <summary>
/// Representa uma violação de regra de negócio.
/// O middleware da API será responsável por converter essa exceção
/// em uma resposta HTTP adequada.
/// </summary>
public sealed class BusinessException : Exception
{
    public BusinessException(string message)
        : base(message)
    {
    }
}