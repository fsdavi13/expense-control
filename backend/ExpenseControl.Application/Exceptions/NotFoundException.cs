namespace ExpenseControl.Application.Exceptions;

/// <summary>
/// Representa a tentativa de acessar um recurso que não existe.
/// </summary>
public sealed class NotFoundException : Exception
{
    public NotFoundException(string message)
        : base(message)
    {
    }
}