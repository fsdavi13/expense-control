namespace ExpenseControl.Domain.Entities;

public sealed class Person
{
    public int Id { get; private set; }

    public string Name { get; private set; }

    public int Age { get; private set; }

    public ICollection<Transaction> Transactions { get; private set; }
        = new List<Transaction>();

    private Person()
    {
        Name = string.Empty;
    }

    public Person(string name, int age)
    {
        Name = name;
        Age = age;
    }

    public void Update(string name, int age)
    {
        // O identificador não é alterado, preservando as transações
        // que já estão associadas à pessoa.
        Name = name;
        Age = age;
    }
}