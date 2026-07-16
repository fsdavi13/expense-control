using ExpenseControl.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseControl.Infrastructure.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transactions");

        builder.HasKey(transaction => transaction.Id);

        builder.Property(transaction => transaction.Description)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(transaction => transaction.Amount)
            .IsRequired()
            .HasColumnType("decimal(10,2)");

        builder.Property(transaction => transaction.Type)
            .IsRequired();
    }
}