using Microsoft.EntityFrameworkCore;

namespace ExpenseControl.Infrastructure.Data;

public sealed class PostgresExpenseControlDbContext
    : ExpenseControlDbContext
{
    public PostgresExpenseControlDbContext(
        DbContextOptions<PostgresExpenseControlDbContext> options)
        : base(options)
    {
    }
}