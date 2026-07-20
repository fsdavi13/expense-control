using ExpenseControl.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseControl.Infrastructure.Data;

public class ExpenseControlDbContext : DbContext
{
    public ExpenseControlDbContext(DbContextOptions options)
        : base(options)
    {
    }

    public DbSet<Person> Persons => Set<Person>();

    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ExpenseControlDbContext).Assembly);
    }
}