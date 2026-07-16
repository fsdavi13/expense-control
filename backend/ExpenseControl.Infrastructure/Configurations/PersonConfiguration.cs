using ExpenseControl.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseControl.Infrastructure.Configurations;

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable("Persons");

        builder.HasKey(person => person.Id);

        builder.Property(person => person.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(person => person.Age)
            .IsRequired();

        // Uma pessoa pode possuir várias transações.
        // A exclusão em cascata garante que, ao remover uma pessoa,
        // todos os registros financeiros associados também sejam removidos.
        builder.HasMany(person => person.Transactions)
            .WithOne(transaction => transaction.Person)
            .HasForeignKey(transaction => transaction.PersonId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}