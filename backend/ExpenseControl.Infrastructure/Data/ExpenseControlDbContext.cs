using ExpenseControl.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseControl.Infrastructure.Data;

public class ExpenseControlDbContext : DbContext
{
    public DbSet<Person> Persons { get; set; }

    public DbSet<Transaction> Transactions { get; set; }

    public ExpenseControlDbContext(
        DbContextOptions<ExpenseControlDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // As configurações das entidades ficam separadas
        // para manter o DbContext organizado e facilitar manutenção.
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ExpenseControlDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}