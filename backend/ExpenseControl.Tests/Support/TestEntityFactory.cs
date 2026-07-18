using ExpenseControl.Domain.Entities;
using ExpenseControl.Domain.Enums;

namespace ExpenseControl.Tests.Support;

internal static class TestEntityFactory
{
    public static Person CreatePerson(
        int id,
        string name,
        int age)
    {
        var person = new Person(name, age);

        SetId(person, id);

        return person;
    }

    public static Transaction CreateTransaction(
        int id,
        string description,
        decimal amount,
        TransactionType type,
        int personId)
    {
        var transaction = new Transaction(
            description,
            amount,
            type,
            personId);

        SetId(transaction, id);

        return transaction;
    }

    private static void SetId<T>(T entity, int id)
    {
        var idProperty = typeof(T).GetProperty("Id");

        var setter = idProperty?.GetSetMethod(nonPublic: true)
            ?? throw new InvalidOperationException(
                $"A entidade {typeof(T).Name} não possui um identificador configurável.");

        setter.Invoke(entity, new object[] { id });
    }
}